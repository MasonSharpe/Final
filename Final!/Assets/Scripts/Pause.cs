using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    GameManager gameManager;
    public GameObject panel;
    void Start()
    {
        panel.SetActive(false);
        gameManager = GetComponentInParent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                panel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void Resume()
    {
        panel.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
