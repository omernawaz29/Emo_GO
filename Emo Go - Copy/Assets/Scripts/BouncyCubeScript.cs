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

    private void Start()
    {
        _uses = 0;
    }
    private void OnCollisionEnter(Collision collision)
    {
        var newDirection = bounceDirectionHelper.transform.position - transform.position;

        collision.rigidbody.velocity = Vector3.zero;
        collision.rigidbody.velocity = newDirection * bounceForce;

        if(++_uses == useAmount)
        {
            Destroy(gameObject);
            Instantiate(destroyEffect, gameObject.transform.position, Quaternion.identity);
        }

    }
}
