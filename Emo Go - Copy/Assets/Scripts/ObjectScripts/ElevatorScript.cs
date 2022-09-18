using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{

    [SerializeField] private Transform anchorPoint;
    [SerializeField] private Transform downPoint;
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float emojisRequired = 2;
    [SerializeField] private Light[] Lights;
    private int _emojisOn = 0;
    private Vector3 _newPos;
    bool _moveDown = false;
    // Start is called before the first frame update
    void Start()
    {
        _newPos = transform.position;
        Lights[0].color = Color.red;
        Lights[1].color = Color.red;

        if (emojisRequired == 1)
            Lights[1].color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if(_moveDown)
            _newPos = Vector3.Lerp(transform.position, downPoint.position, moveSpeed * Time.deltaTime);
        else
            _newPos = Vector3.Lerp(transform.position, anchorPoint.position, moveSpeed * Time.deltaTime);

        transform.position = _newPos;

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Emo" || other.tag == "AngryEmo")
        {
            _emojisOn++;
            Lights[_emojisOn - 1].color = Color.white;
            Invoke("StartMoving", 0.75f);
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
        Lights[_emojisOn].color = Color.red;
        if (_emojisOn < emojisRequired)
            _moveDown = false;
    }

    void StartMoving()
    {
        if (_emojisOn >= emojisRequired)
        {
            _moveDown = true;
            AudioManager.instance.Play("Elevator");
        }
    }
}
