using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;
    [HideInInspector]public int currentLevel;
    [HideInInspector] public int menuFocus;

    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        menuFocus = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
