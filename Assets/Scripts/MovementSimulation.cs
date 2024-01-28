using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSimulation : MonoBehaviour
{
    public float translationSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Translation
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.right * translationSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.left * translationSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * translationSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * translationSpeed * Time.deltaTime, Space.World);
        }

    }
}
