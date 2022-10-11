// Toony Colors Free
// (c) 2012,2016 Jean Moreno


// Want more features ? Check out Toony Colors Pro+Mobile 2 !
// http://www.jeanmoreno.com/toonycolorspro/


Shader "Custom/WorldCoordUVToon"
{
	Properties
	{
		//TOONY COLORS
		_Color ("Color", Color) = (0.5,0.5,0.5,1.0)
		_HColor ("Highlight Color", Color) = (0.6,0.6,0.6,1.0)
		_SColor ("Shadow Color", Color) = (0.3,0.3,0.3,1.0)
		
		//DIFFUSE
		[NoScaleOffset] _MainTex ("Main Texture (RGB)", 2D) = "white" {}
		_TexScale ("Texture Scale", float) = 1.0
		
		//TOONY COLORS RAMP
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		
		//RIM LIGHT
		_RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.6)
		_RimMin ("Rim Min", Range(0,1)) = 0.5
		_RimMax ("Rim Max", Range(0,1)) = 1.0
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		
		#pragma surface surf ToonyColorsCustom
		#pragma target 2.0
		#pragma glsl
		
		
		//================================================================
		// VARIABLES
		
		fixed4 _Color;
		sampler2D _MainTex;
		fixed _TexScale;
		fixed4 _MainTex_ST;
		
		fixed4 _RimColor;
		fixed _RimMin;
		fixed _RimMax;
		float4 _RimDir;
		
		struct Input
		{
			half2 uv_MainTex;
			float3 viewDir;
			float3 worldNormal;
			float3 worldPos;
		};
		
		//================================================================
		// CUSTOM LIGHTING
		
		//Lighting-related variables
		fixed4 _HColor;
		fixed4 _SColor;
		sampler2D _Ramp;
		
		//Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Alpha;
		};
		
		inline half4 LightingToonyColorsCustom (SurfaceOutputCustom s, half3 lightDir, half3 viewDir, half atten)
		{
			s.Normal = normalize(s.Normal);
			fixed ndl = max(0, dot(s.Normal, lightDir)*0.5 + 0.5);
			
			fixed3 ramp = tex2D(_Ramp, fixed2(ndl,ndl));
		#if !(POINT) && !(SPOT)
			ramp *= atten;
		#endif
			_SColor = lerp(_HColor, _SColor, _SColor.a);	//Shadows intensity through alpha
			ramp = lerp(_SColor.rgb,_HColor.rgb,ramp);
			fixed4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp;
			c.a = s.Alpha;
		#if (POINT || SPOT)
			c.rgb *= atten;
		#endif
			return c;
		}
		
		
		//================================================================
		// SURFACE FUNCTION
		
		void surf (Input IN, inout SurfaceOutputCustom o)
		{
			float3 uvs = IN.worldPos.xyz * _TexScale;
			float3 blending = saturate(abs(IN.worldNormal.xyz) - 0.2); // Change the 0.2 value to adjust blending
			blending = pow(blending, 2.0); // Change the 2.0 value to adjust blending
			blending /= dot(blending, float3(1.0, 1.0, 1.0));
			float4 c = blending.x * tex2D(_MainTex, uvs.yz);
			c = blending.y * tex2D(_MainTex, uvs.xz) + c; // Single MAD
			c = blending.z * tex2D(_MainTex, uvs.xy) + c; 
			
			// fixed4 mainTex = tex2D(_MainTex, UV * _MainTex_ST.xy + _MainTex_ST.zw);

			o.Albedo = c.rgb * _Color.rgb;
			o.Alpha = c.a * _Color.a;
			
			//Rim
			float3 viewDir = normalize(IN.viewDir);
			half rim = 1.0f - saturate( dot(viewDir, o.Normal) );
			rim = smoothstep(_RimMin, _RimMax, rim);
			o.Emission += (_RimColor.rgb * rim) * _RimColor.a;
		}
		ENDCG
	}
	
	Fallback "Diffuse"
	CustomEditor "TCF_MaterialInspector"
}
