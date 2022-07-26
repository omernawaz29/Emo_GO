using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    Animator _animator;
    [SerializeField] ParticleSystem poofEffect;

    private void Start()
    {
        _animator = GetComponent<Animator>();
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
        Instantiate(poofEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
