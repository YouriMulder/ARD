// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CustomShaders/FlagShader" {
	Properties {
		_MainTex("Texture", 2D) = "white" {}
		_Color("Overall Diffuse Color Filter", Color) = (1,1,1,1)
		_Speed("Speed", Range(0,20)) = 0
		_Frequency("Frequency", Range(0,3)) = 0
		_Amplitude("Amplitude", Range(0,0.5)) = 0
	}
		
	SubShader {
		Tags { "RenderType" = "Opaque" }
		Cull Back

		Pass {
			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
					
			#include "UnityCG.cginc"

			uniform float4 _LightColor0;

			struct AppData {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float4 color : COLOR0;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			uniform float4 _Color;

			float _Speed;
			float _Frequency;
			float _Amplitude;

			v2f vert(AppData input) {
				v2f output;
								
				float3 newNormal = input.normal * -1;
				// animation
				output.vertex = UnityObjectToClipPos(input.vertex);
				float worldPos = mul(unity_ObjectToWorld, input.vertex).xyz;
						
				float y_movement = sin(
					(worldPos.x + _Time.y * _Speed) * _Frequency
				) * _Amplitude * (input.vertex.x - 5);
				output.vertex.y += y_movement;

				// light
				float4x4 modelMatrixInverse = unity_WorldToObject;
				float3 normalDirection = UnityObjectToWorldNormal(newNormal);
				float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);

				float3 diffuseReflection = _LightColor0.rgb * _Color.rgb
					* max(0.0, dot(normalDirection, lightDirection));

				output.color = float4(diffuseReflection, 1.0);

				output.uv = TRANSFORM_TEX(input.uv, _MainTex);

				return output;
			}

			fixed4 frag(v2f input) : SV_Target {
				return tex2D(_MainTex, input.uv) * input.color;
			}

		ENDCG
		}
	}
	Fallback "Diffuse"
}