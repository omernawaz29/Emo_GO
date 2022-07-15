using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoScript : MonoBehaviour
{
    public float max_hit_force;
    public ParticleSystem pr;

    public GameObject win_check;
    // Start is called before the first frame update
    void Start()
    {
        
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
            if (collision.relativeVelocity.x >= max_hit_force || collision.relativeVelocity.y >= max_hit_force || collision.relativeVelocity.z >= max_hit_force)
            {
                Instantiate(pr, gameObject.transform.position, Quaternion.identity);
                win_check.GetComponent<Win_Checker>().rescued_max -= 1;
                Destroy(gameObject);
            }
        }
    }
}
