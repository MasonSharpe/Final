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
    public GameObject bulletPrefab;
    float stunDuration = 0;
    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        shootDelay -= Time.deltaTime;
        stunDuration -= Time.deltaTime;
        if (shootDelay < 0 && stunDuration < 0)
        {
            shootDelay = 3;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, gameManager.gameObject.transform);
            bullet.GetComponent<Rigidbody2D>().velocity = (gameManager.player.transform.position - transform.position).normalized * bulletSpeed;
            Destroy(bullet, 10);
            
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
