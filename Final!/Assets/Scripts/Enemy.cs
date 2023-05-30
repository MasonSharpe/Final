using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    GameManager gameManager;
    float health = 50;
    float shootDelay = 3;
    public float bulletSpeed = 1.1f;
    public float bulletFireRate = 3;
    public float movementSpeed = 5;
    public GameObject bulletPrefab;
    float stunDuration = 0;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    public string type = "Stationary";
    public Vector2 movementDirection = Vector2.right;
    public bool[] followAxis = { true, false };
    public float pointTimer = 0;
    public float pointTime = -3;
    Vector2 realVelocity;
    Color ogColor;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        gameManager = GetComponentInParent<GameManager>();
        movementDirection *= -1;
        ogColor = sprite.color;
        pointTimer = 0;
        bulletFireRate = 3;
        if (type.Contains("Bird"))
        {
            rb.gravityScale = 0;
        }
        if (type.Contains("Point"))
        {
            movementSpeed += 2;
            bulletSpeed += 2;
            bulletFireRate -= 0.5f;
        }
        if (type.Contains("Stationary"))
        {
            bulletSpeed += 4;
            bulletFireRate -= 1;
        }
        if (type.Contains("Elite"))
        {
            movementSpeed += 3;
        }
        health = 0;
        playerCollision();
    }

    // Update is called once per frame
    void Update()
    {
        if (stunDuration <= 0 || sprite.color == Color.white)
        {
            shootDelay -= gameManager.dTime;
        }
        stunDuration = Mathf.Clamp(stunDuration - gameManager.dTime, 0, 3);
        pointTimer -= gameManager.dTime;
        if (shootDelay < 0.3f && shootDelay > 0)
        {
            sprite.color = Color.white;
        }
        else
        {
            sprite.color = ogColor;
        }
        if (shootDelay < 0)
        {
            shootDelay = bulletFireRate;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, gameManager.gameObject.transform);
            bullet.GetComponent<Rigidbody2D>().velocity = (gameManager.player.transform.position - transform.position).normalized * bulletSpeed;
            if (type.Contains("Elite"))
            {
                bullet.GetComponent<Bullet>().homing = true;
            }
                Destroy(bullet, 10);
        }
        realVelocity = rb.velocity;
        Vector2 previousVelocity = realVelocity;
        if (stunDuration <= 0)
        {
            if (type.Contains("Point"))
            {
                realVelocity = movementDirection * movementSpeed;
                if (pointTimer < pointTime)
                {
                    movementDirection *= -1;
                    pointTimer = 0;
                }
                if (!type.Contains("Bird"))
                {
                    realVelocity = new Vector2(realVelocity.x, previousVelocity.y);
                }
            }
            else if (type.Contains("Follow"))
            {
                realVelocity = new Vector2(followAxis[0] ? (gameManager.player.transform.position - transform.position).normalized.x * movementSpeed : realVelocity.x, followAxis[1] ? (gameManager.player.transform.position - transform.position).normalized.y * movementSpeed : realVelocity.y);
            }
        }
        rb.velocity = realVelocity * (gameManager.slowActive ? 0.1f : 1);
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
        if (gameManager.playerInfo.isConnected)
        {
            stunDuration += gameManager.playerInfo.rb.velocity.magnitude < 4 ? 0 : gameManager.playerInfo.rb.velocity.magnitude / 4;
        }
        else
        {
            health -= gameManager.playerInfo.rb.velocity.magnitude;
            if (gameManager.playerInfo.remainingPierce > 0)
            {
                StartCoroutine(Camera.main.gameObject.GetComponent<CameraShake>().Shake(0.2f, 0.2f));
            }
            gameManager.playerInfo.remainingPierce--;
            if (health < 0)
            {
                gameManager.IncreaseCombo();
                gameManager.enemiesKilledInRoom++;
                gameManager.currentRoom.trySpawnWave();
                Destroy(gameObject);
            }
        }
    }
}
