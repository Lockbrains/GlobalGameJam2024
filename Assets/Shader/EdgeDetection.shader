Shader "Hidden/EdgeDetection"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EdgeOnly("Edge Only", Float) = 1.0
        _EdgeColor("Edge Color", Color) = (0, 0, 0, 1)
        _BackgroundColor("Background Color", Color) = (1,1,1,1)
        _Brightness ("Brightness", float) = 1
        _Saturation ("Saturation", float) = 1
        _Contrast ("Contrast", float) = 1
        _HueShift("Hue Shift", float) = 1
    }
    
    SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragSobel

            #include "UnityCG.cginc"
            

            struct v2f
            {
                half2 uv[9] : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            uniform half4 _MainTex_TexelSize;
			fixed _EdgeOnly;
			fixed4 _EdgeColor;
			fixed4 _BackgroundColor;
            fixed _Brightness;
            fixed _Saturation;
            fixed _Contrast;
            fixed _HueShift;

            
            v2f vert (appdata_img v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                
                half2 uv = v.texcoord;
                o.uv[0] = uv + _MainTex_TexelSize.xy * half2(-1, -1);
                o.uv[1] = uv + _MainTex_TexelSize.xy * half2(0, -1);
                o.uv[2] = uv + _MainTex_TexelSize.xy * half2(1, -1);
                o.uv[3] = uv + _MainTex_TexelSize.xy * half2(-1, 0);
                o.uv[4] = uv + _MainTex_TexelSize.xy * half2(0, 0);
                o.uv[5] = uv + _MainTex_TexelSize.xy * half2(1, 0);
                o.uv[6] = uv + _MainTex_TexelSize.xy * half2(-1, 1);
                o.uv[7] = uv + _MainTex_TexelSize.xy * half2(0, 1);
                o.uv[8] = uv + _MainTex_TexelSize.xy * half2(1, 1);
                
                return o;
            }


            fixed luminance (fixed4 color)
            {
                return 0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b;
            }

            half Sobel(v2f i)
            {
                const half Gx[9] = {-1, 0, 1, -2, 0, 2, -1, 0, 1};
                const half Gy[9] = {-1, -2, -1, 0, 0, 0, 1, 2, 1};

                half texColor;
                half edgeX = 0;
                half edgeY = 0;
                for(int it = 0; it < 9; it ++)
                {
                    texColor = luminance(tex2D(_MainTex, i.uv[it]));
                    edgeX += texColor * Gx[it];
                    edgeY += texColor * Gy[it];
                }

                half edge = 1 - abs(edgeX) - abs(edgeY);

                return edge;
            }

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
            
            fixed4 fragSobel (v2f i) : SV_Target
            {
                half edge = Sobel(i);
                fixed4 col = tex2D(_MainTex, i.uv[4]);
                
                fixed4 withEdgeColor = lerp(_EdgeColor, tex2D(_MainTex, i.uv[4]), edge);
                fixed4 onlyEdgeColor = lerp(_EdgeColor, _BackgroundColor, edge);
                fixed3 finalColor = lerp(withEdgeColor, onlyEdgeColor, _EdgeOnly).rgb;

                
                // brightness
                finalColor *= _Brightness;

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
