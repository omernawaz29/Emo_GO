using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;
using System;

public class UIManagerScript : MonoBehaviour
{
    public static UIManagerScript instance;

    // Camera Effect
    [HideInInspector] public DOTweenAnimation CamShake;

    //Save And Level Stuff
    [SerializeField] private int TotalLevels;

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
    [SerializeField] private GameObject coinAnimationHolder;

    //Win Screen Objects
    [SerializeField] private GameObject NextLevelButton;
    [SerializeField] private GameObject GlassesEmo;


    //Lose Screen Objects
    [SerializeField] private GameObject RestartLevelButton;
    [SerializeField] private GameObject SadEmo;
    private bool wonCheck = false;

    [Space]
    [SerializeField] private float starsAnimationSpeed = 2.0f;
    [SerializeField] private float starsDelayOffset = 0.1f;

    [Space]
    [Header("Ahmed's Stars Work")]
    [SerializeField] private GameObject[] StarsBG;
    [SerializeField] private float Star_End_Screen_Delay = 0.2f;
    [Space]
    [SerializeField] private GameObject[] Stars;
    [SerializeField] private ParticleSystem Star_Puff;
    int star_Active_Number;

    [Space]
    [Tooltip("For Testing events or stuff")]
    public TextMeshProUGUI debugText;

    [Space]
    [SerializeField] private GameObject Retry_Button;


    private float _totalEmos = 0;
    private float _rescuedEmos = 0;

    void Start()
    {

        instance = this;

        Debug.Log("Level intial load. Level : " + (PlayerPrefs.GetInt("CurrentLevel") - 1));
        FirebaseManager.instance.LogEvent("Game_Started", "Initial_Level_Load_", PlayerPrefs.GetInt("CurrentLevel") - 1);

        CamShake = Camera.main.GetComponent<DOTweenAnimation>();

        rescuedEmoCountText.text = "0";
        //---> currentLevelText.text = (SceneManager.GetActiveScene().buildIndex - 1).ToString();

        tutorialHolder.SetActive(true);

        if (PlayerPrefs.HasKey("CurrentLevelNumber"))
        {
            currentLevelText.text = PlayerPrefs.GetInt("CurrentLevelNumber").ToString();
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevelNumber", 1);
        }

    }

    public void Win()
    {
        Debug.Log("Stars Collected : " + star_Active_Number + ". Won in Level : " + (PlayerPrefs.GetInt("CurrentLevel") - 1));
        FirebaseManager.instance.LogEvent("Stars_Collected", "Won_" + star_Active_Number, PlayerPrefs.GetInt("CurrentLevel") - 1);

        PlayerPrefs.SetInt("CurrentLevelNumber", PlayerPrefs.GetInt("CurrentLevelNumber") + 1);
        PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);

        Time.timeScale = 0.5f;
        wonCheck = true;
        endScreen.SetActive(true);
        NextLevelButton.SetActive(true);
        GlassesEmo.SetActive(true);
        Retry_Button.SetActive(false);
        // SetEndStars();

        // Coin Animation
        float endPercentage = (_rescuedEmos * 100) / _totalEmos;

        if (endPercentage < 66 && endPercentage > 0)
        {
            endPercentage += 20f;
        }

        int coins = Mathf.CeilToInt(endPercentage * 10);

        coinText.text = "0";

        if (coins != 0)
        {
            StartCoroutine(SetEndCoinsJuice(coins));
        }

        SaveManager.instance.state.coins += coins;
        PlayerPrefs.Save();

