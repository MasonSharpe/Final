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

    void Start()
    {
        levelSelectPanel.SetActive(false);
        autoload = Autoload.instance;
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
        SceneManager.LoadScene("Level" + index);
    }

    public void closeLevelSelect()
    {
        levelSelectPanel.SetActive(false);
    }

    public void resetTimes()
    {
        autoload.resetTimes();
    }
}
