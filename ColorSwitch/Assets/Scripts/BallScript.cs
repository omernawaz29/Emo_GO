using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] Color ballColor;
    private float ballSpeed = 5;


    void Start()
    {
        spriteRenderer.color = ballColor;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            rigidBody.velocity = Vector2.up * ballSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != gameObject.tag)
        {
            Destroy(gameObject);
        }
    }
}
