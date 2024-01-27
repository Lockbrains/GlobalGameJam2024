Shader "Hidden/BSCDiffuse"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Brightness ("Brightness", float) = 1
        _Saturation ("Saturation", float) = 1
        _Contrast ("Contrast", float) = 1
        _HueShift("Hue Shift", float) = 1
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
            fixed _Brightness;
            fixed _Saturation;
            fixed _Contrast;
            fixed _HueShift;

            

            float3 RGBtoHSV(float3 rgb)
            {
                float Cmax = max(rgb.r, max(rgb.g, rgb.b));
                float Cmin = min(rgb.r, min(rgb.g, rgb.b));
                float delta = Cmax - Cmin;

                float hue = 0.0;
                if (delta != 0)
                {
                    if (Cmax == rgb.r)
                    {
                        hue = fmod((rgb.g - rgb.b) / delta, 6.0);
                    }
                    else if (Cmax == rgb.g)
                    {
                        hue = (rgb.b - rgb.r) / delta + 2.0;
                    }
                    else
                    {
                        hue = (rgb.r - rgb.g) / delta + 4.0;
                    }
                    hue /= 6.0;
                }

                float saturation = Cmax == 0 ? 0 : delta / Cmax;
                float value = Cmax;

                return float3(hue, saturation, value);
            }

            float3 HSVtoRGB(float3 hsv)
            {
                float C = hsv.z * hsv.y;
                float X = C * (1 - abs(fmod(hsv.x * 6, 2) - 1));
                float m = hsv.z - C;

                float3 rgb;

                if (0 <= hsv.x && hsv.x < 1.0/6.0)
                    rgb = float3(C, X, 0);
                else if (1.0/6.0 <= hsv.x && hsv.x < 2.0/6.0)
                    rgb = float3(X, C, 0);
                else if (2.0/6.0 <= hsv.x && hsv.x < 3.0/6.0)
                    rgb = float3(0, C, X);
                else if (3.0/6.0 <= hsv.x && hsv.x < 4.0/6.0)
                    rgb = float3(0, X, C);
                else if (4.0/6.0 <= hsv.x && hsv.x < 5.0/6.0)
                    rgb = float3(X, 0, C);
                else
                    rgb = float3(C, 0, X);

                rgb += m;
                return rgb;
            }

            fixed4 frag (v2f_img i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // brightness
                fixed3 finalColor = col.rgb * _Brightness;

                // saturation
                fixed lum = 0.2125 * col.r + 0.7154 * col.g + 0.0721 * col.b;
                fixed3 lumColor = fixed3(lum, lum, lum);
                finalColor = lerp(lumColor, finalColor, _Saturation);

                // contrast
                fixed3 avgColor = fixed3(0.5, 0.5, 0.5);
                finalColor = lerp(avgColor, finalColor, _Contrast);

                fixed3 hsv = RGBtoHSV(finalColor);
                hsv.x += _HueShift;
                hsv.x = frac(hsv.x);

                finalColor.rgb = HSVtoRGB(hsv);
                
                return fixed4(finalColor, col.a);
            }
            
            ENDCG
        }
    }
}
