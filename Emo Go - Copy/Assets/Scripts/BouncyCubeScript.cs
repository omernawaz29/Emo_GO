using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyCubeScript : MonoBehaviour
{

    [SerializeField] float BounceForce = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        var rb = collision.rigidbody;
        var lastVelocity = rb.velocity;

        var newDirection = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        rb.velocity = newDirection * BounceForce;

    }
}
