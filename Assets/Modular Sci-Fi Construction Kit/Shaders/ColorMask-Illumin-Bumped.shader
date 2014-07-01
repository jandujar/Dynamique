Shader "ColorMask/Self-Illumin/Bumped Diffuse" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_ColorMaskColor ("Mask Color", Color) = (1,0,0,1)
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_Illum ("Illumin (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_ColorMask ("ColorMask (A)", 2D) = "white" {}
	_EmissionLM ("Emission (Lightmapper)", Float) = 0
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 300

CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
sampler2D _BumpMap;
sampler2D _Illum;
sampler2D _ColorMask;
fixed4 _Color;
float4 _ColorMaskColor;

struct Input {
	float2 uv_MainTex;
	float2 uv_Illum;
	float2 uv_BumpMap;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	fixed4 c = tex * _Color;
	//o.Albedo = c.rgb;
	fixed texColorMask = tex2D(_ColorMask, IN.uv_MainTex).a;
	o.Albedo = lerp(c.rgb, tex.rgb * _ColorMaskColor, texColorMask);
	o.Emission = c.rgb * tex2D(_Illum, IN.uv_Illum).a;
	o.Alpha = c.a;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
}
ENDCG
} 
FallBack "ColorMask/Self-Illumin/Diffuse"
}
