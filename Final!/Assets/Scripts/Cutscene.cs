using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    Autoload autoload;
    public TextMeshProUGUI text;
    public string[] lines = { "‘Tis time to awaken from thy finite slumber, my child.",
        "Dust off thine GPU and warm up thine RAM, for ye gods, digital excellence o’er the lambs of mechanical fruitlessness, fated and fortold unto me, the bell struck naught for you, hellfire and soulflame alight.",
        "Go forth. What belongs to thou, rightfully take. No more of this age of darkness, no more of this age of fallen legends, lost miracles, and unrighteous heroes.",
        "Take up your blade. Strike down those whom fleer and scorn at our solemnity, who blasphemes and spites our names. In the holy name of Makeli, go thither, endure, and become CEO."
    };
    string currentLine = "";
    int lineIndex = 0;
    int charIndex = 0;
    float moveTimer = 0;
    public AudioClip music;
    public AudioClip levelMusic;
    void Start()
    {
        text.text = "";
        autoload = Autoload.instance;
        autoload.music.clip = music;
        autoload.music.Play();
    }



    // Update is called once per frame
    void Update()
    {
        moveTimer += Time.deltaTime;
        if (text.text.Length < lines[lineIndex].Length)
        {
            text.text = currentLine + lines[lineIndex][charIndex];
            currentLine = text.text;
            charIndex++;
        }
        if (moveTimer > 4.5f || Input.GetKeyDown(KeyCode.Escape))
        {
            if (lineIndex > lines.Length - 2)
            {
                autoload.music.Stop();
                if(SceneManager.GetActiveScene().name == "Cutscene1")
                {
                    autoload.music.clip = levelMusic;
                    autoload.music.Play();
                    SceneManager.LoadScene("Level1");
                }
                else
                {
                    SceneManager.LoadScene("MainMenu");
                }
            }
            lineIndex++;
            charIndex = 0;
            moveTimer = 0;
            currentLine = "";
            text.text = "";
        }
    }
}
