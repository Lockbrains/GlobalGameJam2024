using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DistortionEffect : MonoBehaviour
{
    public Material m;
    
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m != null)
        {
            Graphics.Blit(source, destination, m); 
        }
        else
        {
            Graphics.Blit(source, destination); 
        }
        
    }
}
