using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DuplicateCamera : MonoBehaviour
{
    [SerializeField] private Camera _originCam;

    // Update is called once per frame
    void Update()
    {
        transform.position = _originCam.transform.position;
        transform.rotation = _originCam.transform.rotation;
    }
}
