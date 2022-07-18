using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBreakScript : MonoBehaviour
{

    [SerializeField] private ParticleSystem glassBreakParticles;
    [SerializeField] private Material brokenGlassMaterial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Instantiate(glassBreakParticles, transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<MeshRenderer>().material = brokenGlassMaterial; 
        Destroy(gameObject,0.15f);
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
