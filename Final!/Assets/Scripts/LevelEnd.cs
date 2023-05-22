using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    GameManager gameManager;
    float shrinkTimer = 0;
    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        shrinkTimer -= Time.deltaTime / 3;
        if (shrinkTimer < 1 && shrinkTimer > 0.1f)
        {
            float mult = (shrinkTimer) * 0.438f;
            gameManager.player.transform.localScale = new Vector3(mult, mult, 1);
            if (shrinkTimer < 0.2f)
            {
                gameManager.level++;
                SceneManager.LoadScene("Level" + gameManager.level);
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
        if (shrinkTimer < 0)
        {
            shrinkTimer = 1;
        }
    }
}
