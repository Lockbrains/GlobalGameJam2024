using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EDEffect : MonoBehaviour
{
    public Material m;
    [Range(0,1)]
    public float edgeOnly = 0;
    public Color edgeColor;
    public Color backgroundColor;
    
    
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m != null)
        {
            m.SetFloat("_EdgeOnly", edgeOnly);
            m.SetColor("_EdgeColor", edgeColor);
            m.SetColor("_BackgroundColor", backgroundColor);
            Graphics.Blit(source, destination, m);
        }
        else
        {
            Graphics.Blit(source, destination);
        }

    }
}
