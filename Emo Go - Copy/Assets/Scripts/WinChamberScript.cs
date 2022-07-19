using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinChamberScript : MonoBehaviour
{
    private GameManagerScript _gameManager;
    

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManagerScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Emo" || other.tag == "AngryEmo")
        {
            _gameManager.RescueEmo();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Emo" || other.tag == "AngryEmo")
        {
            _gameManager.UnRescueEmo();
        }
    }
}
