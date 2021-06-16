Shader "Custom/DefaultURPShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ToonLightRange("Toon Light Range", Range(0, 1)) = 0.7
        _ToonLightForce("Toon Light Force", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags 
        {
             "RenderType"="Opaque" 
             "RenderPipline" = "UniversalPipline"
             "Queue" = "Geometry"
        }
        // outline
        // Pass
        // {           
        //     cull front

        //     HLSLPROGRAM
        //     #pragma vertex vert
        //     #pragma fragment frag

        //      #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

        //      struct appdata
        //     {
        //         float4 vertex : POSITION;
        //         float3 normal : NORMAL;
        //     };

        //     struct v2f
        //     {
        //         float4 vertex : SV_POSITION;
        //         float3 normal : NORMAL;
        //     };
        //      v2f vert (appdata v)
        //     {
        //         v2f o;
        //         v.vertex.xyz += v.normal.xyz * 0.0003;

        //         o.vertex = TransformObjectToHClip(v.vertex.xyz);
        //         o.normal = TransformObjectToWorldNormal(v.normal);
        //         return o;
        //     }

        //     half4 frag (v2f i) : SV_Target
        //     {
        //         // sample the texture
        //         half4 col = half4(0, 0, 0, 1);
        //         return col;
        //     }
        //     ENDHLSL
        // }
        Pass
        {
            cull back
            
            Name "Universal Forward"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag
           
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float _ToonLightRange;
            float _ToonLightForce;

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = TransformObjectToWorldNormal(v.normal);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // sample the texture
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                float3 light = _MainLightPosition.xyz;
                float3 lightColor = _MainLightColor.rgb;
                float3 ndotl = saturate(dot(light, i.normal) * 0.5) + 0.5;
                float3 toonLight = ndotl < _ToonLightRange ? _ToonLightForce : ndotl;

                col.rgb *= toonLight* lightColor;
                return col;
            }
            ENDHLSL
        }
    }
}
