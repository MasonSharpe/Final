using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject panel;
    public Slider health;
    public GameManager manager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        health.value = manager.playerInfo.health;
    }
}
