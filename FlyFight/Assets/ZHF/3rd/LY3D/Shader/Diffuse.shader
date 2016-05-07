Shader "Unlit/Diffuse"
{
	Properties
	{

		_Tex0("Tex0 (RGBA)", 2D) = "white" {}

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0
			
			
			#include "UnityCG.cginc"
			sampler2D _Tex0;
			float4 _Tex0_ST;

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

			
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _Tex0);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 clr0 = tex2D(_Tex0, i.uv);
	
				return clr0;
			}
			ENDCG
		}
	}
}
