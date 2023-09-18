using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public static int finalScore = 0;


    private int _emosAlive = 0;
    private int _emosRescued = 0;
    private int _totalEmos = 0;
    private UIManagerScript _uiManager;
    [HideInInspector] public AudioManager _audioManager;

    [SerializeField] private float delay_Win_Lose_Screen = 1;

    [SerializeField] private GameObject[] FireZones;
    [SerializeField] private float fireZoneDelay = 1.5f;
    [SerializeField] private float firstFireZoneDelay = 0f;


    // Timer to see player gameplay time in each level
    float Timer = 0;
    bool TimerIsRunning = true;
    int Minutes = 0;
    int Seconds = 0;

    // 
    [SerializeField] private int _trappingObjects = 0;

    private int nextFireZoneIndex = 0;

    [Tooltip("It's by default is false and only gets true if emo wins while, It is to prevent coins going 0 when emo who is left dies")]
    private bool EmoLeftBehind = false;

    private void OnEnable()
    {
        PlatformController.OnFirstTouched += EnableFireZonesWrapper;
        TestInputScript.OnFirstTouched += EnableFireZonesWrapper;
    }

    private void OnDisable()
    {
        PlatformController.OnFirstTouched -= EnableFireZonesWrapper;
        TestInputScript.OnFirstTouched -= EnableFireZonesWrapper;
    }

    void Start()
    {
        instance = this;

        TimerIsRunning = true;

        _uiManager = FindObjectOfType<UIManagerScript>();
        _audioManager = FindObjectOfType<AudioManager>();
        //Debug.Log(_audioManager);
        nextFireZoneIndex = 0;

        EmoLeftBehind = false;

        // StartCoroutine(EnableFireZones());

    }

    // Ahmed's Timer Stuff for Firebase
    private void Update()
    {
        if (TimerIsRunning)
        {
            Timer += Time.deltaTime;
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        TimerIsRunning = false;

        Minutes = Mathf.FloorToInt(timeToDisplay / 60);
        Seconds = Mathf.FloorToInt(timeToDisplay % 60);

        Timer = 0;
    }

    public void AddEmo()
    {
        _emosAlive++;
        _totalEmos++;
    }

    public void KillEmo()
    {

        _emosAlive--;
        _audioManager.Play("EmojiPop");
        _uiManager.SetTotalEmos(_totalEmos);
        if (_emosAlive == _trappingObjects)
        {
            Handheld.Vibrate();
            //---> _uiManager.Invoke("Lose", delay_Win_Lose_Screen);
            StartCoroutine(Lose());
            LogAnalyticData();

        }
        else if (_emosRescued == _emosAlive - _trappingObjects)
        {
            foreach (var f in FireZones)
            {
                f.GetComponent<FirezoneScript>().smokeTime = 0f;
            }
            fireZoneDelay = 0;
            //---> _uiManager.Invoke("Win", delay_Win_Lose_Screen);
            StartCoroutine(Won());
            EmoLeftBehind = true;
            LogAnalyticData();

            // This Code is acting weird and saving and playing levels weirdly on some occutions So now it's updating in Win Function in UIManagerScript
            /*
            if (SaveManager.instance.state.levelsCompleted < GameManagerScript.instance.currentLevel)
            {
                Handheld.Vibrate();
                SaveManager.instance.state.levelsCompleted = GameManagerScript.instance.currentLevel;
                SaveManager.instance.SaveGame();
            }
            GameManagerScript.instance.currentLevel++;
            */

        }
    }
    IEnumerator Won()
    {
        DisplayTime(Timer);

        Debug.Log("Level_Completed_" + (PlayerPrefs.GetInt("CurrentLevel") - 1));
        Debug.Log("Level_Win_Time_" + "Minutes_" + Minutes + "_Seconds_" + Seconds + "._In_Level_" + (PlayerPrefs.GetInt("CurrentLevel") - 1));
        Debug.Log("Emojis_Alive_" + _emosRescued + "._In_Level_" + (PlayerPrefs.GetInt("CurrentLevel") - 1));
        FirebaseManager.instance.LogLevelCompleteEvent(PlayerPrefs.GetInt("CurrentLevel") - 1);
        FirebaseManager.instance.LogEvent("Time_Played_Won", "Minutes_" + Minutes + "_Seconds_" + Seconds, +PlayerPrefs.GetInt("CurrentLevel") - 1);
        FirebaseManager.instance.LogEvent("Emojis_Alive", "Alive_" + _emosRescued, PlayerPrefs.GetInt("CurrentLevel") - 1);

        yield return new WaitForSeconds(delay_Win_Lose_Screen);
        UIManagerScript.instance.Win();
    }
    IEnumerator Lose()
    {
        DisplayTime(Timer);

        Debug.Log("Level_Lose_" + (PlayerPrefs.GetInt("CurrentLevel") - 1));
        FirebaseManager.instance.LogLevelFailedEvent(PlayerPrefs.GetInt("CurrentLevel") - 1);

        Debug.Log("Level_Lose_Time_" + "Minutes_" + Minutes + "_Seconds_" + Seconds + "_In_Level_" + (PlayerPrefs.GetInt("CurrentLevel") - 1));
        FirebaseManager.instance.LogEvent("Time_Played_Lost", "Minutes_" + Minutes + "_Seconds_" + Seconds, +PlayerPrefs.GetInt("CurrentLevel") - 1);

        yield return new WaitForSeconds(delay_Win_Lose_Screen);
        UIManagerScript.instance.Lose(EmoLeftBehind);
    }

    public void RescueEmo()
    {

        _emosRescued++;
        _uiManager.SetRescuedEmoCount(_emosRescued);
        _audioManager.Play("EmojiYay");
        _uiManager.SetTotalEmos(_totalEmos);
        if (_emosRescued == _emosAlive - _trappingObjects)
        {
            fireZoneDelay = 0;
            //---> _uiManager.Invoke("Win", delay_Win_Lose_Screen);
            StartCoroutine(Won());
            LogAnalyticData();

            // This Code is acting weird and saving and playing levels weirdly on some occutions So now it's updating in Win Function in UIManagerScript
            /*
            if (SaveManager.instance.state.levelsCompleted < GameManagerScript.instance.currentLevel)
            {
                Handheld.Vibrate();
                SaveManager.instance.state.levelsCompleted = GameManagerScript.instance.currentLevel;
                SaveManager.instance.SaveGame();
            }
            GameManagerScript.instance.currentLevel++;
            */

        }
    }
    public void UnRescueEmo()
    {
        _emosRescued--;
        _uiManager.SetRescuedEmoCount(_emosRescued);

    }
    void EnableFireZonesWrapper()
    {
        //Debug.Log("Enabling firezones");
        _uiManager.DisableTutorial();

        StartCoroutine(EnableFireZones());
    }

    IEnumerator EnableFireZones()
    {
        yield return new WaitForSeconds(firstFireZoneDelay);
        while (true && FireZones.Length != 0)
        {
            FireZones[nextFireZoneIndex].SetActive(true);
            yield return new WaitForSeconds(fireZoneDelay);

            if (++nextFireZoneIndex >= FireZones.Length)
            {

                _uiManager.SetTotalEmos(_totalEmos);

                if (_emosAlive == _trappingObjects)
                {
                    Handheld.Vibrate();
                    //---> _uiManager.Invoke("Lose", 2f);
                    StartCoroutine(Lose());
                    LogAnalyticData();

                }
                break;
            }
        }
    }

    public void AddTrappingObject()
    {
        _trappingObjects++;
    }

    public void RemoveTrappingObject()
    {
        _trappingObjects--;
    }

    private void LogAnalyticData()
    {
        string currentLevel = (SceneManager.GetActiveScene().buildIndex - 1).ToString();

        AnalyticsResult resultLog =
            Analytics.CustomEvent("levelComplete",
                new Dictionary<string, object> { { "CurrentLevel_EmosRescued_TotalEmos", currentLevel + "_" + _emosRescued.ToString() + "_" + _totalEmos } });

        //Debug.Log(resultLog);

    }

    public void StarSound()
    {

    }
}