using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInputScript : MonoBehaviour
{
    public float rotationSpeed = 2.0f;
    public float maxRotationAngle = 10f;
    public float movementMultiplier = 5f;
    public TestPlayerScript[] playerScripts;

    private Touch _touch;

    private bool _firstTouch = false;
    public delegate void FirstTouchAction();
    public static event FirstTouchAction OnFirstTouched;

    private float _xRotation;
    private float _zRotation;

    void Start()
    {
        _xRotation = 0;
        _zRotation = 0;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (_firstTouch == false)
                OnFirstTouched();
            _firstTouch = true;

            _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Moved)
            {
                _xRotation = 
                    Mathf.Lerp(_xRotation, _xRotation + _touch.deltaPosition.y, Time.deltaTime * movementMultiplier);
                _zRotation = 
                    Mathf.Lerp(_zRotation, _zRotation - _touch.deltaPosition.x, Time.deltaTime * movementMultiplier);

                transform.Rotate(
                    _touch.deltaPosition.y * Time.deltaTime * movementMultiplier, 0f, 
                    _touch.deltaPosition.x * Time.deltaTime * -movementMultiplier
                    );
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0f, 0f, -rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0f, 0f, rotationSpeed);
        } 
        else if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(rotationSpeed, 0f, 0f);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(-rotationSpeed, 0f, 0f);
        }

        LimitRot();
    }

    private void LimitRot()
    {
        Vector3 playerEulerAngles = transform.rotation.eulerAngles;

        playerEulerAngles.x = (playerEulerAngles.x > 180) ? playerEulerAngles.x - 360 : playerEulerAngles.x;
        playerEulerAngles.x = Mathf.Clamp(playerEulerAngles.x, -maxRotationAngle, maxRotationAngle);

        playerEulerAngles.y = 0;

        playerEulerAngles.z = (playerEulerAngles.z > 180) ? playerEulerAngles.z - 360 : playerEulerAngles.z;
        playerEulerAngles.z = Mathf.Clamp(playerEulerAngles.z, -maxRotationAngle, maxRotationAngle);

        transform.rotation = Quaternion.Euler(playerEulerAngles);

        SetPlayerInput(playerEulerAngles.x, playerEulerAngles.z);
    }

    private void SetPlayerInput(float x, float z)
    {
        foreach (TestPlayerScript playerScript in playerScripts)
        {
            playerScript.playerInput.y = x / maxRotationAngle;
            playerScript.playerInput.x = -z / maxRotationAngle;
        }
    }
}
