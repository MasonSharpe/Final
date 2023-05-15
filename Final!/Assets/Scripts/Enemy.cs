using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameManager gameManager;
    float health = 50;
    float shootDelay = 3;
    public float bulletSpeed = 1.1f;
    public GameObject bulletPrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shootDelay -= Time.deltaTime;
        if (shootDelay < 0)
        {
            shootDelay = 3;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = (gameManager.player.transform.position - transform.position).normalized * bulletSpeed;
            Destroy(bullet, 2);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            playerCollision();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            playerCollision();
        }
    }

    void playerCollision()
    {
        health -= gameManager.playerInfo.rb.velocity.magnitude;
        gameManager.playerInfo.remainingPierce--;
        if (health < 0)
        {
            Destroy(gameObject);
        }
    }
}
