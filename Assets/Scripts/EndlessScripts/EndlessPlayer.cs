using Unity.VisualScripting;
using UnityEngine;

public class EndlessPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    private EndlessGameManager endlessGameManager;
    private Animator animator;
    private float oxygen;
    public int Oxygen
    {
        get { return Mathf.RoundToInt(oxygen); }
    }

    private EndlessUI ui;
    private EndlessOxygen mask;

    [SerializeField] private Sprite dead;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pop;
    [SerializeField] private AudioClip[] hurt;
    [SerializeField] private AudioClip die;
    [SerializeField] private float drainRate = 3f; // oxygen per second]
    private bool isDead = false;
    void Start()
    {
        endlessGameManager = FindObjectOfType<EndlessGameManager>();
        mask = FindObjectOfType<EndlessOxygen>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        ui = FindObjectOfType<EndlessUI>();
        animator = gameObject.GetComponent<Animator>();
        rb.position = new Vector2(0, -3);
        oxygen = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        // Continuous oxygen drain
        if (!endlessGameManager.PlayerWins && !ui.IsGameOver)
        {
            if (oxygen > 0)
            {
                oxygen -= drainRate * Time.deltaTime;
                if (oxygen < 0)
                {
                    oxygen = 0;
                }
            }
        }
        if (oxygen > 100)
        {
            oxygen = 100;
        }

        // Death from drowning - trigger death animation only once
        if (oxygen <= 0 && !isDead)
        {
            oxygen = 0f;
            isDead = true;
            animator.SetTrigger("Dead");
            audioSource.Stop();
            audioSource.PlayOneShot(die);
            GetComponent<SpriteRenderer>().sprite = dead;
        }

        if (ui.IsGameOver)
        {
            isDead = true;
        }
    

    }

    // Use FixedUpdate for physics-based movement
    void FixedUpdate()
    {
        float moveInput = 0f;
        if (!isDead && !endlessGameManager.PlayerWins) // Prevent movement if player has won
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                moveInput = -1f;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                moveInput = 1f;
            }
        }
    
        float moveSpeed = 20f; 
        //new Vector2(moveInput * moveSpeed, rb.velocity.y);
        rb.AddForce(new Vector2(moveInput * moveSpeed, rb.velocity.y));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead || endlessGameManager.PlayerWins || ui.IsGameOver)
        {
            return;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Don't process enemy hits if already dead
            //Debug.Log("Trigger");
            mask.CrackMask();
            mask.ShakeMask();
            EndlessEnemy enemy = other.GetComponent<EndlessEnemy>();
            if (oxygen - enemy.OxygenDepletion <= 0)
            {
                //Debug.Log("Dead");
                oxygen = 0f;
                isDead = true;
                animator.SetTrigger("Dead");
                audioSource.Stop();
                audioSource.PlayOneShot(die);
                GetComponent<SpriteRenderer>().sprite = dead;
            }
            else
            {
                endlessGameManager.TriggerSlowDown();
                oxygen -= enemy.OxygenDepletion;
                audioSource.PlayOneShot(hurt[Random.Range(0, 2)]);
                animator.SetTrigger("Hurt");
            }
            
        }
        if (other.gameObject.CompareTag("AirBubble"))
        {
            audioSource.PlayOneShot(pop);
            if (!ui.IsGameOver)
            {
                if (oxygen + 10 >= 100)
                {
                    oxygen = 100f;
                }
                else
                {
                    oxygen += 5f;
                }
            }
            Destroy(other.gameObject);
        }
    }

}
