using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    GameManager gameManager;
    float shrinkTimer = 0;
    public UI ui;

    
    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        shrinkTimer -= gameManager.dTime / 3;
        if (shrinkTimer < 1 && shrinkTimer > 0.1f)
        {
            float mult = (shrinkTimer) * 0.438f;
            gameManager.player.transform.localScale = new Vector3(mult, mult, 1);
            int preRank = gameManager.autoload.levelRanks[gameManager.level - 1];
            if (shrinkTimer < 0.2f)
            {
                if (gameManager.TotalRank > preRank)
                {
                    gameManager.autoload.levelRanks[gameManager.level - 1] = gameManager.TotalRank;
                    gameManager.autoload.saveData.isDeleted = false;
                }
                gameManager.level++;
                gameManager.autoload.resetVariables();
                ui.ShowPanel();
                Time.timeScale = 0;
                shrinkTimer = 0;
            }
        }

    }
    private void LateUpdate()
    {
        Vector2 dist = (gameManager.player.transform.position - transform.position);
        if (dist.magnitude < 10)
        {
            gameManager.playerInfo.rb.velocity += -dist / 2;

        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shrinkTimer < 0 && !gameManager.playerInfo.isConnected)
        {
            shrinkTimer = 1;
            gameManager.playerInfo.hasControl = false;
        }
    }
}
