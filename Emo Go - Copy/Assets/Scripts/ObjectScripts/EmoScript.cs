using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoScript : MonoBehaviour
{

    [SerializeField] private GameObject AngryEmo;
    [SerializeField] private GameObject NormalEmo;
    [SerializeField] private ParticleSystem emoPoofEffect;
    [SerializeField] private GameObject angryEmoEffect;

    [SerializeField] GameObject[] Faces;
    public float maxHitForce;
    public ParticleSystem emoDieEffect;

    private LevelManager _levelManager;
    private AudioManager _audioManager;
    private void Awake()
    {
        _levelManager = FindObjectOfType<LevelManager>();
        _audioManager = FindObjectOfType<AudioManager>();
        _levelManager.AddEmo();
    }
    void Start()
    {
        UpdateFaces();
    }

    void UpdateFaces()
    {
        foreach (GameObject t in Faces)
        {
            t.SetActive(false);
        }
        Faces[SaveManager.instance.state.activeEmo].SetActive(true);

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
                CameraController.instance.StartShake(0.3f, 0.25f);
                Instantiate(emoDieEffect, gameObject.transform.position, Quaternion.identity);
                _levelManager.KillEmo();
                Destroy(gameObject);
            }
        }
        else if(collision.gameObject.tag == "Glass")
        {
            if(gameObject.tag == "AngryEmo")
            {
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
            _levelManager.KillEmo();
            CameraController.instance.StartShake(0.3f, 0.25f);
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
