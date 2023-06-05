using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    GameManager gameManager;
    float health = 50;
    float maxHealth = 50;
    float shootDelay = 1;
    public float bulletSpeed = 1.1f;
    public float bulletFireRate = 3;
    public float movementSpeed = 5;
    public GameObject bulletPrefab;
    public GameObject stun;
    float stunDuration = 0;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    public string type = "Stationary";
    public Vector2 movementDirection = Vector2.right;
    public bool[] followAxis = { true, false };
    public float pointTimer = 0;
    public float pointTime = -3;
    int bossBursts = 3;
    float bossFirerate;
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
            health += 25;
            ogColor.b = 0;
        }
        if (type.Contains("Boss"))
        {
            bulletSpeed += 8;
            bulletFireRate += 0.5f;
            bossFirerate = bulletFireRate;
            health += 150;
            ogColor.r = 0;
        }
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (stunDuration <= 0 || sprite.color == Color.red)
        {
            shootDelay -= gameManager.dTime;
        }
        stunDuration = Mathf.Clamp(stunDuration - gameManager.dTime, 0, 3);
        pointTimer -= gameManager.dTime;
        if (shootDelay < 0.3f && shootDelay > 0)
        {
            sprite.color = Color.red;
        }
        else
        {
            sprite.color = ogColor;
        }
        if (shootDelay < 0)
        {
            if (type.Contains("Boss"))
            {
                if (bossBursts < 2)
                {
                    bossBursts = 3;
                    shootDelay = bulletFireRate;
                }
                else
                {
                    bossBursts--;
                    shootDelay = 0.1f;
                }
            }
            else
            {
                shootDelay = bulletFireRate;
            }
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
            stun.SetActive(false);
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
            else if (type.Contains("Boss"))
            {
                realVelocity = new Vector2((gameManager.player.transform.position - transform.position).normalized.x * movementSpeed, (gameManager.player.transform.position - transform.position).normalized.y * movementSpeed);
                if (shootDelay <= 0.4f)
                {
                    realVelocity *= 0.5f;
                }
                bulletFireRate = Mathf.Clamp(bossFirerate * (health / maxHealth), 0.5f, 100);
            }
        }
        else
        {
            stun.SetActive(true);
        }
        rb.velocity = realVelocity * (gameManager.slowActive ? 0.1f : 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            playerCollision();
        }
        if (collision.gameObject.tag == "Kill Barrier")
        {
            takeDamage(999);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            playerCollision();
        }
        if (collision.gameObject.tag == "Kill Barrier")
        {
            takeDamage(999);
        }
    }

    void playerCollision()
    {
        if (gameManager.playerInfo.isConnected)
        {
            if (!type.Contains("Boss"))
            {
                stunDuration += gameManager.playerInfo.rb.velocity.magnitude < 3 ? 0 : gameManager.playerInfo.rb.velocity.magnitude / 3;
            }
        }
        else
        {
            takeDamage(gameManager.playerInfo.rb.velocity.magnitude);
        }
    }

    void takeDamage(float amount)
    {
        health -= amount;
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
