using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

    private float _xRotation;
    private float _zRotation;
    private float _movementMultiplier = 5f;

    private Touch _touch;

    private bool _firstTouch = false;
    public delegate void FirstTouchAction();
    public static event FirstTouchAction OnFirstTouched;

    public float rotateX { get { return _xRotation; } } 

    void Start()
    {
        _xRotation = 0;
        _zRotation = 0;
    }

    void Update()
    {

        if(Input.touchCount > 0)
        {
            if (_firstTouch == false)
                OnFirstTouched();
            _firstTouch = true;

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
        
    }
}
