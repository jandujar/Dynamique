Shader "ColorMask/Transparent/Cutout/Bumped Diffuse" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_ColorMaskColor ("Mask Color", Color) = (1,0,0,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_ColorMask ("ColorMask (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}

SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 300
	
CGPROGRAM
#pragma surface surf Lambert alphatest:_Cutoff

sampler2D _MainTex;
sampler2D _BumpMap;
sampler2D _ColorMask;
fixed4 _Color;
float4 _ColorMaskColor;

struct Input {
	float2 uv_MainTex;
	float2 uv_BumpMap;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	fixed texColorMask = tex2D(_ColorMask, IN.uv_MainTex).a;
	o.Albedo = lerp(tex.rgb * _Color.rgb, tex.rgb * _ColorMaskColor.rgb, texColorMask);
	//o.Albedo = c.rgb;
	o.Alpha = tex.a;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
}
ENDCG
}

FallBack "ColorMask/Transparent/Cutout/Diffuse"
}
