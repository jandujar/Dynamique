Shader "ColorMask/Transparent/Cutout/Diffuse" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_ColorMaskColor ("Mask Color", Color) = (1,0,0,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_ColorMask ("ColorMask (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}

SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 200
	
CGPROGRAM
#pragma surface surf Lambert alphatest:_Cutoff

sampler2D _MainTex;
sampler2D _ColorMask;
fixed4 _Color;
float4 _ColorMaskColor;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	//o.Albedo = c.rgb;
	fixed texColorMask = tex2D(_ColorMask, IN.uv_MainTex).a;
	o.Albedo = lerp(tex.rgb * _Color.rgb , tex.rgb * _ColorMaskColor.rgb, texColorMask);
	o.Alpha = tex.a;
}
ENDCG
}

Fallback "Transparent/Cutout/VertexLit"
}
