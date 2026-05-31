using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int levelIndex = 0;
    [SerializeField] private string sceneName;
    //THIS WILL REFACTOR SCENE CHANGES SO THAT MEANS MENUMANAGER BUTTON SCRIPTS MUST BE DETACHED
    [SerializeField] private GameObject [] scoreStars = new GameObject[3];
    [SerializeField] private Sprite emptyStarSprite;
    [SerializeField] private Sprite fullStarSprite;
    [SerializeField] private AudioSource audioSource;
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

        // overlay handled in inspector (if present)

        foreach (GameObject star in scoreStars)
        {
            star.SetActive(false);
        }

        updateScoreStars(score);
    }

    public void updateScoreStars(int score)
    {
        for (int i = 0; i < score; i++)
        {
            SetSpriteOnGameObject(scoreStars[i], fullStarSprite);
        }
        for (int i = score; i < scoreStars.Length; i++)
        {
            SetSpriteOnGameObject(scoreStars[i], emptyStarSprite);
        }
    }

    public void SelectLevel()
    {
        if (!PlayerData.IsLevelUnlocked(levelIndex))
        {
            PlayDenySound();
            return;
        }

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private void PlayDenySound()
    {
        if (audioSource == null || denyClip == null)
        {
            return;
        }

        audioSource.PlayOneShot(denyClip);
    }

    private void SetSpriteOnGameObject(GameObject go, Sprite sprite)
    {
        if (go == null || sprite == null) return;

        // Try UI Image first
        Image img = go.GetComponent<Image>();
        if (img != null)
        {
            img.sprite = sprite;
            return;
        }

        // Fall back to SpriteRenderer
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = sprite;
            return;
        }
    }
}