        StartCoroutine(UIStarUpdate());

    }
    public void Lose(bool EmoLeftBehind)
    {
        Debug.Log("Stars Collected : " + star_Active_Number + ". in Failed Level : " + (PlayerPrefs.GetInt("CurrentLevel") - 1));
        FirebaseManager.instance.LogEvent("Stars_Collected", "Lost_" + star_Active_Number, PlayerPrefs.GetInt("CurrentLevel") - 1);

        if (wonCheck)
        {
            Debug.Log("Lost After Winning the Level, in Level : " + (PlayerPrefs.GetInt("CurrentLevel") - 1));
            FirebaseManager.instance.LogEvent("Lost", "After_Level_Won", PlayerPrefs.GetInt("CurrentLevel") - 1);
        }

        Time.timeScale = 0.5f;
        if (EmoLeftBehind)
        {
            coinText.text = "0";
        }
        endScreen.SetActive(true);
        RestartLevelButton.SetActive(true);
        SadEmo.SetActive(true);
        Retry_Button.SetActive(false);
        // SetEndStars();

        StartCoroutine(UIStarUpdate());

        wonCheck = false;

    }

    // Ahmed's Star Work
    public void UpdateStars(Transform Star_Transform)
    {
        if (Stars != null && star_Active_Number <= Stars.Length)
        {
            if (Star_Puff != null)
            {
                Star_Puff.transform.position = Star_Transform.position;
                Star_Puff.Play();
            }
            LevelManager.instance._audioManager.Play("StarCollected");

            Stars[star_Active_Number].GetComponent<DOTweenAnimation>().DOPlay();
            star_Active_Number++;
        }

    }
    public IEnumerator UIStarUpdate()
    {
        if (StarsBG != null)
        {
            for (int i = 0; i < StarsBG.Length; i++)
            {
                StarsBG[i].GetComponent<DOTweenAnimation>().DOPlay();
                yield return new WaitForSecondsRealtime(Star_End_Screen_Delay);
            }
        }
    }

    private void SetEndStars()
    {
        float endPercentage = (_rescuedEmos * 100) / _totalEmos;

        if (endPercentage < 66 && endPercentage > 0)
            endPercentage += 20f;

        int coins = Mathf.CeilToInt(endPercentage * 10);

        coinText.text = "0";

        if (coins != 0)
            StartCoroutine(SetEndCoinsJuice(coins));



        SaveManager.instance.state.coins += coins;

        int starsShowCount = 0;
        if (endPercentage == 100)
            starsShowCount = 3;
        else if (endPercentage >= 66)
            starsShowCount = 2;
        else if (endPercentage >= 33)
            starsShowCount = 1;

        StartCoroutine(SetEndStarsJuice(starsShowCount));

        /*
        if (endPercentage >= 33)
            stars[0].SetActive(true);
        if (endPercentage >= 66)
            stars[1].SetActive(true);
        if (endPercentage == 100)
            stars[2].SetActive(true);
        */
    }

    IEnumerator SetEndStarsJuice(int count)
    {
        for (int i = 0; i < count; i++)
        {
            stars[i].SetActive(true);
            Animator animator = stars[i].GetComponent<Animator>();
            animator.speed = starsAnimationSpeed;

            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length - starsDelayOffset);
        }
    }

    IEnumerator SetEndCoinsJuice(int finalCoins)
    {


        coinAnimationHolder.SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        int coins = 0;
        while (coins < finalCoins)
        {
            coins += 15;

            if (coins > finalCoins)
                coins = finalCoins;

            coinText.text = coins.ToString();
            yield return null;
        }

    }

    public void ClickNextButton()
    {
        Debug.Log("Next Button Pressed After Completing Level : " + (PlayerPrefs.GetInt("CurrentLevel") - 2));
        FirebaseManager.instance.LogEvent("Next", "Button_Pressed_After_Completing_Level_", PlayerPrefs.GetInt("CurrentLevel") - 2);
        Time.timeScale = 1;

        /*var newSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (newSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1;
            GameManagerScript.instance.menuFocus = 2;
            SceneManager.LoadScene("MainMenu");
            newSceneIndex = SceneManager.GetActiveScene().buildIndex - 10;

        }*/
        if (PlayerPrefs.GetInt("CurrentLevel") >= TotalLevels + 2)
        {
            Time.timeScale = 1;
            PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") - 11);

            Debug.Log("Level Looped After 20th Level");
            FirebaseManager.instance.LogEvent("Level_Looped_After_20th_Level");
        }
        if(PlayerPrefs.GetInt("CurrentLevel") % 2 ==0 )
        {
            InterstitialAds.instance.ShowInterstitialAd();
        }
        else
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
        }

        //---> SceneManager.LoadScene(newSceneIndex);
    }

    public void ClickRedoButton()
    {
        Debug.Log("Retry Button Pressed After Failing : " + (PlayerPrefs.GetInt("CurrentLevel") - 1));
        FirebaseManager.instance.LogEvent("Retry", "Button_Pressed_After_Failing_Level_", PlayerPrefs.GetInt("CurrentLevel") - 1);

        Time.timeScale = 1;
        //---> SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
    }

    public void ClickRedoInGameplayButton()
    {
        Debug.Log("Retry Gameplay Button Pressed in Level : " + (PlayerPrefs.GetInt("CurrentLevel") - 1));
        FirebaseManager.instance.LogEvent("Retry_Gameplay", "Button_Pressed_in_Level_", PlayerPrefs.GetInt("CurrentLevel") - 1);

        Time.timeScale = 1;
        //---> SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
    }

    public void ClickHomeButton()
    {
        Time.timeScale = 1;
        GameManagerScript.instance.menuFocus = 0;
        SceneManager.LoadScene("MainMenu");
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
        Debug.Log("Level Started : " + (PlayerPrefs.GetInt("CurrentLevel") - 1));
        FirebaseManager.instance.LogLevelStartedEvent(PlayerPrefs.GetInt("CurrentLevel") - 1);

        tutorialHolder.SetActive(false);

    }
}