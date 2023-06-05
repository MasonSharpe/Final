using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autoload : MonoBehaviour
{
    public GameManager gameManager;
    public static Autoload instance;

    public Vector3 currentCheckpoint = Vector3.zero;
    public List<GameObject> completedRooms = new List<GameObject>();
    public float levelTime = 0;
    public int highestCombo = 0;
    public int[] levelRanks = {-1, -1, -1, -1, -1};



    void Start()
    {
        DontDestroyOnLoad(gameObject);
        resetVariables();
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager != null)
        {
            
        }
    }

    public void Respawn()
    {
        if (currentCheckpoint != null && currentCheckpoint != Vector3.zero)
        {
            gameManager.playerInfo.tr.enabled = false;
            gameManager.player.transform.position = currentCheckpoint;
            gameManager.playerInfo.cameraMover.transform.position = currentCheckpoint;
            gameManager.playerInfo.tr.enabled = true;
        }
        else
        {
            resetVariables();
        }
    }


    public void resetVariables()
    {
        currentCheckpoint = Vector3.zero;
        completedRooms = new List<GameObject>();
        levelTime = 0;
        highestCombo = 0;
    }

    public void resetTimes()
    {
        levelRanks = new int[] { -1, -1, -1, -1, -1 };
    }
}
