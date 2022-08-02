using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputScript : MonoBehaviour
{
    public float rotationSpeed = 2.0f;
    public float maxRotationAngle = 10f;
    public float movementMultiplier = 2;
    public TestPlayerScript playerScript;
    public GameObject debugPanel;

    private Touch _touch;

    private bool _firstTouch = false;
    public delegate void FirstTouchAction();
    public static event FirstTouchAction OnFirstTouched;

    void Start()
    {
        if (debugPanel != null)
            debugPanel.SetActive(false);
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
                transform.Rotate(
                    _touch.deltaPosition.y * Time.deltaTime * movementMultiplier, 0f,
                    _touch.deltaPosition.x * Time.deltaTime * -movementMultiplier
                    );
            }
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
        playerScript.playerInput.y = x / maxRotationAngle;
        playerScript.playerInput.x = -z / maxRotationAngle;
    }

    public void EnableDebugPanel()
    {
        debugPanel.SetActive(!debugPanel.activeSelf);
    }
}
