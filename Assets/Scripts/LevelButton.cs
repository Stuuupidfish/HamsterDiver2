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

    [SerializeField] private int levelIndex = 0;
    [SerializeField] private string sceneName;
    //THIS WILL REFACTOR SCENE CHANGES SO THAT MEANS MENUMANAGER BUTTON SCRIPTS MUST BE DETACHED
    [SerializeField] private bool[] scoreStars = new bool[3];
    [SerializeField] private GameObject emptyStarPrefab;
    [SerializeField] private GameObject fullStarPrefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip denyClip;
    private Button button;
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

        SetScoreStars(score);
    }

    public void SetScoreStars(int score)
    {
        for (int i = 0; i < scoreStars.Length; i++)
        {
            scoreStars[i] = i < score;
        }
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
