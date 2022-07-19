using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyCubeScript : MonoBehaviour
{

    [SerializeField] float bounceForce = 10;
    [SerializeField] GameObject bounceDirectionHelper;


    private void OnCollisionEnter(Collision collision)
    {
        var newDirection = bounceDirectionHelper.transform.position - transform.position;

        collision.rigidbody.velocity = Vector3.zero;
        collision.rigidbody.velocity = newDirection * bounceForce;

    }
}
