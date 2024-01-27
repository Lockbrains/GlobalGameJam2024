using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public RawImage image;
    public Camera myCamera;
    
    
    
    private void Awake()
    {
        instance = this;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //int ratio = 1;
        //Rect rect = new Rect(0, 0, (int)Screen.width / ratio, (int)Screen.height / ratio);
        
        //image.texture = ShowScreen(myCamera, rect);
    }
    
    public Texture2D ShowScreen(Camera camera, Rect rect)
    {
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
        camera.targetTexture = rt;
        camera.Render();

        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        camera.targetTexture = null;
        RenderTexture.active = null;

        GameObject.Destroy(rt);
        return screenShot;
    }
}
