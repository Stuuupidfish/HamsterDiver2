using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int oxygenDepletion;
    public int OxygenDepletion
    {
        get {return oxygenDepletion;}
    }
    public string enemyType;
    [SerializeField] private float speed; 
    private GameManager gameManager;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool moveRight = false;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sound;

    private UI ui;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UI>();
        gameManager = FindObjectOfType<GameManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (rb.position.x < 0)
        {
            moveRight = true;
        }
        else
        {
            moveRight = false;
        }
        // Flip sprite if moving right (coming from left)
        spriteRenderer.flipX = moveRight;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.position.y < -6)
        {
            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        float moveInput = moveRight ? 1f : -1f;
        float downSpeed = gameManager.DownSpeed;
        float moveSpeed = speed*(downSpeed/0.1f); //left/right move speed is proportional to downwards move speed, so enemies don't move too fast when the background moves faster/slower
        Vector2 pos = rb.position;
        pos.x += moveInput * moveSpeed * Time.fixedDeltaTime;
        pos.y -= downSpeed;
        if (!ui.IsPaused)
        {
            rb.MovePosition(pos);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(sound);
        }
    }
    
}
