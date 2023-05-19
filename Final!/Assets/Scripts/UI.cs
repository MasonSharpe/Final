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
    public TextMeshProUGUI comboText;
    public GameManager gameManager;
    Vector3 startingPanelPos;
    void Start()
    {
        startingPanelPos = panel.transform.position;
        gameManager = GetComponentInParent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        health.value = gameManager.playerInfo.health;
        essence.value = gameManager.playerInfo.essence;
        comboLeft.value = gameManager.comboLeft;
        comboText.text = "Combo: " + gameManager.combo;
        Vector3 pos = Input.mousePosition;
        pos.z = -Camera.main.transform.position.z;
        pos -= new Vector3(Screen.width / 2, Screen.height / 2, 0);
        panel.transform.position = startingPanelPos + (pos / 60);
    }
}
