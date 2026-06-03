using System.Collections;
using UnityEngine;
using TMPro;
public class UI : MonoBehaviour
{
    public bool IsGameOver = false;
    // Start is called before the first frame update
    public TextMeshProUGUI scoreText;
    private Player player;
    private GameManager gameManager;
    [SerializeField] private GameObject gameOver; //text
    [SerializeField] private GameObject youWin; //text
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject paused; //text
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject nextLvlButton;
    [SerializeField] private GameObject fullStar;
    [SerializeField] private GameObject emptyStar;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip lose;

    private bool isPaused = false;
    public bool IsPaused
    {
        get { return isPaused; }
    }
    private bool hasWinBeenTriggered = false;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<Player>();
        scoreText.text = "Oxygen level: 100%";
        gameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !IsGameOver && !gameManager.PlayerWins)
        {
            if (!isPaused)
            {
                Pause();
                isPaused = true;
            }
            else
            {
                Resume();
                isPaused = false;
            }
        }
        scoreText.text = "Oxygen level: " + player.Oxygen + "%";
        if (player.Oxygen == 0 && !IsGameOver)
        {
            GameOver();
        }
        
        if (gameManager.PlayerWins && !hasWinBeenTriggered)
        {
            Win();
            hasWinBeenTriggered = true;
        }
    }
    public void GameOver()
    {
        gameOver.SetActive(true);
        IsGameOver = true;
        menu.SetActive(true);
        nextLvlButton.SetActive(false);
        audioSource.PlayOneShot(lose);
    }
    public void Win()
    {
        StartCoroutine(WinSequence());
    }

    private IEnumerator WinSequence()
    {
        youWin.SetActive(true);
        menu.SetActive(true);
        if (nextLvlButton != null)
        {
            nextLvlButton.SetActive(true);
        }
        audioSource.PlayOneShot(win);

        yield return new WaitForSecondsRealtime(1f);

        int starCount;
        if (player.Oxygen >= 65)
        {
            starCount = 3;
        }
        else if (player.Oxygen >= 50)
        {
            starCount = 2;
        }
        else if (player.Oxygen >= 25)
        {
            starCount = 1;
        }
        else
        {
            starCount = 0;
        }
        
        PlayerData.SetLevelScore(gameManager.CurrentLevel, starCount);
        GameObject[] starPrefabs = new GameObject[3];
        for (int i = 0; i < starCount; i++)
        {
            starPrefabs[i] = fullStar;
        }
        for (int i = starCount; i < 3; i++)
        {
            starPrefabs[i] = emptyStar;
        }

        for (int i = 0; i < starPrefabs.Length; i++)
        {
            SpawnStar(starPrefabs[i], canvas.transform, new Vector2(-100 + (i * 100), 25));
            if (i < starPrefabs.Length - 1)
            {
                yield return new WaitForSecondsRealtime(0.5f);
            }
        }
    }

    public GameObject SpawnStar(GameObject starPrefab, Transform parent, Vector2 anchoredPosition)
    {
        GameObject spawnedStar = Instantiate(starPrefab, parent, false);
        RectTransform starRectTransform = spawnedStar.GetComponent<RectTransform>();
        if (starRectTransform != null)
        {
            starRectTransform.anchoredPosition = anchoredPosition;
        }
        return spawnedStar;
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        paused.SetActive(true);
        menu.SetActive(true);
        nextLvlButton.SetActive(false);
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        paused.SetActive(false);
        menu.SetActive(false);
    }
}
