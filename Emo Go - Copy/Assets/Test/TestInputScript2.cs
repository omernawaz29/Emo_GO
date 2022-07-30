using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInputScript2 : MonoBehaviour
{
    public float maxSpeed = 5.0f;
    public float maxAcceleration = 5.0f;
    public float bounciness = 0.5f;
    public float gravity = -9.8f;

    public Vector2 playerInput;

    Vector3 velocity;

    private CharacterController charController;

    private void Start()
    {
        // rb = GetComponent<Rigidbody>();
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Vector2 playerInput;
        // playerInput.x = Input.GetAxis("Horizontal");
        // playerInput.y = Input.GetAxis("Vertical");
        // playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        // print("Player input x " + playerInput.x + " Y : " + playerInput.y);

        CheckGround();

        Vector3 movement = new Vector3(0f, 0f, 0f);
        movement.y = gravity;
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);
        // charController.Move(movement);

        Vector3 desiredVelocity =
            new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        velocity.x =
            Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z =
            Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        Vector3 displacement = velocity * Time.deltaTime;
        Vector3 newPosition = transform.localPosition + displacement;

        transform.localPosition = newPosition;

        // ClampSpeed();
    }

    private void CheckGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            Debug.DrawLine(transform.position, hit.point, Color.cyan);
            Vector3 newPos = transform.position;
            newPos.y = hit.point.y;

            Collider collider = GetComponent<SphereCollider>();
            newPos.y += collider.bounds.size.x + 0.05f;
            transform.localPosition = newPos;
        }
    }

    /*
    private void ClampSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }
    */
}
