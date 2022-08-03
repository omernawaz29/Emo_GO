using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static int finalScore = 0;


    private int _emosAlive = 0;
    private int _emosRescued = 0;
    private int _totalEmos = 0;
    private UIManagerScript _uiManager;
    private AudioManager _audioManager;

    [SerializeField] private GameObject[] FireZones;
    [SerializeField] private float fireZoneDelay = 1.5f;
    [SerializeField] private float firstFireZoneDelay = 0f;
    [SerializeField] private float minAliveEmo = 1;


    // 
    private int _trappingObjects = 0;

    private int nextFireZoneIndex = 0;
    // Start is called before the first frame update

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
        _uiManager = FindObjectOfType<UIManagerScript>();
        _audioManager = FindObjectOfType<AudioManager>();
        nextFireZoneIndex = 0;
        _trappingObjects = 0;
        // StartCoroutine(EnableFireZones());

    }

    // Update is called once per frame
    void Update()
    {

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


        if (_emosAlive < minAliveEmo || (_emosAlive == 1 && _trappingObjects > 0) )
        {
            Handheld.Vibrate();
            _uiManager.Invoke("Lose", 2f);
        }
        else if (_emosRescued == _emosAlive)
        {
            fireZoneDelay = 0;
            _uiManager.Invoke("Win", 2f);
            if (SaveManager.instance.state.levelsCompleted < GameManagerScript.instance.currentLevel)
            {
                SaveManager.instance.state.levelsCompleted = GameManagerScript.instance.currentLevel;
                SaveManager.instance.SaveGame();
            }
            GameManagerScript.instance.currentLevel++;

        }
    }

    public void RescueEmo()
    {
        _emosRescued++;
        _uiManager.SetRescuedEmoCount(_emosRescued);
        _audioManager.Play("EmojiYay");
        _uiManager.SetTotalEmos(_totalEmos);

        if (_emosRescued == _emosAlive)
        {
            fireZoneDelay = 0;
            _uiManager.Invoke("Win", 2f);
            if (SaveManager.instance.state.levelsCompleted < GameManagerScript.instance.currentLevel)
            {
                Handheld.Vibrate();
                SaveManager.instance.state.levelsCompleted = GameManagerScript.instance.currentLevel;
                SaveManager.instance.SaveGame();
            }
            GameManagerScript.instance.currentLevel++;

        }
    }
    public void UnRescueEmo()
    {
        _emosRescued--;
        _uiManager.SetRescuedEmoCount(_emosRescued);

    }
    void EnableFireZonesWrapper()
    {
        Debug.Log("Enabling firezones");
        _uiManager.DisableTutorial();

        StartCoroutine(EnableFireZones());
    }

    IEnumerator EnableFireZones()
    {
        yield return new WaitForSeconds(firstFireZoneDelay);
        while(true && FireZones.Length != 0)
        {
            FireZones[nextFireZoneIndex].SetActive(true);
            yield return new WaitForSeconds(fireZoneDelay);

            if (++nextFireZoneIndex >= FireZones.Length)
                break;
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
}
