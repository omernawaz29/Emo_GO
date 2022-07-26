using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyCubeScript : MonoBehaviour
{

    [SerializeField] float bounceForce = 10;
    [SerializeField] float useAmount = 3;

    [SerializeField] GameObject bounceDirectionHelper;
    [SerializeField] ParticleSystem destroyEffect;

    private float _uses;

    AudioManager _audioManager;
    Animator _anim;
    Collider _col;
    private void Start()
    {
        _uses = 0;
        _audioManager = FindObjectOfType<AudioManager>();
        _anim = GetComponent<Animator>();
        _col = GetComponent<Collider>();

    }
    private void OnCollisionEnter(Collision collision)
    {
        _col.enabled = false;
        StartCoroutine(DelayJumpSpam());


        var newDirection = bounceDirectionHelper.transform.position - transform.position;

        collision.rigidbody.velocity = Vector3.zero;
        collision.rigidbody.velocity = newDirection.normalized * bounceForce;

        _anim.Play("JumpPadAnimation");
        _audioManager.Play("Bounce");
        

        if(++_uses == useAmount)
        {
            Destroy(gameObject);
            Instantiate(destroyEffect, gameObject.transform.position, Quaternion.identity);
        }

    }

    IEnumerator DelayJumpSpam()
    {
        Debug.Log("Bounce Called");
        yield return new WaitForSeconds(1f);
        _col.enabled = true;
    }
}
