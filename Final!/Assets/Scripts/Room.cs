using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public GameObject[] barriersToDestroy;
    public GameObject[] barriersToSpawn;
    public int[] enemiesPerWave;
    public GameObject[] enemies;
    GameManager gameManager;
    float enemySpawnTimer = 0;
    int enemyIndex = 0;
    int enemiesLeftToSpawn = 0;
    int wave = 0;
    

    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
        enemiesLeftToSpawn = enemiesPerWave[0];
        gameManager.enemiesKilledInRoom = 0;
    }

    // Update is called once per frame
    void Update()
    {
        enemySpawnTimer -= Time.deltaTime;
        if (enemySpawnTimer < 1 && enemySpawnTimer > 0 && enemiesLeftToSpawn > 0)
        {
            enemies[enemyIndex].SetActive(true);
            enemiesLeftToSpawn--;
            enemyIndex += 1;
            enemySpawnTimer = 1.4f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameManager.currentRoom = this;
        enemySpawnTimer = 1.4f;
        gameManager.inRoom = true;
        for (int i = 0; i < barriersToSpawn.Length; i++)
        {
            barriersToSpawn[i].SetActive(true);
        }
        
    }

    public void trySpawnWave()
    {
        if (gameManager.enemiesKilledInRoom == enemiesPerWave[wave])
        {
            if (wave == enemiesPerWave.Length - 1)
            {
                for (int i = 0; i < barriersToDestroy.Length; i++)
                {
                    barriersToDestroy[i].SetActive(false);
                    gameManager.inRoom = false;
                    gameManager.enemiesKilledInRoom = 0;
                    //gameManager.completedRooms.Add(gameObject);
                }
            }
            else
            {
                wave++;
                gameManager.enemiesKilledInRoom = 0;
                enemiesLeftToSpawn = enemiesPerWave[wave];
                enemySpawnTimer = 1.4f;
            }
        }
    }
}
