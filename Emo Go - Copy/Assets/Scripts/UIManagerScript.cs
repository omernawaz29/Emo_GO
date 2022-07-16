using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManagerScript : MonoBehaviour
{
    // UI Screens

    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject winScreen;
    

    // UI Variables

    [SerializeField] private TextMeshProUGUI rescuedEmoCountText;
    [SerializeField] private TextMeshProUGUI starCountText;


    // Start is called before the first frame update
    void Start()
    {
        rescuedEmoCountText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
    }

    
    void Win()
    {
        Time.timeScale = 0.2f;
        winScreen.SetActive(true);
    }
    void Lose()
    {
        Time.timeScale = 0.2f;
        loseScreen.SetActive(true);
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

    public void SetRescuedEmoCount(int count)
    {
        rescuedEmoCountText.text = count.ToString();
    }
}
