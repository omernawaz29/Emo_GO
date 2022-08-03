using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerScript : MonoBehaviour
{
    public FloatBox maxSpeed = new FloatBox(5f);
    public FloatBox maxAcceleration = new FloatBox(5f);

    public float groundHoverBuffer = 0.05f;

    public Vector2 playerInput;

    Vector3 velocity, desiredVelocity;
    Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        maxSpeed = SaveManager.instance.settings.playerMaxSpeed;
        maxAcceleration = SaveManager.instance.settings.playerMaxAcceleration;
    }

    private void Update()
    {
        desiredVelocity =
            new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed.Value;
    }

    private void FixedUpdate()
    {
        UpdateVelocity();
    }

    private void UpdateVelocity()
    {
        velocity = body.velocity;
        float maxSpeedChange = maxAcceleration.Value * Time.deltaTime;
        velocity.x =
            Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z =
            Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
        body.velocity = velocity;
    }
    public void CheckGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            Debug.DrawLine(transform.position, hit.point, Color.cyan);
            Vector3 newPos = transform.position;
            newPos.y = hit.point.y;

            Collider collider = GetComponent<SphereCollider>();
            newPos.y += collider.bounds.size.x / 2 + groundHoverBuffer;
            transform.localPosition = newPos;
        }
        else
        {
            if (Physics.Raycast(transform.position, Vector3.up, out hit))
            {
                Debug.DrawLine(transform.position, hit.point, Color.cyan);
                Vector3 newPos = transform.position;
                newPos.y = hit.point.y;

                Collider collider = GetComponent<SphereCollider>();
                newPos.y += collider.bounds.size.x / 2 + hit.collider.bounds.size.y + groundHoverBuffer;
                transform.localPosition = newPos;
            }
        }
    }
}
