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
    [SerializeField] private float defaultSensitivity = 1.0f;
    [SerializeField] private float sensitivity;
    [SerializeField] private float startUpTime = 6.0f;
    [SerializeField] private float lv1Height = 80.0f;
    [SerializeField] private float lv2Height = 170.0f;
    [SerializeField] private float lv3Height = 260.0f;
    [SerializeField] private float sensitivityInc = 1.2f;

    [Header("lv Config")]
    [SerializeField] private float lv1Itchy = 10f;
    [SerializeField] private float lv2Itchy = 20f;
    [SerializeField] private float lv3Itchy = 30f;
    
    private Vector3 _lastPos;

    private bool _started = false;
    private bool _newRegion = true;
    private float _timer = 0.0f;

    [SerializeField] private Transform playerTransform;

    private bool _itchy1 = false;
    private bool _itchy2 = false;

    private float initialHeight;

    private void Start()
    {
        initialHeight = playerTransform.position.z;
        sensitivity = defaultSensitivity;
    }
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

        float heightReached = playerTransform.position.z - initialHeight;
        //Debug.Log(heightReached);
        
        if (heightReached < lv1Height)
        {
            //Debug.LogWarning("Lv0 Height, default sensitivity!");
            sensitivity = defaultSensitivity;
        }
        else if (heightReached < lv2Height)
        {
            //Debug.LogWarning("Lv1 Height, sensitivity Inc 1x!");
            sensitivity = defaultSensitivity + 1 * sensitivityInc;
        }
        else if (heightReached < lv3Height)
        {
            //Debug.LogWarning("Lv2 Height, sensitivity Inc 2x!");
            sensitivity = defaultSensitivity + 2 * sensitivityInc;
        }
        else //heightReached >= lv3Height)
        {
            //Debug.LogWarning("Lv3 Height, sensitivity Inc 3x!");
            sensitivity = defaultSensitivity + 3 * sensitivityInc;
        }

    }

    private void CheckItchy()
    {
        // Jesse: i change the playerTransform from FishmanSeperatev02 to fishmanBody_geo, for somehow, the former one does not change in Transform
        float displacement = Vector3.Magnitude(_lastPos - playerTransform.position);
        if (displacement >= displacementRequired)
        {
            _newRegion = true;
            _timer = 0.0f;
            _itchy1 = false;
            _itchy2 = false;
        }
        Debug.Log($"timer: {_timer}, dis: {displacement}");

        if (_timer > lv3Itchy)
        {
            Debug.LogWarning("Lv3 Reached!!!");
            HingeArm hingeArm = playerTransform.parent.GetComponent<HingeArm>();
            if (hingeArm._leftGripPoint != null)
            {
                Destroy(hingeArm._leftGripPoint);
            }
            if (hingeArm._rightGripPoint != null)
            {
                Destroy(hingeArm._rightGripPoint);
            }
            hingeArm.Paralyze();
            _newRegion = true;
            _timer = 0.0f;
            _itchy1 = false;
            _itchy2 = false;
            CameraManager.instance.ShakeCamera();
            SoundManager.instance.MakeLaughSnd(2);
        } else if (!_itchy2 && _timer > lv2Itchy)
        {
            Debug.LogWarning("Lv2 Reached!!!");
            _itchy2 = true;
            CameraManager.instance.ShakeCamera();
            SoundManager.instance.MakeLaughSnd(0);
        } else if (!_itchy1 && _timer > lv1Itchy)
        {
            Debug.LogWarning("Lv1 Reached!!!");
            _itchy1 = true;
            SoundManager.instance.StopSnore();
        }
    }
    
}
