using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BSCEffect : MonoBehaviour
{
    public Material m;
    [Range(0, 3)]
    public float brightness = 1;
    [Range(0, 3)]
    public float saturation = 1;
    [Range(0,3)]
    public float contrast = 1;
    
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m != null)
        {
            m.SetFloat("_Brightness", brightness);
            m.SetFloat("_Saturation", saturation);
            m.SetFloat("_Contrast", contrast);
            Graphics.Blit(source, destination, m); 
        }
        else
        {
            Graphics.Blit(source, destination);
        }
        
    }
}
