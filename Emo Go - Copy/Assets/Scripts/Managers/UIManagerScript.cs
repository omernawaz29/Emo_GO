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
    [SerializeField] private TextMeshProUGUI currentLevelText;
    //[SerializeField] private TextMeshProUGUI endButtonText;
    //[SerializeField] private TextMeshProUGUI endScreenTitle;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameObject[] stars;

    // Tutorial Text
    [SerializeField] private GameObject tutorialHolder;

    //Win Screen Objects
    [SerializeField] private GameObject NextLevelButton;
    [SerializeField] private GameObject GlassesEmo;


    //Lose Screen Objects
    [SerializeField] private GameObject RestartLevelButton;
    [SerializeField] private GameObject SadEmo;


    private float _totalEmos = 0;
    private float _rescuedEmos = 0;
    // Start is called before the first frame update
    void Start()
    {
        rescuedEmoCountText.text = "0";
        currentLevelText.text = (SceneManager.GetActiveScene().buildIndex - 1).ToString();

        tutorialHolder.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
    }

    
    void Win()
    {
        Time.timeScale = 0.5f;
        endScreen.SetActive(true);
        NextLevelButton.SetActive(true);
        GlassesEmo.SetActive(true);
        SetEndStars();

    }
    void Lose()
    {
        Time.timeScale = 0.5f;
        endScreen.SetActive(true);
        RestartLevelButton.SetActive(true);
        SadEmo.SetActive(true);
        SetEndStars();

    }

    private void SetEndStars()
    {
        float endPercentage = (_rescuedEmos * 100) / _totalEmos;

        if(endPercentage < 66 && endPercentage > 0)
            endPercentage += 20f;

        int coins = Mathf.CeilToInt(endPercentage * 10);

        coinText.text = "$" + coins.ToString();
        SaveManager.instance.state.coins += coins;

        if (endPercentage >= 33)
            stars[0].SetActive(true);
        if (endPercentage >= 66)
            stars[1].SetActive(true);
        if (endPercentage == 100)
            stars[2].SetActive(true);
    }

    

    public void ClickNextButton()
    {
        Time.timeScale = 1;

        var newSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (newSceneIndex > SceneManager.sceneCountInBuildSettings)
        {
            GameManagerScript.instance.menuFocus = 2;
            SceneManager.LoadScene("MainMenu");
        }
        else
            SceneManager.LoadScene(newSceneIndex);
    }

    public void ClickRedoButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ClickHomeButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
        GameManagerScript.instance.menuFocus = 0;
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

    public void DisableTutorial()
    {
        tutorialHolder.SetActive(false);

    }
}
