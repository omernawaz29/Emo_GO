using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManagerScript : MonoBehaviour
{
    // UI Screens

    [SerializeField] private GameObject endScreen;

    // UI Variables

    [SerializeField] private TextMeshProUGUI rescuedEmoCountText;
    [SerializeField] private TextMeshProUGUI starCountText;
    [SerializeField] private TextMeshProUGUI endButtonText;
    [SerializeField] private TextMeshProUGUI endScreenTitle;
    [SerializeField] private GameObject[] stars;


    private float _totalEmos = 0;
    private float _rescuedEmos = 0;
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
        endButtonText.text = "Next";
        endScreenTitle.text = "You Won!";
        endScreen.SetActive(true);
        SetEndStars();

    }
    void Lose()
    {
        Time.timeScale = 0.2f;
        endButtonText.text = "Fired Up!";
        endScreenTitle.text = "You Almost Won!";
        endScreen.SetActive(true);
        SetEndStars();

    }

    private void SetEndStars()
    {
        float endPercentage = (_rescuedEmos * 100) / _totalEmos;

        if (endPercentage >= 33)
            stars[0].SetActive(true);
        if (endPercentage >= 66)
            stars[1].SetActive(true);
        if (endPercentage == 100)
            stars[2].SetActive(true);
    }

    public void ClickEndButton()
    {
        Time.timeScale = 1;
        if(endButtonText.text == "Next")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void SetTotalEmos(int count)
    {
        _totalEmos = count; 
    }


    public void SetRescuedEmoCount(int count)
    {
        rescuedEmoCountText.text = count.ToString();
        _rescuedEmos = count;
    }
}
