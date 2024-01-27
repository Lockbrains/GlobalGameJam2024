Shader "Hidden/BWDiffuse"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BWBlend ("Black and White Blend", Range (0,1)) = 0 
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            // uniform: a global effect
            uniform sampler2D _MainTex;
            uniform float _BWBlend;

            fixed4 frag (v2f_img i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float lum = col.r * 0.3 + col.g * 0.59 + col.b * 0.11;

                float4 bw = float4(lum, lum, lum, 1); 
                
                return lerp(col, bw, _BWBlend);
            }
            ENDCG
        }
    }
}
