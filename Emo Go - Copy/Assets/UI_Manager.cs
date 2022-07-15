using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    // UI Operations checking

    public GameObject Win_Check;

    // UI Screens

    public GameObject Lose_Screen;
    public GameObject Win_Screen;
    

    // UI Variables

    public TextMeshProUGUI Rescued_Emos_Counts;
    public float delay_time;
    private float rescuable_data;


    // Start is called before the first frame update
    void Start()
    {

        Win_Check.GetComponent<Win_Checker>().rescuable = Win_Check.GetComponent<Win_Checker>().rescued_max;
        rescuable_data = Win_Check.GetComponent<Win_Checker>().rescuable;
    }

    // Update is called once per frame
    void Update()
    {
        Rescued_Emos_Counts.SetText(Win_Check.GetComponent<Win_Checker>().rescued_num.ToString());

        if (Win_Check.GetComponent<Win_Checker>().rescued_num >= Win_Check.GetComponent<Win_Checker>().rescued_max)
        {
            Invoke("Win_Screen_", delay_time);
        }

        if (Win_Check.GetComponent<Win_Checker>().rescued_max < rescuable_data)
        {
            Invoke("Lose_Screen_", delay_time);
        }
    }

    void Win_Screen_()
    {
        Time.timeScale = 0.2f;
        Win_Screen.SetActive(true);
    }
    void Lose_Screen_()
    {
        Time.timeScale = 0.2f;
        Lose_Screen.SetActive(true);
    }

    public void Next_Level()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
