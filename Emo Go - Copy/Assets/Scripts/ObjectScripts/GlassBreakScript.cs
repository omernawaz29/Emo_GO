using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBreakScript : MonoBehaviour
{

    [SerializeField] private ParticleSystem glassBreakParticles;
    [SerializeField] private ParticleSystem hitEffectParticles;
    [SerializeField] private Material brokenGlassMaterial;
    [SerializeField] private float breakForce = 1;
    [SerializeField] bool normalEmoBreak = false;
    [SerializeField] bool trapsEmoji = false;
    // Start is called before the first frame update

    AudioManager _audioManager;
    LevelManager _levelManager;
    void Start()
    {
            

        _audioManager = FindObjectOfType<AudioManager>();
        _levelManager = FindObjectOfType<LevelManager>();

        if (trapsEmoji)
            _levelManager.AddTrappingObject();

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision Detected| Velocity: " + collision.relativeVelocity.magnitude + "BreakForce: " + breakForce + "Tag: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "AngryEmo" || (collision.gameObject.tag == "Emo" && normalEmoBreak) && collision.relativeVelocity.magnitude >= breakForce)
        {
            _audioManager.Play("GlassBreak");
            gameObject.GetComponent<MeshRenderer>().material = brokenGlassMaterial;

            CameraController.instance.StartShake(0.2f, 0.35f);
            Handheld.Vibrate();
            Instantiate(glassBreakParticles, transform.position, Quaternion.identity);
            Instantiate(hitEffectParticles, transform.position, Quaternion.identity);
            collision.gameObject.tag = "Emo";

            if (trapsEmoji)
                _levelManager.RemoveTrappingObject();

            Destroy(gameObject, 0.15f);
        }
        
    }
}
