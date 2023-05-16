using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject barrierToDestroy;
    public GameObject barrierToSpawn;
    GameObject[][] enemies;
    public GameObject[] rawEnemies;
    GameManager gameManager;
    float enemySpawnTimer = 0;
    int enemyIndex = 0;
    int wave = 0;

    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
        for (int i = 0; i < enemies.Length; i++)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        enemySpawnTimer -= Time.deltaTime;
        if (enemySpawnTimer < 1 && enemySpawnTimer > 0 && enemyIndex < enemies.Length)
        {
            enemies[enemyIndex][wave].SetActive(true);
            enemyIndex += 1;
            enemySpawnTimer = 1.4f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameManager.currentRoom = this;
        enemySpawnTimer = 1.4f;
        barrierToSpawn.SetActive(true);
    }
}
