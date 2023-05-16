using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameManager gameManager;
    Rigidbody2D rb;
    Vector2 startingVelocity;
    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        startingVelocity = rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = startingVelocity * (gameManager.slowActive ? 0.1f : 1);
        if (((Vector2)Camera.main.transform.position - (Vector2)transform.position).magnitude > 10)
        {
            Destroy(gameObject);
        }
    }
}
