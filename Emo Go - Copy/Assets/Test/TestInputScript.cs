using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInputScript : MonoBehaviour
{
    public FloatBox rotationSpeed = new FloatBox(2f);
    public FloatBox maxRotationAngle = new FloatBox(10f);
    public FloatBox movementMultiplier = new FloatBox(5f);
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

        rotationSpeed = SaveManager.instance.settings.levelRotationSpeed;
        maxRotationAngle = SaveManager.instance.settings.levelMaxRotationAngle;
        movementMultiplier = SaveManager.instance.settings.levelMovementMultiplier;
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
                    Mathf.Lerp(_xRotation, _xRotation + _touch.deltaPosition.y, Time.deltaTime * movementMultiplier.Value * rotationSpeed.Value);
                _zRotation = 
                    Mathf.Lerp(_zRotation, _zRotation - _touch.deltaPosition.x, Time.deltaTime * movementMultiplier.Value * rotationSpeed.Value);

                transform.Rotate(
                    _touch.deltaPosition.y * Time.deltaTime * movementMultiplier.Value, 0f, 
                    _touch.deltaPosition.x * Time.deltaTime * -movementMultiplier.Value
                    );
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0f, 0f, -rotationSpeed.Value);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0f, 0f, rotationSpeed.Value);
        } 
        else if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(rotationSpeed.Value, 0f, 0f);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(-rotationSpeed.Value, 0f, 0f);
        }

        LimitRot();
    }

    private void LimitRot()
    {
        Vector3 playerEulerAngles = transform.rotation.eulerAngles;

        playerEulerAngles.x = (playerEulerAngles.x > 180) ? playerEulerAngles.x - 360 : playerEulerAngles.x;
        playerEulerAngles.x = Mathf.Clamp(playerEulerAngles.x, -maxRotationAngle.Value, maxRotationAngle.Value);

        playerEulerAngles.y = 0;

        playerEulerAngles.z = (playerEulerAngles.z > 180) ? playerEulerAngles.z - 360 : playerEulerAngles.z;
        playerEulerAngles.z = Mathf.Clamp(playerEulerAngles.z, -maxRotationAngle.Value, maxRotationAngle.Value);

        transform.rotation = Quaternion.Euler(playerEulerAngles);

        SetPlayerInput(playerEulerAngles.x, playerEulerAngles.z);
    }

    private void SetPlayerInput(float x, float z)
    {
        foreach (TestPlayerScript playerScript in playerScripts)
        {
            playerScript.playerInput.y = x / maxRotationAngle.Value;
            playerScript.playerInput.x = -z / maxRotationAngle.Value;
        }
    }
}
