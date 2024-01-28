using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItchyManager : MonoBehaviour
{
    [Header("Itchy Config")]
    [SerializeField] private float displacementRequired = 3.0f;
    // TODO: Sensitivity should increase overtime
    [SerializeField] private float sensitivity = 1.0f;
    [SerializeField] private float startUpTime = 6.0f;
    [SerializeField] private float lv1Height = 80.0f;
    [SerializeField] private float lv2Height = 170.0f;
    [SerializeField] private float lv3Height = 260.0f;
    [SerializeField] private float sensitivityInc = 1.2f;

    [Header("lv Config")]
    [SerializeField] private float lv1Itchy = 3.5f;
    [SerializeField] private float lv2Itchy = 5.0f;
    [SerializeField] private float lv3Itchy = 8.0f;
    
    private Vector3 _lastPos;

    private bool _started = false;
    private bool _newRegion = true;
    private float _timer = 0.0f;

    [SerializeField] private Transform playerTransform;

    private bool _itchy1 = false;
    private bool _itchy2 = false;
    

    private void Update()
    {
        if (!_started)
        {
            startUpTime -= Time.deltaTime;
            if (startUpTime <= 0.0f) _started = true;
            return;
        }

        if (_newRegion)
        {
            _lastPos = playerTransform.position;
            _newRegion = false;
        }

        _timer += Time.deltaTime * sensitivity;
        CheckItchy();
        CheckHeight();
    }

    private void CheckHeight()
    {
        
    }

    private void CheckItchy()
    {
        float displacement = Vector3.Magnitude(_lastPos - playerTransform.position);
        if (displacement >= displacementRequired)
        {
            _newRegion = true;
            _timer = 0.0f;
            _itchy1 = false;
            _itchy2 = false;
        }

        if (_timer > lv3Itchy)
        {
            Debug.LogWarning("Lv3 Reached!!!");
            HingeArm hingeArm = playerTransform.GetComponent<HingeArm>();
            if (hingeArm._leftGripPoint != null)
            {
                Destroy(hingeArm._leftGripPoint);
            }
            if (hingeArm._rightGripPoint != null)
            {
                Destroy(hingeArm._rightGripPoint);
            }
            _newRegion = true;
            _timer = 0.0f;
            _itchy1 = false;
            _itchy2 = false;
        } else if (!_itchy2 && _timer > lv2Itchy)
        {
            Debug.LogWarning("Lv2 Reached!!!");
            _itchy2 = true;
        } else if (!_itchy1 && _timer > lv1Itchy)
        {
            Debug.LogWarning("Lv1 Reached!!!");
            _itchy1 = true;
        }
    }
    
}
