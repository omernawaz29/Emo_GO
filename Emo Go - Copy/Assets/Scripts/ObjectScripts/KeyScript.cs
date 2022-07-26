using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [SerializeField] DoorScript doorScript;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Emo")
        {
            Destroy(gameObject);
            doorScript.OpenDoor();
        }
    }
}
