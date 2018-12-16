Shader "Custom/BallShader"
{
	Properties{
		_MainTex("Texture", 2D) = "white" {}
	}

		SubShader{

			Tags { "RenderType" = "Transparent" "IgnoreProjector" = "True" "Queue" = "Transparent" }

			Pass {
				ZWrite Off
				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"


				sampler2D _MainTex;

				struct Interpolators {
					float4 position : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				struct VertexData {
					float4 position : POSITION;
					float2 uv : TEXCOORD0;
				};

				Interpolators vert(VertexData v) {
					Interpolators i;
					i.position = UnityObjectToClipPos(v.position);
					i.uv = v.uv;
					return i;
				}


				float4 frag(Interpolators i) : SV_TARGET {
					float dist = distance(i.uv, float2(0.5, 0.5));

					float3 color = float3(0.25, 0.25, 0.25);
					float transparency = 1 - smoothstep(0.45, 0.5, dist);

					return float4(color, transparency);
				}

				ENDCG
			}
	}
}
