using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoScript : MonoBehaviour
{

    [SerializeField] private GameObject AngryEmo;
    [SerializeField] private GameObject NormalEmo;
    [SerializeField] private ParticleSystem emoPoofEffect;
    [SerializeField] private GameObject angryEmoEffect;
    public float maxHitForce;
    public ParticleSystem emoDieEffect;

    private GameManagerScript _gameManager;
    private AudioManager _audioManager;
    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManagerScript>();
        _audioManager = FindObjectOfType<AudioManager>();
        _gameManager.AddEmo();
    }
    void Start()
    {
        AngryEmo.SetActive(false);
        NormalEmo.SetActive(true);
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
                Handheld.Vibrate();
                Instantiate(emoDieEffect, gameObject.transform.position, Quaternion.identity);
                _gameManager.KillEmo();
                Destroy(gameObject);
            }
        }
        if(collision.gameObject.tag == "Glass")
        {
            if(gameObject.tag == "AngryEmo")
            {
                _audioManager.Play("GlassBreak");
                Handheld.Vibrate();

                Instantiate(emoPoofEffect, gameObject.transform.position, Quaternion.identity);

                angryEmoEffect.SetActive(false);
                AngryEmo.SetActive(false);
                NormalEmo.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Object")
        {
            Instantiate(emoDieEffect, gameObject.transform.position, Quaternion.identity);

            Handheld.Vibrate();
            _gameManager.KillEmo();
            Destroy(gameObject);
        }
        if(other.tag == "Burger")
        {
            gameObject.tag = "AngryEmo";

            Handheld.Vibrate();
            _audioManager.Play("BurgerPowerUp");
            Instantiate(emoPoofEffect, gameObject.transform.position, Quaternion.identity);


            angryEmoEffect.SetActive(true);
            AngryEmo.SetActive(true);
            NormalEmo.SetActive(false);

            Destroy(other.gameObject);
        }
    }
}
