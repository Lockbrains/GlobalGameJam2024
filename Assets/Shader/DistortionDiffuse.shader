Shader "Hidden/DistortionDiffuse"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DisplaceTex("Displacement Map", 2D) = "bump" {}
        _Strength("Strength", float) = 1
        
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            sampler2D _DisplaceTex;
            float _Strength;

            fixed4 frag (v2f_img i) : SV_Target
            {
                half2 n = tex2D(_DisplaceTex, i.uv);
                half2 d = n * 2 - 1; // transform from color to unit vector
                i.uv += d * _Strength;
                // saturate: keep value in between (0,1)
                i.uv = saturate(i.uv);

                float4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
