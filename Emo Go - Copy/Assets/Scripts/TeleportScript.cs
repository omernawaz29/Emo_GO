using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    [SerializeField] GameObject teleportLocation;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Emo" || other.tag == "AngryEmo")
        {
            other.transform.position = teleportLocation.transform.position;
            StartCoroutine(DisableInstantReturn());
        }
    }

    IEnumerator DisableInstantReturn()
    {
        var col = teleportLocation.GetComponent<Collider>();

        col.enabled = false;

        yield return new WaitForSeconds(2f);

        col.enabled = true;
    }

   
}
