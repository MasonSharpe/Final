using System.Collections;
using System.Collections.Generic;
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
        shootDelay -= Time.deltaTime;
        stunDuration -= Time.deltaTime;
        pointTimer -= Time.deltaTime;
        if (shootDelay < 0 && stunDuration < 0)
        {
            shootDelay = 3;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, gameManager.gameObject.transform);
            bullet.GetComponent<Rigidbody2D>().velocity = (gameManager.player.transform.position - transform.position).normalized * bulletSpeed;
            Destroy(bullet, 10);
        }
        Vector2 previousVelocity = rb.velocity;
        if (stunDuration < 0)
        {
            if (type.Contains("Point"))
            {
                rb.velocity = movementDirection * movementSpeed;
                if (pointTimer < pointTime)
                {
                    movementDirection *= -1;
                    pointTimer = 0;
                }
                if (!type.Contains("Bird"))
                {
                    rb.velocity = new Vector2(rb.velocity.x, previousVelocity.y);
                }
            }
            else if (type.Contains("Follow"))
            {
                rb.velocity = new Vector2(followAxis[0] ? (gameManager.player.transform.position - transform.position).normalized.x * movementSpeed : rb.velocity.x, followAxis[1] ? (gameManager.player.transform.position - transform.position).normalized.y * movementSpeed : rb.velocity.y);
            }
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
                gameManager.comboLeft = 8;
                gameManager.enemiesKilledInRoom++;
                gameManager.currentRoom.trySpawnWave();
                Destroy(gameObject);
            }
        }
    }
}
