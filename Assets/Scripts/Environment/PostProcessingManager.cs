using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//[ExecuteInEditMode]
public class PostProcessingManager : MonoBehaviour
{
    //public Shader postProcessShader;
    private CommandBuffer buffer;
    public Material postProcessMaterial;

    void Start()
    {
        //postProcessMaterial = new Material(postProcessShader);
        buffer = new CommandBuffer();
        buffer.name = "Custom Post Processing";
        
        Camera cam = GetComponent<Camera>();
        cam.AddCommandBuffer(CameraEvent.AfterImageEffects, buffer);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        buffer.Clear();
        buffer.SetRenderTarget(dest);
        buffer.Blit(src, dest, postProcessMaterial);
    }
}
