using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject levelSelectPanel;
    Autoload autoload;
    string[] rankLetters = { "F", "D", "C", "B", "A", "S" };
    public AudioClip levelMusic;
    public AudioClip mainMusic;

    void Start()
    {
        levelSelectPanel.SetActive(false);
        autoload = Autoload.instance;
        autoload.music.clip = mainMusic;
        autoload.music.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void openLevelSelect()
    {
        TextMeshProUGUI[] buttons = levelSelectPanel.GetComponentsInChildren<TextMeshProUGUI>();
        int rankIndex = 0;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i % 2 != 0)
            {
                int rank = autoload.levelRanks[rankIndex];
                buttons[i].text = "Rank: " + (rank == -1 ? "NA" : rankLetters[rank]);
                rankIndex++;
            }
        }
        levelSelectPanel.SetActive(true);
    }

    public void Close()
    {
        Application.Quit();
    }

    public void selectLevel(int index)
    {
        autoload.music.Stop();
        if (!(index == 1 && autoload.levelRanks[0] == -1))
        {
            autoload.music.clip = levelMusic;
            autoload.music.Play();
        }
        SceneManager.LoadScene(index == 1 && autoload.levelRanks[0] == -1 ? "Cutscene1" : "Level" + index);
    }

    public void closeLevelSelect()
    {
        levelSelectPanel.SetActive(false);
    }

    public void resetTimes()
    {
        autoload.saveData.isDeleted = true;
        autoload.resetTimes();
        autoload.WriteFile();
    }

    public void loadSave()
    {
        autoload.ReadFile();
        if (!autoload.saveData.isDeleted)
        {
            autoload.levelRanks[0] = autoload.saveData.level1;
            autoload.levelRanks[1] = autoload.saveData.level2;
            autoload.levelRanks[2] = autoload.saveData.level3;
            autoload.levelRanks[3] = autoload.saveData.level4;
            autoload.levelRanks[4] = autoload.saveData.level5;
        }
    }

    public void saveGame()
    {
        autoload.WriteFile();
    }
}
