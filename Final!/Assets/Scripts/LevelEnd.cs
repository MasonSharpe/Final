using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    GameManager gameManager;
    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
       Vector2 dist = (gameManager.player.transform.position - transform.position);
        if (dist.magnitude < 10)
        {
            gameManager.playerInfo.rb.velocity += -dist;
        }
    }
}
