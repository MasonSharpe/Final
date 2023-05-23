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
    public float movementSpeed = 5;
    public GameObject bulletPrefab;
    float stunDuration = 0;
    Rigidbody2D rb;
    public string type = "Stationary";
    Vector2 movementDirection = Vector2.right;
    public bool[] followAxis = { true, false };
    public float pointTimer = 0;
    public float pointTime = -3;
    Vector2 realVelocity;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GetComponentInParent<GameManager>();
        if (type.Contains("Bird"))
        {
            rb.gravityScale = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        shootDelay -= gameManager.dTime;
        stunDuration -= gameManager.dTime;
        pointTimer -= gameManager.dTime;
        if (shootDelay < 0 && stunDuration < 0)
        {
            shootDelay = 3;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, gameManager.gameObject.transform);
            bullet.GetComponent<Rigidbody2D>().velocity = (gameManager.player.transform.position - transform.position).normalized * bulletSpeed;
            Destroy(bullet, 10);
        }
        realVelocity = rb.velocity;
        Vector2 previousVelocity = realVelocity;
        if (stunDuration < 0)
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
            stunDuration = gameManager.playerInfo.rb.velocity.magnitude < 4 ? 0 : gameManager.playerInfo.rb.velocity.magnitude / 4;
        }
        else
        {
            health -= gameManager.playerInfo.rb.velocity.magnitude;
            StartCoroutine(Camera.main.gameObject.GetComponent<CameraShake>().Shake(0.2f, 0.2f));
            gameManager.playerInfo.remainingPierce--;
            if (health < 0)
            {
                gameManager.combo++;
                if (gameManager.combo > gameManager.highestCombo)
                {
                    gameManager.highestCombo = gameManager.combo;
                }
                gameManager.comboLeft = 8;
                gameManager.enemiesKilledInRoom++;
                gameManager.currentRoom.trySpawnWave();
                Destroy(gameObject);
            }
        }
    }
}
