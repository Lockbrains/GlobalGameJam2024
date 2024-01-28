using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attach : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 test;
    public Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        test = rb.velocity;

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            //pos = rb.position;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            //rb.velocity = new Vector3(0, 0, 0);
            //transform.position = pos;

            /*
            LayerMask mask = ~0;
            RaycastHit hit;

            if(Physics.Raycast(transform.position, Vector3.right, out hit, 0.1f, mask))
            {
                transform.position = hit.point; //new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }
            */

            
        }
    }
}