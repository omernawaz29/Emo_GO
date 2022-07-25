using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControllerAlt : MonoBehaviour
{

    private float _xRotation;
    private float _zRotation;
    private float _movementMultiplier = 7f;

    private Touch _touch;

    Camera _cam;


    Vector3 _camNormalPos;
    Vector3 _camUpPos;
    Quaternion _camNormalRotation;
    Quaternion _camUpRotation;

    // Start is called before the first frame update
    void Start()
    {
        _xRotation = 0;
        _zRotation = 0;
        _cam = Camera.main;

        _camNormalPos = _cam.transform.position;
        _camUpPos = new Vector3(_cam.transform.position.x, 20f, _cam.transform.position.z + 8);

        _camNormalRotation = _cam.transform.rotation;
        _camUpRotation = Quaternion.Euler(76, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);

            if(_touch.phase == TouchPhase.Moved)
            {
                _xRotation = Mathf.Lerp(_xRotation, _xRotation + _touch.deltaPosition.y, Time.deltaTime * _movementMultiplier);
                _zRotation = Mathf.Lerp(_zRotation, _zRotation - _touch.deltaPosition.x, Time.deltaTime * _movementMultiplier);
            }

        }

        _xRotation += Input.GetAxis("Vertical");
        _zRotation -= Input.GetAxis("Horizontal");

        _xRotation = Mathf.Clamp(_xRotation, -30, 30);
        _zRotation = Mathf.Clamp(_zRotation, -25, 25);

        gameObject.transform.rotation = Quaternion.Euler(_xRotation, 0, _zRotation);

        if(_xRotation >= 27)
        {
            _cam.transform.position = Vector3.Lerp(_cam.transform.position, _camUpPos, 5 * Time.deltaTime);
            _cam.transform.rotation = Quaternion.Slerp(_cam.transform.rotation, _camUpRotation, 5 * Time.deltaTime);
        }
        else
        {
            _cam.transform.position = Vector3.Lerp(_cam.transform.position, _camNormalPos, 5 * Time.deltaTime);
            _cam.transform.rotation = Quaternion.Slerp(_cam.transform.rotation, _camNormalRotation, 5 * Time.deltaTime);
        }

    }
}
