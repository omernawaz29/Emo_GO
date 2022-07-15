using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win_Checker : MonoBehaviour
{
    [HideInInspector] public float rescued_num;
    public float rescued_max;

    [HideInInspector] public float rescuable;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Emo")
        {
            rescued_num += 1;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Emo")
        {
            rescued_num -= 1;
        }
    }
}
