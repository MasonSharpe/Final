using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Autoload : MonoBehaviour
{
    public GameManager gameManager;
    public static Autoload instance;

    public Vector3 currentCheckpoint = Vector3.zero;
    public List<GameObject> completedRooms = new List<GameObject>();
    public float levelTime = 0;
    public int highestCombo = 0;
    public bool ranksVisible = false;
    public int[] levelRanks = {-1, -1, -1, -1, -1};
    public AudioSource sfx;
    public AudioSource music;
    public AudioHighPassFilter filter;

    FileStream dataStream;
    BinaryFormatter converter = new BinaryFormatter();
    string saveFile;
    public SaveData saveData;

    private void Awake()
    {
        saveFile = Application.persistentDataPath + "/gamedata.data";
        saveData = new SaveData();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        resetVariables();
        instance = this;
        sfx.volume = 0.7f;
        music.volume = 0.7f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager != null)
        {
            if (gameManager.slowActive)
            {
                filter.cutoffFrequency = 1000;
            }
            else
            {
                filter.cutoffFrequency = 10;
            }
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

    public void WriteFile()
    {
        saveData.level1 = levelRanks[0];
        saveData.level2 = levelRanks[1];
        saveData.level3 = levelRanks[2];
        saveData.level4 = levelRanks[3];
        saveData.level5 = levelRanks[4];
        dataStream = new FileStream(saveFile, FileMode.Create);
        converter.Serialize(dataStream, saveData);
        dataStream.Close();
    }

    public void ReadFile()
    {
        if (File.Exists(saveFile))
        {
            dataStream = new FileStream(saveFile, FileMode.Open);
            saveData = (SaveData)converter.Deserialize(dataStream);
            dataStream.Close();
        }
    }

    [System.Serializable]

    public class SaveData
    {
        public int level1 = -1;
        public int level2 = -1;
        public int level3 = -1;
        public int level4 = -1;
        public int level5 = -1;
        public bool isDeleted = false;
    }
}
