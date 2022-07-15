using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plateform_Controller_Alt : MonoBehaviour
{

    private float _xRotation;
    private float _zRotation;

    private Touch _touch;

    // Start is called before the first frame update
    void Start()
    {
        _xRotation = 0;
        _zRotation = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);

            if(_touch.phase == TouchPhase.Moved)
            {
                _xRotation += _touch.deltaPosition.y * Time.deltaTime;
                _zRotation -= _touch.deltaPosition.x * Time.deltaTime;
            }

        }

        _xRotation += Input.GetAxis("Vertical");
        _zRotation -= Input.GetAxis("Horizontal");

        _xRotation = Mathf.Clamp(_xRotation, -30, 30);
        _zRotation = Mathf.Clamp(_zRotation, -25, 25);

        gameObject.transform.rotation = Quaternion.Euler(_xRotation, 0, _zRotation);

    }
}
