Shader "Custom/CustomCelShaderHLSL" {
    Properties {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Glossiness ("Smoothness", Range(0, 1)) = 0.5
        _Metallic ("Metallic", Range(0, 1)) = 0.0
        _RampTex ("Ramp Texture", 2D) = "white" {}
    }

    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"

        struct appdata {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
            float3 normal : NORMAL;
        };

        struct v2f {
            float2 uv : TEXCOORD0;
            float3 viewDir : TEXCOORD1;
            float3 worldNormal : TEXCOORD2;
            float4 vertex : SV_POSITION;
        };

        sampler2D _MainTex;
        sampler2D _RampTex;
        float _Glossiness;
        float _Metallic;
        float4 _Color;

        v2f vert (appdata v) {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            o.worldNormal = UnityObjectToWorldNormal(v.normal);
            o.viewDir = normalize(UnityWorldSpaceViewDir(v.vertex));
            return o;
        }

        fixed4 frag (v2f i) : SV_Target {
            fixed4 col = tex2D(_MainTex, i.uv) * _Color;
            fixed3 normal = normalize(i.worldNormal);
            float3 lightDir = normalize(_WorldSpaceLightPos0.xyz - i.vertex.xyz);
            float NdotL = saturate(dot(normal, lightDir));
            float3 viewDir = normalize(i.viewDir);
            float3 halfDir = normalize(lightDir + viewDir);
            float spec = pow(saturate(dot(normal, halfDir)), _Glossiness * 128);

            fixed4 rampColor = tex2D(_RampTex, float2(NdotL, 0.5));
            fixed4 finalColor;
            finalColor.rgb = rampColor.rgb * _LightColor0.rgb;
            finalColor.rgb += tex2D(_RampTex, float2(spec, 0.5)).rgb * _LightColor0.rgb * _Glossiness;
            finalColor.a = col.a;

            return finalColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
