using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrivialMovement : MonoBehaviour
{
    public float speed;
    public Vector3 moveDirection;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 curPos = transform.position;
            curPos += moveDirection * speed * Time.deltaTime;
            transform.position = curPos;
        } else if (Input.GetKey(KeyCode.S))
        {
            Vector3 curPos = transform.position;
            curPos -= moveDirection * speed * Time.deltaTime;
            transform.position = curPos;
        }
        
    }
}
