using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BloomEffect : MonoBehaviour
{
    public Material m;
    
    [Range(0, 4)] public int iterations = 3;
    
    [Range(0.2f, 3.0f)] public float blurSpread = 0.6f;

    [Range(1, 8)] public int downSample = 2;

    [Range(0.0f, 4.0f)] public float luminanceThreshold = 0.6f;
    
    void OnRenderImage (RenderTexture src, RenderTexture dest) {
        if (m != null) {
            m.SetFloat("_LuminanceThreshold", luminanceThreshold);

            int rtW = src.width/downSample;
            int rtH = src.height/downSample;
			
            RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
            buffer0.filterMode = FilterMode.Bilinear;
			
            Graphics.Blit(src, buffer0, m, 0);
            buffer0.filterMode = FilterMode.Point;
				
            RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0); // 

            for (int i = 0; i < iterations; i++) {
                m.SetFloat("_BlurSize", 1.0f + i * blurSpread);
                Graphics.Blit(buffer0, buffer1, m, 1);
                Graphics.Blit(buffer1, buffer0, m, 2);
            }

            m.SetTexture ("_Bloom", buffer0);  
            Graphics.Blit (src, dest, m, 3);  

            RenderTexture.ReleaseTemporary(buffer1);
            RenderTexture.ReleaseTemporary(buffer0);
        } else {
            Graphics.Blit(src, dest);
        }
    }
}
