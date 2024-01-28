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

    [Header("Noise Profile Assets")] 
    [SerializeField] private NoiseSettings _6DShake;
    [SerializeField] private NoiseSettings _Handheld;
    [SerializeField] private float shakeDuration;
    
    private int _curPriority;
    private CinemachineVirtualCamera _curCam;
    
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


    private void TestCamera()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShakeCamera();
        }
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
        _curCam = cam;
    }

    public void StartGame()
    {
        SetPrioritizedCam(virtualCameras[1]);
    }

    public void ShakeCamera()
    {
        StartCoroutine(TriggerShake());
    }

    private IEnumerator TriggerShake()
    {
        CinemachineBasicMultiChannelPerlin
            noise = _curCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_NoiseProfile = _6DShake;
        yield return new WaitForSeconds(shakeDuration);
        noise.m_NoiseProfile = _Handheld;
    }
}
