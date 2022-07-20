using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{


    [SerializeField] GameObject Door;
    [SerializeField] ParticleSystem poofEffect;

    AudioManager _audioManager;
    // Start is called before the first frame update
    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Emo")
        {
            Destroy(gameObject);

            Instantiate(poofEffect, Door.transform.position, Quaternion.identity);
            _audioManager.Play("KeyPowerUp");
            Destroy(Door);
        }
    }
}
