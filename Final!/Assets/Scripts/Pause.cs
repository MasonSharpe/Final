using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    GameManager gameManager;
    public GameObject panel;
    public AudioClip select;
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

    public void Restart()
    {
        gameManager.autoload.currentCheckpoint = Vector3.zero;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void selectSound()
    {
        gameManager.autoload.sfx.PlayOneShot(select, 0.7f);
    }
}
