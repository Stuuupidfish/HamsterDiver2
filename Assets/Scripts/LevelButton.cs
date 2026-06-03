using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class LevelButton : MonoBehaviour, IPointerDownHandler
{
    private static readonly Color LockedButtonColor = new Color32(190, 190, 190, 255);
    private static readonly Color LockedTextColor = new Color32(200, 200, 200, 255);
    private static readonly Color UnlockedColor = Color.white;
    private static readonly Vector2[] StarOffsets =
    {
        new Vector2(-25f, -50f),
        new Vector2(0f, -50f),
        new Vector2(25f, -50f)
    };

    [SerializeField] private int levelIndex = 0;
    [SerializeField] private string sceneName;
    //THIS WILL REFACTOR SCENE CHANGES SO THAT MEANS MENUMANAGER BUTTON SCRIPTS MUST BE DETACHED
    [SerializeField] private bool[] scoreStars = new bool[3];
    [SerializeField] private GameObject emptyStarPrefab;
    [SerializeField] private GameObject fullStarPrefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip denyClip;

    [SerializeField] private bool isEndlessMode = false;
    [SerializeField] private TextMeshProUGUI endlessHighScoreText;
    private Button button;
    private readonly List<GameObject> spawnedScoreStars = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        Refresh();
        PlayerData.OnChanged += Refresh;
    }

    private void OnDestroy()
    {
        PlayerData.OnChanged -= Refresh;
    }

    public void Refresh()
    {
        bool unlocked = PlayerData.IsLevelUnlocked(levelIndex);
        int score = PlayerData.GetLevelScore(levelIndex);

        if (button != null)
        {
            button.interactable = true;
        }

        Image buttonImage = GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = unlocked ? UnlockedColor : LockedButtonColor;
        }

        TMP_Text buttonLabel = GetComponentInChildren<TMP_Text>(true);
        if (buttonLabel != null)
        {
            buttonLabel.color = unlocked ? UnlockedColor : LockedTextColor;
        }

        if (!isEndlessMode)
        {
            SetScoreStars(score);
            return;
        }
        DisplayEndlessHighScore(PlayerData.EndlessHighScore);
    }
    public void DisplayEndlessHighScore(float highScore)
    {
        if (!PlayerData.IsLevelUnlocked(levelIndex) || PlayerData.IsLevelBeaten(levelIndex) == false) // Only show score if the level is unlocked and beaten
        {
            return;
        }
        endlessHighScoreText.text = "Best: " + highScore.ToString("F2") + "m";
        endlessHighScoreText.gameObject.SetActive(true);
    }
    public void SetScoreStars(int score)
    {
        bool unlocked = PlayerData.IsLevelUnlocked(levelIndex);
        int starCount = score;

        for (int i = 0; i < scoreStars.Length; i++)
        {
            scoreStars[i] = i < starCount;
        }

        ClearSpawnedScoreStars();

        if (!unlocked || PlayerData.IsLevelBeaten(levelIndex) == false) // Only show stars if the level is unlocked and beaten
        {
            return;
        }

        for (int i = 0; i < StarOffsets.Length; i++)
        {
            bool isFullStar = i < starCount;
            GameObject starPrefab = GetStarPrefab(isFullStar);
            if (starPrefab == null)
            {
                continue;
            }

            GameObject spawnedStar = Instantiate(starPrefab, transform, false);
            RectTransform starRectTransform = spawnedStar.GetComponent<RectTransform>();
            if (starRectTransform != null)
            {
                starRectTransform.anchoredPosition = StarOffsets[i];
            }
            else
            {
                spawnedStar.transform.localPosition = StarOffsets[i];
            }

            spawnedScoreStars.Add(spawnedStar);
        }
    }

    private void ClearSpawnedScoreStars()
    {
        for (int i = 0; i < spawnedScoreStars.Count; i++)
        {
            if (spawnedScoreStars[i] != null)
            {
                Destroy(spawnedScoreStars[i]);
            }
        }

        spawnedScoreStars.Clear();
    }

    public bool[] GetScoreStars()
    {
        return scoreStars;
    }

    public GameObject GetStarPrefab(bool isFullStar)
    {
        return isFullStar ? fullStarPrefab : emptyStarPrefab;
    }

    public void SelectLevel()
    {
        if (!PlayerData.IsLevelUnlocked(levelIndex))
        {
            return;
        }

        if (!string.IsNullOrEmpty(sceneName))
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }
    }

    private IEnumerator LoadSceneCoroutine(string targetSceneName)
    {
        if (audioSource != null && click != null)
        {
            audioSource.PlayOneShot(click);
            yield return new WaitForSecondsRealtime(click.length);
        }
        else
        {
            yield return null;
        }

        SceneManager.LoadScene(targetSceneName);
    }

    private void PlayDenySound()
    {
        audioSource.PlayOneShot(denyClip);
        Debug.Log("sound");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerData.IsLevelUnlocked(levelIndex))
        {
            return;
        }
        Debug.Log("clicked");
        PlayDenySound();
    }

}
