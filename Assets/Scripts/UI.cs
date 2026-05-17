using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject paused; //text
    [SerializeField] private GameObject menu;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip win;

    private bool isPaused = false;
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
        if(Input.GetKey(KeyCode.Escape))
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
        if (player.Oxygen == 0)
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
    }
    public void Win()
    {
        audioSource.PlayOneShot(win);
        youWin.SetActive(true);
        menu.SetActive(true);
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        paused.SetActive(true);
        menu.SetActive(true);
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        paused.SetActive(false);
        menu.SetActive(false);
    }
}
