using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinChamberScript : MonoBehaviour
{
    private LevelManager _levelManager;
    [SerializeField] ParticleSystem winEffect;
    

    private void Awake()
    {
        _levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Emo" || other.tag == "AngryEmo")
        {
            Vector3 effectPos = new Vector3(other.transform.position.x, other.transform.position.y + 1, other.transform.position.z);
            Instantiate(winEffect, effectPos, Quaternion.Euler(90, 0, 0));
            _levelManager.RescueEmo();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Emo" || other.tag == "AngryEmo")
        {
            _levelManager.UnRescueEmo();
        }
    }
}
