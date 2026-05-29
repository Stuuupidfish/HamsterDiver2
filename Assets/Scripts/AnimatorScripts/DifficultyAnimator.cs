using UnityEngine;
using UnityEngine.UI;

public class DifficultyAnimator : MonoBehaviour
{
    [SerializeField] private GameObject[] difficultyObjs = new GameObject[5];

    [Header("Wave Motion")]
    [SerializeField, Min(0f)] private float jumpHeight = 18f;
    [SerializeField, Min(0.01f)] private float waveSpeed = 3f;
    [SerializeField, Min(0f)] private float waveSpacing = 0.22f;
    [SerializeField, Min(0.01f)] private float burstDuration = 1.25f;
    [SerializeField, Min(0f)] private float pauseDuration = 0.85f;
    [SerializeField, Min(0f)] private float blendDuration = 0.2f;

    [Header("Infinite Level")]
    [SerializeField] private bool enableSpriteCycle;
    [SerializeField] private Sprite fullStarSprite;
    [SerializeField] private Sprite emptyStarSprite;
    [SerializeField, Range(0f, 1f)] private float fillThreshold = 0.9f;

    private Transform[] starTransforms;
    private RectTransform[] starRects;
    private Vector3[] baseLocalPositions;
    private Vector2[] baseAnchoredPositions;
    private Image[] starImages;
    private SpriteRenderer[] starRenderers;

    private void Awake()
    {
        CacheStars();
    }

    private void OnEnable()
    {
        CacheStars();
    }

    private void Start()
    {
        CacheStars();
    }

    private void CacheStars()
    {
        if (difficultyObjs == null)
        {
            return;
        }

        int count = difficultyObjs.Length;
        starTransforms = new Transform[count];
        starRects = new RectTransform[count];
        baseLocalPositions = new Vector3[count];
        baseAnchoredPositions = new Vector2[count];
        starImages = new Image[count];
        starRenderers = new SpriteRenderer[count];

        for (int i = 0; i < count; i++)
        {
            GameObject star = difficultyObjs[i];
            if (star == null)
            {
                continue;
            }

            Transform starTransform = star.transform;
            starTransforms[i] = starTransform;
            starRects[i] = starTransform as RectTransform;
            starImages[i] = star.GetComponent<Image>();
            starRenderers[i] = star.GetComponent<SpriteRenderer>();

            if (starRects[i] != null)
            {
                baseAnchoredPositions[i] = starRects[i].anchoredPosition;
            }
            else
            {
                baseLocalPositions[i] = starTransform.localPosition;
            }
        }
    }

    private void Update()
    {
        if (starTransforms == null || starTransforms.Length == 0)
        {
            return;
        }

        float cycleDuration = burstDuration + pauseDuration;
        if (cycleDuration <= 0f)
        {
            return;
        }

        float cycleTime = Time.unscaledTime % cycleDuration;

        for (int i = 0; i < starTransforms.Length; i++)
        {
            Transform starTransform = starTransforms[i];
            if (starTransform == null)
            {
                continue;
            }

            float phaseOffset = i * waveSpacing;
            float starCycleTime = Mathf.Repeat(cycleTime - phaseOffset, cycleDuration);
            float envelope = GetBurstEnvelope(starCycleTime);
            float waveTime = starCycleTime * waveSpeed;
            float wave = Mathf.Sin(waveTime) * 0.5f + 0.5f;
            wave = Mathf.SmoothStep(0f, 1f, wave);
            float heightOffset = jumpHeight * wave * envelope;

            if (enableSpriteCycle)
            {
                UpdateSpriteState(i, wave, envelope);
            }

            RectTransform rectTransform = starRects[i];
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = baseAnchoredPositions[i] + new Vector2(0f, heightOffset);
            }
            else
            {
                starTransform.localPosition = baseLocalPositions[i] + new Vector3(0f, heightOffset, 0f);
            }
        }
    }

    private void UpdateSpriteState(int index, float wave, float envelope)
    {
        if (emptyStarSprite == null || fullStarSprite == null)
        {
            return;
        }

        bool shouldBeFull = envelope > 0f && wave >= fillThreshold;
        SetStarSprite(index, shouldBeFull ? fullStarSprite : emptyStarSprite);
    }

    private void SetStarSprite(int index, Sprite sprite)
    {
        Image image = starImages[index];
        if (image != null)
        {
            image.sprite = sprite;
            return;
        }

        SpriteRenderer spriteRenderer = starRenderers[index];
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;
        }
    }

    private float GetBurstEnvelope(float cycleTime)
    {
        if (cycleTime >= burstDuration)
        {
            return 0f;
        }

        if (blendDuration <= 0f)
        {
            return 1f;
        }

        float blend = Mathf.Min(blendDuration, burstDuration * 0.5f);
        if (blend <= 0f)
        {
            return 1f;
        }

        if (cycleTime < blend)
        {
            return Mathf.SmoothStep(0f, 1f, cycleTime / blend);
        }

        float fadeStart = burstDuration - blend;
        if (cycleTime > fadeStart)
        {
            return Mathf.SmoothStep(0f, 1f, (burstDuration - cycleTime) / blend);
        }

        return 1f;
    }
}