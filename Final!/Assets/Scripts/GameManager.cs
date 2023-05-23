using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Orb playerInfo;
    public bool slowActive = false;
    public Room currentRoom;
    public int enemiesKilledInRoom;
    public float dTime;
    public float levelTime;
    public int level = 0;
    public int combo = 0;
    public int highestCombo = 0;
    public float comboLeft;
    public float STime;
    public int SCombo;
    public int TimeRank;
    public int ComboRank;
    public int TotalRank;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TimeRank = levelTime == 0 ? 0 : 5 - (levelTime <= STime ? 0 : Mathf.Clamp((int)Mathf.Round((levelTime - STime) / (STime / 4f)), 0, 5));
        ComboRank = highestCombo == 0 ? 0 : highestCombo >= SCombo ? 5 : (int)Mathf.Round(highestCombo / (SCombo / 4f));
        TotalRank = (int)Mathf.Floor((TimeRank + ComboRank) / 2);
    }
}
