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
        transform.position = new Vector3(curPos.x, followAt.position.y, curPos.z);
    }
}
