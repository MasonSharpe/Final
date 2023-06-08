using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Orb playerInfo;
    public bool slowActive = false;
    public Room currentRoom;
    public int enemiesKilledInRoom;
    public float dTime;

    public int level = 0;
    public int combo = 0;
    public float comboLeft;
    public float STime;
    public int SCombo;
    public int TimeRank;
    public int ComboRank;
    public int TotalRank;
    public bool inRoom = false;
    public Autoload autoload;
    public List<GameObject> completedRooms = new List<GameObject>();

    private void Awake()
    {
        autoload = Autoload.instance;
        autoload.gameManager = this;
    }

    void Start()
    {
        
        autoload.Respawn();
    }


    // Update is called once per frame
    void Update()
    {
        TimeRank = autoload.levelTime == 0 ? 0 : 5 - (autoload.levelTime <= STime ? 0 : Mathf.Clamp((int)Mathf.Round((autoload.levelTime - STime) / (STime / 4f)), 0, 5));
        ComboRank = SCombo == 0 ? 5 : autoload.highestCombo == 0 ? 0 : autoload.highestCombo >= SCombo ? 5 : (int)Mathf.Round(autoload.highestCombo / (SCombo / 4f));
        TotalRank = SCombo == 0 ? ComboRank : (int)Mathf.Floor((TimeRank + ComboRank) / 2);
    }

    public void IncreaseCombo()
    {
        combo++;
        if (combo > autoload.highestCombo)
        {
            autoload.highestCombo = combo;
        }
        comboLeft = 8;
    }
}
