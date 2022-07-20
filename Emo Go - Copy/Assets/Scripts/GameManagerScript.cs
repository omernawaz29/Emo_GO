using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static int finalScore = 0;

    private int _emosAlive = 0;
    private int _emosRescued = 0;
    private int _totalEmos = 0;
    private UIManagerScript _uiManager;
    private AudioManager _audioManager;

    [SerializeField] private GameObject[] FireZones;
    [SerializeField] private float fireZoneDelay = 1.5f;


    private int nextFireZoneIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = FindObjectOfType<UIManagerScript>();
        _audioManager = FindObjectOfType<AudioManager>();
        nextFireZoneIndex = 0;
        StartCoroutine(EnableFireZones());

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
        if (_emosAlive == 0)
            _uiManager.Invoke("Lose", 1f);
        else if (_emosRescued == _emosAlive)
            _uiManager.Invoke("Win", 1f);
    }

    public void RescueEmo()
    {
        _emosRescued++;
        _uiManager.SetRescuedEmoCount(_emosRescued);
        _audioManager.Play("EmojiYay");
        _uiManager.SetTotalEmos(_totalEmos);
        if (_emosRescued == _emosAlive)
            _uiManager.Invoke("Win", 1f);
    }

    public void UnRescueEmo()
    {
        _emosRescued--;
        _uiManager.SetRescuedEmoCount(_emosRescued);

    }

    IEnumerator EnableFireZones()
    {
        while(true && FireZones.Length != 0)
        {
            FireZones[nextFireZoneIndex].SetActive(true);
            yield return new WaitForSeconds(fireZoneDelay);

            if (++nextFireZoneIndex < FireZones.Length) { }
            else break;
        }
    }
}
