using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] ParticleSystem poofEffect;
    [SerializeField] bool trapsEmoji = false;

    Animator _animator;
    LevelManager _levelManager;

    private void Start()
    {


        _animator = GetComponent<Animator>();
        _levelManager = FindObjectOfType<LevelManager>();

        // For some reason on mobile it was not updating so the value is assigned manually on LevelManager
        /*if (trapsEmoji)
        {
            LevelManager.instance.AddTrappingObject();
        }*/
    }
    public void OpenDoor()
    {
        if(!_animator)
        {
            Debug.LogError("No Animator On Door!");
            return;
        }

        _animator.Play("DoorAnimation");
        AudioManager.instance.Play("KeyPowerUp");
    }

    public void DestroyMe()
    {
        if (trapsEmoji)
        {
            LevelManager.instance.RemoveTrappingObject();
        }
        Instantiate(poofEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
