using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessAirBubble : MonoBehaviour
{
    private EndlessGameManager endlessGameManager;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        endlessGameManager = FindObjectOfType<EndlessGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        float downSpeed = endlessGameManager.DownSpeed;
        Vector2 pos = rb.position;
        pos.y -= downSpeed * Time.fixedDeltaTime * SpeedScaler.VerticalScale;
        rb.MovePosition(pos);
    }
}
