using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoScript : MonoBehaviour
{
    public float maxHitForce;
    public ParticleSystem emoDieEffect;

    private GameManagerScript _gameManager;
    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManagerScript>();
        _gameManager.AddEmo();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Object")
        {
            // To check it object hit hard enough for code to execute
            if (collision.relativeVelocity.magnitude >= maxHitForce)
            {
                Instantiate(emoDieEffect, gameObject.transform.position, Quaternion.identity);
                _gameManager.KillEmo();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Object")
        {
            Instantiate(emoDieEffect, gameObject.transform.position, Quaternion.identity);

            _gameManager.KillEmo();
            Destroy(gameObject);
        }
        if(other.tag == "Burger")
        {
            gameObject.tag = "AngryEmo";
            Destroy(other.gameObject);
        }
    }
}
