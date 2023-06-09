using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameManager gameManager;
    Rigidbody2D rb;
    Vector2 startingVelocity;
    public bool homing;
    float homingPeriod = 0.4f;
    float startingMag = 0;
    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        startingVelocity = rb.velocity;
        startingMag = startingVelocity.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        homingPeriod -= Time.deltaTime;
        rb.velocity = startingVelocity;
        if (homing)
        {
            rb.velocity = (Vector2)(gameManager.player.transform.position - transform.position).normalized * (startingMag);
        }
        if (homingPeriod < 0 && homing)
        {
            homing = false;
            
        }
        startingVelocity = rb.velocity.normalized * startingVelocity.magnitude;
        rb.velocity *= (gameManager.slowActive ? 0.1f : 1);
        if (((Vector2)Camera.main.transform.position - (Vector2)transform.position).magnitude > 20)
        {
            Destroy(gameObject);
        }
    }
}
