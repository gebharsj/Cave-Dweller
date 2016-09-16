Shader "Custom/BSC_Effect"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_BrightnessAmount("Brightness Amount", Range(0.0, 1)) = 1.0
		_SaturationAmount("Saturation Amount", Range(0.0, 1)) = 1.0
		_ContrastAmount("Contrast Amount", Range(0.0, 1)) = 1.0
	}
		SubShader
		{
			// No culling or depth
			Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			fixed _BrightnessAmount;
			fixed _SaturationAmount;
			fixed _ContrastAmount;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			float3 ContrastSaturationBrightness(float3 color, float brt, float sat, float con)
			{
				//Increase or decrease these values to adjust r, g, and b color channels separately
				float AvgLumR = 0.5;
				float AvgLumG = 0.5;
				float AvgLumB = 0.5;

				//Luminance coefficients for getting luminance from the image
				float3 LuminanceCoeff = float3(0.2125, 0.7154, 0.0721);

				//Operation for brightness
				float3 AvgLumin = float3(AvgLumR, AvgLumG, AvgLumB);
				float3 brtColor = color * brt;
				float intensityf = dot(brtColor, LuminanceCoeff);
				float3 intensity = float3(intensityf, intensityf, intensityf);

				//Operation for saturation
				float3 satColor = lerp(intensity, brtColor, sat);

				//Operation for contrast
				float3 conColor = lerp(AvgLumin, satColor, con);
				return conColor;
			}

			fixed4 frag(v2f i) : COLOR
			{
				//Get the colors from the RenderTexture and the uv's
				//from the v2f struct
				fixed4 renderTex = tex2D(_MainTex, i.uv);

				//Apply brightness, saturation, contrast operations
				renderTex.rgb = ContrastSaturationBrightness(renderTex.rgb, _BrightnessAmount, _SaturationAmount, _ContrastAmount);

				return renderTex;
			}
			ENDCG
		}
	}
}