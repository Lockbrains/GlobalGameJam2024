using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BWEffect : MonoBehaviour
{
    private Material m;
    [Range(0,1)]
    public float intensity = 0;
    
    private void Awake()
    {
        m = new Material(Shader.Find("Hidden/BWDiffuse")); 
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        m.SetFloat("_BWBlend", intensity);
        Graphics.Blit(source, destination, m); 
    }
}
