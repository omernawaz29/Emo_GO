using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePreloader : MonoBehaviour
{
    CanvasGroup fade;
    float loadTime;
    float minLoadTime = 2.0f;


    private void Start()
    {
        fade = FindObjectOfType<CanvasGroup>();
        fade.alpha = 1.0f;


        //We can do all kinds of loading at this point

        if (Time.time < minLoadTime)
            loadTime = minLoadTime;
        else
            loadTime = Time.time;
    }

    private void Update()
    {
        if(Time.time< minLoadTime)
        {
            fade.alpha = 1 - Time.time;
        }

        if(Time.time >=minLoadTime && loadTime != 0)
        {
            fade.alpha = Time.time - minLoadTime;
            if(fade.alpha >= 1)
            {
                SceneManager.LoadScene(SaveManager.instance.state.levelsCompleted + 2);
            }
        }
    }

}
