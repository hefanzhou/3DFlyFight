Shader "Custom/LY3DC" {
	Properties {
		_Tex0("Tex0 (RGBA)", 2D) = "white" {}
		_Tex1("Tex1 (RGBA)", 2D) = "white" {}
		_Tex2("Tex2 (RGBA)", 2D) = "white" {}
		_Tex3("Tex3 (RGBA)", 2D) = "white" {}
		_Tex4("Tex4 (RGBA)", 2D) = "white" {}
		_Tex5("Tex5 (RGBA)", 2D) = "white" {}
		_Tex6("Tex6 (RGBA)", 2D) = "white" {}
		_Tex7("Tex7 (RGBA)", 2D) = "white" {}
		_Width("Width",int) = 1920
		_Height("Height",int) = 1080
		_Xoff("Xoff",float) = 0.0
		_Pitch("Pitch",float) = 8.0
		_Cot("Cot",float) = -0.333333
	}
	SubShader {
		Tags { "RenderType"="Opaque"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert
		#pragma target 5.0
		
		
		sampler2D _Tex0;
		sampler2D _Tex1;
		sampler2D _Tex2;
		sampler2D _Tex3;
		sampler2D _Tex4;
		sampler2D _Tex5;
		sampler2D _Tex6;
		sampler2D _Tex7;
		int _Width;
		int _Height;
		float _Xoff;
		float _Pitch;
		float _Cot;

		struct Input {
			float2 uv_Tex0;
		};
		
		float fmod (float x, float y)
		{
				float r;
				//double q = x / y;
				int quotient = (int)(x / y);
				if (quotient < 0)
						r = (x - quotient * y) + y;
				else if (quotient == 0) {
						if (x < 0)
								r = (x - quotient * y) + y;//负数求余？
						else
								r = x - quotient * y;
				} else
						r = x - quotient * y;
				return r;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			float4 clr0 = tex2D (_Tex0, IN.uv_Tex0);
			float4 clr1 = tex2D (_Tex1, IN.uv_Tex0);
			float4 clr2 = tex2D (_Tex2, IN.uv_Tex0);
			float4 clr3 = tex2D (_Tex3, IN.uv_Tex0);
			float4 clr4 = tex2D (_Tex4, IN.uv_Tex0);
			float4 clr5 = tex2D (_Tex5, IN.uv_Tex0);
			float4 clr6 = tex2D (_Tex6, IN.uv_Tex0);
			float4 clr7 = tex2D (_Tex7, IN.uv_Tex0);
			
			float fB1 = 0.0;
			float fB2 = 0.0;
			float fR1 = 0.0;
			float fR2 = 0.0;
			float fG1 = 0.0;
			float fG2 = 0.0;
    		
    		int iBIdx=0;
    		int iGIdx=0;
    		int iRIdx=0;
    		float fBRatio=0;
    		float fGRatio=0;
    		float fRRatio=0;
    		float N;
    		int xt = (int)(IN.uv_Tex0.x *_Width) * 3+2;
    		int y = (int)((1-IN.uv_Tex0.y) * _Height);
    		N = (fmod((xt - _Xoff - 3 * y * _Cot) , _Pitch))/_Pitch*8;
    		iBIdx=(int)N;
    		//fBRatio=0;
    		fBRatio=N-iBIdx;
    		
    		xt--;
    		N = (fmod((xt - _Xoff - 3 * y * _Cot) , _Pitch))/_Pitch*8;
    		iGIdx=(int)N;
    		//fGRatio=0;
    		fGRatio=N-iGIdx;
    		
    		xt--;
    		N = (fmod((xt - _Xoff - 3 * y * _Cot) , _Pitch))/_Pitch*8;
    		iRIdx=(int)N;
    		//fRRatio=0;
    		fRRatio=N-iRIdx;
			
			////////////////////////////////////////////////////////////////////
			////B
			//判断应该采样哪两个
			if(iBIdx==0)
			{
				fB1 = clr1.b;
				fB2 = clr0.b;
			}
			else if(iBIdx==1)
			{
				fB1 = clr2.b;
				fB2 = clr1.b;
			}
			else if( iBIdx==2)
			{
				fB1 = clr3.b;
				fB2 = clr2.b;
			}
			else if( iBIdx==3)
			{
				fB1 = clr4.b;
				fB2 = clr3.b;
			}
			else if( iBIdx==4)
			{
				fB1 = clr5.b;
				fB2 = clr4.b;
			}
			else if( iBIdx==5)
			{
				fB1 = clr6.b;
				fB2 = clr5.b;
			}
			else if( iBIdx==6)
			{
				fB1 = clr7.b;
				fB2 = clr6.b;
			}
			else if( iBIdx==7)
			{
				fB1 = clr0.b;
				fB2 = clr7.b;
			}
			
			////////////////////////////////////////////////////////////////////
			////G
			//判断应该采样哪两个
			if(iGIdx==0)
			{
				fG1 = clr1.g;
				fG2 = clr0.g;
			}
			else if(iGIdx==1)
			{
				fG1 = clr2.g;
				fG2 = clr1.g;
			}
			else if(iGIdx==2)
			{
				fG1 = clr3.g;
				fG2 = clr2.g;
			}
			else if(iGIdx==3)
			{
				fG1 = clr4.g;
				fG2 = clr3.g;
			}
			else if(iGIdx==4)
			{
				fG1 = clr5.g;
				fG2 = clr4.g;
			}
			else if(iGIdx==5)
			{
				fG1 = clr6.g;
				fG2 = clr5.g;
			}
			else if(iGIdx==6)
			{
				fG1 = clr7.g;
				fG2 = clr6.g;
			}
			else if(iGIdx==7)
			{
				fG1 = clr0.g;
				fG2 = clr7.g;
			}
  
			////////////////////////////////////////////////////////////////////
			////R
			//判断应该采样哪两个
			if(iRIdx==0)
			{
				fR1 = clr1.r;
				fR2 = clr0.r;
			}
			else if(iRIdx==1)
			{
				fR1 = clr2.r;
				fR2 = clr1.r;
			}
			else if(iRIdx==2)
			{
				fR1 = clr3.r;
				fR2 = clr2.r;
			}
			else if(iRIdx==3)
			{
				fR1 = clr4.r;
				fR2 = clr3.r;
			}
			else if(iRIdx==4)
			{
				fR1 = clr5.r;
				fR2 = clr4.r;
			}    
			else if(iRIdx==5)
			{
				fR1 = clr6.r;
				fR2 = clr5.r;
			}
			else if(iRIdx==6)
			{
				fR1 = clr7.r;
				fR2 = clr6.r;
			}
			else if(iRIdx==7)
			{
				fR1 = clr0.r;
				fR2 = clr7.r;
			}
			
			//o.Albedo = clrIdx;  

			
			
			o.Albedo.b = (fB2*(1-fBRatio) + fB1*fBRatio);  
			o.Albedo.g = (fG2*(1-fGRatio) + fG1*fGRatio);
			o.Albedo.r = (fR2*(1-fRRatio) + fR1*fRRatio);
			
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
