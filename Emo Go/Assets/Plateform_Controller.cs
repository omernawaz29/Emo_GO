using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plateform_Controller : MonoBehaviour
{
    public float rotation_speed;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Rotate(0, 0, rotation_speed * Time.deltaTime, Space.World);

        }
        if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Rotate(0, 0, -rotation_speed * Time.deltaTime, Space.World);

        }
        
    }

}
