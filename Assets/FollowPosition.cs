using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    public Transform followAt;
    
    // Update is called once per frame
    void Update()
    {
        Vector3 curPos = transform.position;
        transform.position = new Vector3(followAt.position.x, followAt.position.y, followAt.position.z -20f);
    }
}
