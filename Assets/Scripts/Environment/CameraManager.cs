using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [Header("Rendering Image")]
    public RawImage image;
    public Camera myCamera;

    [Header("Virtual Cameras")] 
    [SerializeField] private CinemachineVirtualCamera[] virtualCameras;

    [SerializeField] private CinemachineVirtualCamera _initialCam;
    private int _curPriority;
    
    private void Awake()
    {
        instance = this;
        _curPriority = 1;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        if(_initialCam != null) RefreshCamPriority();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RefreshCamPriority()
    {
        foreach (var cam in virtualCameras)
        {
            cam.Priority = 0;
        }

        _initialCam.Priority = 1;
    }

    private void SetPrioritizedCam(CinemachineVirtualCamera cam)
    {
        _curPriority++;
        cam.Priority = _curPriority;
    }

    public void StartGame()
    {
        SetPrioritizedCam(virtualCameras[1]);
    }
}
