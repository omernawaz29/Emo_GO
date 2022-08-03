using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float camSpeed = 5;


    public static CameraController instance;

    Camera _cam;

    Vector3 _camNormalPos;
    Vector3 _camUpPos;
    Quaternion _camNormalRotation;
    Quaternion _camUpRotation;

    //PlatformController platformController;

    private void OnLevelWasLoaded(int level)
    {
        _cam = Camera.main;

        _camNormalPos = _cam.transform.position;
        _camUpPos = new Vector3(_cam.transform.position.x, 20f, _cam.transform.position.z + 8);

        _camNormalRotation = _cam.transform.rotation;
        _camUpRotation = Quaternion.Euler(76, 0, 0);

        //platformController = FindObjectOfType<PlatformController>();

        //if (platformController == null)
        //{
        //    Debug.LogWarning("PlatForm Controller not Found!");
        //}
    }

    private void Start()
    {

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        //if (!platformController)
        //    return;

        //if (platformController.rotateX >= 20)
        //{
        //    _cam.transform.position = Vector3.Lerp(_cam.transform.position, _camUpPos, camSpeed * Time.deltaTime);
        //    _cam.transform.rotation = Quaternion.Slerp(_cam.transform.rotation, _camUpRotation, camSpeed * Time.deltaTime);
        //}
        //else
        //{
        //    _cam.transform.position = Vector3.Lerp(_cam.transform.position, _camNormalPos, camSpeed * Time.deltaTime);
        //    _cam.transform.rotation = Quaternion.Slerp(_cam.transform.rotation, _camNormalRotation, camSpeed * Time.deltaTime);
        //}
    }


    public void StartShake(float duration, float magnitude)
    {
        StartCoroutine(CameraController.instance.Shake(duration, magnitude));
    }

    IEnumerator Shake (float duration, float magnitude)
    {
        float elapsed = 0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            _cam.transform.localPosition = new Vector3(_cam.transform.localPosition.x + x, _cam.transform.localPosition.y + y, _cam.transform.localPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
    }
}
