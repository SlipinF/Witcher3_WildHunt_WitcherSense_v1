Shader "Custom/Outline"
{
	Properties
	{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Texture", 2D) = "white" {}

		_FirstOutlineColor("Outline color", Color) = (1,0,0,0.5)
		_FirstOutlineWidth("Outlines width", Range(0.0, 2.0)) = 0.15

		_SecondOutlineColor("Outline color", Color) = (0,0,1,1)
		_SecondOutlineWidth("Outlines width", Range(0.0, 2.0)) = 0.025

		_Angle("Switch shader on angle", Range(0.0, 180.0)) = 89
		_Distance("Distance", float) = 0.01
		_Amplitude("Amplitude", float) = 0.01
		_Speed ("Speed", float) = 0.01
		_Amount("Amount", float) = 0.01
		_Transparency("Transparency", Range(0.0,0.5)) = 0.25
		_CutoutThresh("Cutout Threshold", Range(0.0,1.0)) = 0.2
		_Glow ("Intensity", Range(0, 3)) = 1
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata {
		float4 vertex : POSITION;
		float4 normal : NORMAL;
	};

	uniform float4 _FirstOutlineColor;
	uniform float _FirstOutlineWidth;

	uniform float4 _SecondOutlineColor;
	uniform float _SecondOutlineWidth;

	uniform sampler2D _MainTex;
	uniform float4 _Color;
	uniform float _Angle;
	float _Distance;
	float _Amplitude;
	float _Speed;
	float _Amount;
	float _Transparency;
	float _CutoutThresh;
	half _Glow;

	ENDCG

	Subshader
		{
        Pass
        {
			Tags{"Queue" = "Geometry +1"}
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			//Cull Front

            CGPROGRAM

			struct v2f {
				float4 pos : SV_POSITION;
			};
            #pragma target 3.0           
			#pragma vertex vert
			#pragma fragment frag
           
            struct SHADERDATA
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

			float _octaves,_lacunarity,_gain,_value,_amplitude,_frequency, _offsetX, _offsetY, _power, _scale, _monochromatic, _range;
            float4 _color;

				v2f vert(appdata v){
				appdata original = v;

				v.vertex.x += cos(_Time.y * _Speed + v.vertex.y * _Amplitude) * _Distance * _Amount;
				v.vertex.y += cos(_Time.y * _Speed + v.vertex.x * _Amplitude) * _Distance * _Amount;
				v.vertex.z += cos(_Time.y * _Speed + v.vertex.x * _Amplitude) * _Distance * _Amount;

				float3 scaleDir = normalize(v.vertex.xyz - float4(0,0,0,1));
				if (degrees(acos(dot(scaleDir.xyz, v.normal.xyz))) > _Angle) {
					v.vertex.xyz += normalize(v.normal.xyz) * _FirstOutlineWidth;
				}else {
					v.vertex.xyz += scaleDir * _FirstOutlineWidth;
				}

				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
				}

			half4 frag(v2f i) : COLOR{
				float4 color = _FirstOutlineColor;
				color.a = _Transparency;
				color *= _Glow;
				clip(color.r - _CutoutThresh);
				return color;
			}
            ENDCG
}
		//Surface shader
		Tags{ "RenderType"="Transparecy"}

		CGPROGRAM
		#pragma surface surf Lambert noshadow

		struct Input {
			float2 uv_MainTex;
			float4 color : COLOR;
		};

		void surf(Input IN, inout SurfaceOutput  o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}
