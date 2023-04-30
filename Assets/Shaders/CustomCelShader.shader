Shader "Custom/CustomCelShader" {
    Properties {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Glossiness ("Smoothness", Range(0, 1)) = 0.5
        _Metallic ("Metallic", Range(0, 1)) = 0.0
        _RampTex ("Ramp Texture", 2D) = "white" {}
		_BumpMap ("Normal Map", 2D) = "bump" {}
    }

    SubShader {
        Tags {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        CGPROGRAM
        #pragma surface surf CustomLightModel vertex:vert noambient nolightmap nodynlightmap

        sampler2D _MainTex;
        sampler2D _RampTex;
		sampler2D _BumpMap;
        float _Glossiness;
        float _Metallic;
        float4 _Color;

        struct Input {
            float2 uv_MainTex;
			float2 uv_BumpMap;
            float3 viewDir;
        };

        void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.viewDir = WorldSpaceViewDir(v.vertex);
        }

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Specular = _Metallic;
            o.Gloss = _Glossiness;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
        }
		
		inline fixed4 LightingCustomLightModel(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed3 halfDir = lightDir;
			fixed diff = max(0, dot(s.Normal, lightDir));
			fixed spec = pow(max(0, dot(s.Normal, halfDir)), s.Specular * 128);

			fixed4 c;
			c.rgb = (tex2D(_RampTex, fixed2(diff, 0.5)) * _LightColor0.rgb) * (atten * 2);
			c.rgb += (tex2D(_RampTex, fixed2(spec, 0.5)) * _LightColor0.rgb) * (atten * s.Gloss);
			c.a = 1;
   			return c;
		}

        ENDCG
    }
    FallBack "Diffuse"
}
