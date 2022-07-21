using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{

    [SerializeField] private Transform anchorPoint;
    [SerializeField] private Transform downPoint;
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float emojisRequired = 2;
    private int _emojisOn = 0;
    private Vector3 _newPos;
    // Start is called before the first frame update
    void Start()
    {
        _newPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(_emojisOn >= emojisRequired)
            _newPos = Vector3.Lerp(transform.position, downPoint.position, moveSpeed * Time.deltaTime);
        else
            _newPos = Vector3.Lerp(transform.position, anchorPoint.position, moveSpeed * Time.deltaTime);

        transform.position = _newPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Emo" || other.tag == "AngryEmo")
        {
            _emojisOn++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Emo" || other.tag == "AngryEmo")
        {
            Invoke("RemoveEmoji", 1f);
        }
    }

    void RemoveEmoji()
    {
        _emojisOn--;
    }
}
