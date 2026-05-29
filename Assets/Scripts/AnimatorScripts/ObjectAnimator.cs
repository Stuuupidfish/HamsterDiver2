using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] private GameObject obj;

    [Header("Float Motion")]
    [SerializeField] private float bobAmplitude = 2f;
    [SerializeField] private float bobSpeed = 1.2f;
    [SerializeField] private float driftAmplitude = 1f;
    [SerializeField] private float driftSpeed = 0.8f;
    [SerializeField] private float rotationAmplitude = 1f;
    [SerializeField] private float rotationSpeed = 0.9f;

    [Header("Press Feedback")]
    [SerializeField, Range(0.8f, 1f)] private float pressedScale = 0.92f;
    [SerializeField, Range(1f, 1.5f)] private float hoveredScale = 1.1f;
    [SerializeField] private float scaleLerpSpeed = 12f;

    private RectTransform rectTransform;
    private Vector2 startAnchoredPosition;
    private Vector3 startScale;
    private float seedX;
    private float seedY;
    private float seedRotation;
    private bool isPressed;
    private bool isHovered;
    public bool IsHovered
    {
        get { return isHovered; }
    }

    private void Awake()
    {
        Transform target = obj != null ? obj.transform : transform;
        rectTransform = target as RectTransform;
        startScale = target.localScale;
        seedX = Random.Range(0f, 100f);
        seedY = Random.Range(0f, 100f);
        seedRotation = Random.Range(0f, 100f);
    }

    private void OnEnable()
    {
        CacheBasePosition();
    }

    private void Start()
    {
        CacheBasePosition();
    }

    private void CacheBasePosition()
    {
        if (rectTransform != null)
        {
            startAnchoredPosition = rectTransform.anchoredPosition;
        }
    }

    private void Update()
    {
        Transform target = obj != null ? obj.transform : transform;

        if (rectTransform != null)
        {
            float time = Time.unscaledTime;
            float offsetX = Mathf.Sin(time * driftSpeed + seedX) * driftAmplitude;
            float offsetY = Mathf.Sin(time * bobSpeed + seedY) * bobAmplitude;
            float rotation = Mathf.Sin(time * rotationSpeed + seedRotation) * rotationAmplitude;

            rectTransform.anchoredPosition = startAnchoredPosition + new Vector2(offsetX, offsetY);
            target.localRotation = Quaternion.Euler(0f, 0f, rotation);
        }

        float targetScale = isPressed ? pressedScale : (isHovered ? hoveredScale : 1f);
        target.localScale = Vector3.Lerp(target.localScale, startScale * targetScale, Time.unscaledDeltaTime * scaleLerpSpeed);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPressed = false;
        isHovered = false;
    }
}
