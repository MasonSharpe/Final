using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autoload : MonoBehaviour
{
    public GameManager gameManager;
    public static Autoload instance;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager != null)
        {
            
        }
    }
}
