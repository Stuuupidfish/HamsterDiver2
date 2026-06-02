using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndlessGameManager : MonoBehaviour
{
    private float bubbleTimer = 0f;


    [SerializeField] private GameObject[] enemies = new GameObject[5];
    // ORDER OF ENEMIES IN THE ARRAY:
    // 0 : school of fish (-1)
    // 1 : jellyfish (-15)
    // 2 : octopus (-5)
    // 3 : pufferfish (-10)
    // 4 : shark (-25)
    private int enemySelectionRange = 2; //start off by spwaning first 2
    private float totalDistanceTraveled = 0f;
    public float TotalDistanceTraveled
    {
        get {
            float scaled = totalDistanceTraveled * 10f; // scale to desired units
            return Mathf.Round(scaled * 100f) / 100f;    // round to hundredths (2 decimal places)
        }
    }
    private int[] spawnX = {-6,6};
    private bool damageSlowDown = false;
    public bool DamageSlowDown
    {
        get {return damageSlowDown;}
    }
    [SerializeField] private GameObject airBubble;
    [SerializeField] private GameObject bkg;
    private Vector2 lastSpawnPosition;
    private float defaultSpeed = 0.06f; //0.1f is the original speed, but will be changed for additinal levels so ill treat it as like the basis
    
    private float accelerationRate = 0.0005f; //the rate at which the game speeds up, the game will speed up by this amount every frame
    private float downSpeed;
    private float maxSpeed = 0.2f; //the max speed the game can reach
    public float DownSpeed
    {
        get {return downSpeed;}
    }
    private EndlessUI ui;

    private bool playerWins = false;
    public bool PlayerWins
    {
        get {return playerWins;}
    }

    [SerializeField] private AudioSource audioSource;


    [SerializeField] private float bubbleSpawnInterval = 1.5f;
    private float maxBubbleSpawnInterval = 0.5f; 
    [SerializeField] private float enemySpawnInterval = 5.5f;
    private float maxEnemySpawnInterval = 5f; //the minimum time between enemy spawns, the game will never spawn enemies faster than this
    private float prevSpeed;
    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<EndlessUI>();
        spawnNewEnemy();
        downSpeed = defaultSpeed;
        prevSpeed = downSpeed;

        // audioSource volume handling removed
    }

    // Update is called once per frame
    void Update()
    {
        if (!ui.IsPaused)
        {
            if (damageSlowDown)
            {
                downSpeed = 0.5f*prevSpeed;
            }
            else
            {
                downSpeed = prevSpeed;
            }

            if (totalDistanceTraveled < 3f)
            {
                enemySelectionRange = 2;
            }
            else if (totalDistanceTraveled < 9f)
            {
                enemySelectionRange = 3; 
            }
            else if (totalDistanceTraveled < 16f)
            {
                enemySelectionRange = 4; 
            }
            else
            {
                enemySelectionRange = 5; 
            }

            float currentY = bkg.GetComponent<Rigidbody2D>().position.y;
            if (!ui.IsGameOver)
            {
                bkg.GetComponent<Rigidbody2D>().position += new Vector2(0, -downSpeed);
                downSpeed = Mathf.Min(downSpeed + accelerationRate * Time.deltaTime, maxSpeed);
                bubbleSpawnInterval = Mathf.Max(bubbleSpawnInterval - 0.007f * Time.deltaTime, maxBubbleSpawnInterval);
                enemySpawnInterval = Mathf.Max(enemySpawnInterval - 0.05f * Time.deltaTime, maxEnemySpawnInterval);
                if (!damageSlowDown)
                    prevSpeed = downSpeed;
                Debug.Log("Current down speed: " + downSpeed);
                totalDistanceTraveled += downSpeed * Time.deltaTime;
                if (Vector2.Distance(bkg.GetComponent<Rigidbody2D>().position, lastSpawnPosition) >= enemySpawnInterval)
                {
                    spawnNewEnemy();
                }
                // Bubble spawn timer
                bubbleTimer += Time.deltaTime;
                if (bubbleTimer >= bubbleSpawnInterval)
                {
                    spawnAirBubble();
                    bubbleTimer = 0f;
                }
            }
            
            if (ui.IsGameOver)
            {
                audioSource.Stop();
            }
        }
    }

    //trigger the slowdown effect for 1 second
    public void TriggerSlowDown()
    {
        StartCoroutine(SlowDownCoroutine());
    }

    private IEnumerator SlowDownCoroutine()
    {
        damageSlowDown = true;
        yield return new WaitForSeconds(1.5f);
        damageSlowDown = false;
    }

    private void spawnNewEnemy()
    {
        GameObject newObject = Instantiate(enemies[UnityEngine.Random.Range(0, enemySelectionRange)], new Vector2(UnityEngine.Random.Range(-5,6), 6), Quaternion.identity);
        lastSpawnPosition = bkg.GetComponent<Rigidbody2D>().position;
    }

    private void spawnAirBubble()
    {
        GameObject bubble = Instantiate(airBubble, new Vector2(UnityEngine.Random.Range(-2,3),6), Quaternion.identity);
    }

    // music fade methods removed
}
