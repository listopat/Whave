﻿Shader "Custom/WaveShader"
{
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader {
		Pass {
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

			float2 dist;

			float4 frag(Interpolators i) : SV_TARGET{
				clip(i.uv[0] - 0.5);
				float dist = distance(i.uv, float2(0.5, 0.5));
				clip(dist - 0.3);
				return step(0.25, dist) * float4(0.5, 0.5, step(0.4, dist), 1);
			}

			ENDCG
		}
	}
}
