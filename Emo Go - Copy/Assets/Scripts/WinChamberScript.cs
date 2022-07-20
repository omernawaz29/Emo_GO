using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinChamberScript : MonoBehaviour
{
    private GameManagerScript _gameManager;
    [SerializeField] ParticleSystem winEffect;
    

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManagerScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Emo" || other.tag == "AngryEmo")
        {
            Vector3 effectPos = new Vector3(other.transform.position.x, other.transform.position.y + 1, other.transform.position.z);
            Instantiate(winEffect, effectPos, Quaternion.Euler(90, 0, 0));
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
