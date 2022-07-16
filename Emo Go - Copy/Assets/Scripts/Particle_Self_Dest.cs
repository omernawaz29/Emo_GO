using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Self_Dest : MonoBehaviour
{
    public float destroy_time = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Dest", destroy_time);
    }

    void Dest()
    {
         Destroy(gameObject);
    }

}
