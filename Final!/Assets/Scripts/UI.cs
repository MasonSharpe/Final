using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject panel;
    public Slider health;
    public Slider essence;
    public Slider comboLeft;
    public Toggle stable;
    public TextMeshProUGUI comboText;
    public GameObject rankPanel;
    public TextMeshProUGUI time;
    public TextMeshProUGUI combo;
    public TextMeshProUGUI rank;
    Vector3 startingPanelPos;
    GameManager gameManager;
    string[] rankLetters = { "F", "D", "C", "B", "A", "S"};
    int prevCombo;
    void Start()
    {
        startingPanelPos = panel.transform.position;
        gameManager = GetComponentInParent<GameManager>();
        prevCombo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        health.value = gameManager.playerInfo.health;
        essence.value = gameManager.playerInfo.essence;
        comboLeft.value = gameManager.comboLeft;
        comboText.text = gameManager.combo.ToString();
        time.text = "Time: " + rankLetters[gameManager.TimeRank] + " (" + Mathf.Round(gameManager.autoload.levelTime).ToString() + ")";
        combo.text = "Combo: " + rankLetters[gameManager.ComboRank] + " (" + gameManager.autoload.highestCombo.ToString() + ")";
        rank.text = "Total: " + rankLetters[gameManager.TotalRank];
        stable.isOn = gameManager.playerInfo.pierceTimer <= 1;
        Vector3 pos = Input.mousePosition;
        pos.z = -Camera.main.transform.position.z;
        pos -= new Vector3(Screen.width / 2, Screen.height / 2, 0);
        panel.transform.position = startingPanelPos + (pos / 60);
        stable.gameObject.SetActive(!gameManager.playerInfo.isConnected);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            rankPanel.SetActive(!rankPanel.activeSelf);
        }
        if (prevCombo != gameManager.combo)
        {
            comboText.GetComponentInChildren<Animation>().Play();
            prevCombo = gameManager.combo;
        }
    }
}
