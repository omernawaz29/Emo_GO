using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirezoneScript : MonoBehaviour
{
    Collider fireZoneCollider;
    [SerializeField] GameObject actualFire;
    [SerializeField] GameObject prefireSmoke;


    private void OnEnable()
    {
        fireZoneCollider = GetComponent<Collider>();
        fireZoneCollider.enabled = false;
        actualFire.SetActive(false);
        prefireSmoke.SetActive(true);
        StartCoroutine(SmoketoFire());
    }

    IEnumerator SmoketoFire()
    {
        yield return new WaitForSeconds(1.5f);
        actualFire.SetActive(true);
        fireZoneCollider.enabled = true;

        yield return new WaitForSeconds(0.5f);
        prefireSmoke.SetActive(false);
    }



}
