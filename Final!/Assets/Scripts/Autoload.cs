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



    void Start()
    {
        DontDestroyOnLoad(gameObject);
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
        if (currentCheckpoint != null)
        {
            gameManager.playerInfo.tr.enabled = false;
            gameManager.player.transform.position = currentCheckpoint;
            gameManager.playerInfo.cameraMover.transform.position = currentCheckpoint;
            gameManager.playerInfo.tr.enabled = true;
            for (int i = 0; i < completedRooms.Count; i++)
            {
                completedRooms[i].SetActive(false);
            }
        }
    }
}
