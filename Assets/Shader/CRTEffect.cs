using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CRTEffect : MonoBehaviour
{
    public Material m;
    [Range(0,1)]
    public float intensity = 0;
    
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m != null)
        {
            m.SetFloat("_MaskBlend", intensity);
            Graphics.Blit(source, destination, m); 
        }
        else
        {
            Graphics.Blit(source, destination); 
        }
        
    }
}
