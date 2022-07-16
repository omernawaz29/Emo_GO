using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    private int _emosAlive = 0;
    private int _emosRescued = 0;
    private UIManagerScript _uiManager;


    [SerializeField] private GameObject[] FireZones;
    [SerializeField] private float fireZoneDelay = 1.5f;


    private int nextFireZoneIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = FindObjectOfType<UIManagerScript>();

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
    }

    public void KillEmo()
    {
        _emosAlive--;
        if (_emosAlive == 0)
            _uiManager.Invoke("Lose", 0.2f);
        else if (_emosRescued == _emosAlive)
            _uiManager.Invoke("Win", 0.2f);
    }

    public void RescueEmo()
    {
        _emosRescued++;
        _uiManager.SetRescuedEmoCount(_emosRescued);
        if (_emosRescued == _emosAlive)
            _uiManager.Invoke("Win", 0.2f);
    }

    public void UnRescueEmo()
    {
        _emosRescued--;
        _uiManager.SetRescuedEmoCount(_emosRescued);
    }

    IEnumerator EnableFireZones()
    {
        while(true)
        {
            FireZones[nextFireZoneIndex].SetActive(true);
            yield return new WaitForSeconds(fireZoneDelay);

            if (++nextFireZoneIndex < FireZones.Length) { }
            else break;
        }
    }
}
