using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoScript : MonoBehaviour
{
    public float max_hit_force;
    public ParticleSystem pr;

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
            if (collision.relativeVelocity.magnitude >= max_hit_force)
            {
                Instantiate(pr, gameObject.transform.position, Quaternion.identity);
                _gameManager.KillEmo();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Object")
        {
            Instantiate(pr, gameObject.transform.position, Quaternion.identity);

            _gameManager.KillEmo();
            Destroy(gameObject);
        }
    }
}
