using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;
    [HideInInspector] public int currentLevel = 1;
    [HideInInspector] public int menuFocus;

    void Start()
    {
        menuFocus = 0;
        currentLevel = 1;
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
