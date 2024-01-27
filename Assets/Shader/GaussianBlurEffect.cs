using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GaussianBlurEffect : MonoBehaviour
{
    public Material m;

    [Range(0, 4)] public int iterations = 3; // Blur Times - More blurry when iteration gets higher
    [Range(0.2f, 3.0f)] public float blurSpread = 0.6f; 
    [Range(1, 8)] public int downSample = 2; // Improve the performance

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m != null)
        {
            int rtW = source.width / downSample;
            int rtH = source.height / downSample;
            
            // temporary buffer
            RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
            buffer0.filterMode = FilterMode.Bilinear; // Set an anti-aliasing effect 
            
            // vertical blur
            Graphics.Blit(source, buffer0);
            buffer0.filterMode = FilterMode.Point;
            
            // horizontal blur
            RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
            for (int i = 0; i < iterations; i++)
            {
                m.SetFloat("_BlurSize", 1.0f + i * blurSpread);
                Graphics.Blit(buffer0, buffer1, m, 0);
                Graphics.Blit(buffer1, buffer0, m, 1);

            }
            
            Graphics.Blit(buffer0, destination);
            
            RenderTexture.ReleaseTemporary(buffer1);
            RenderTexture.ReleaseTemporary(buffer0);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
