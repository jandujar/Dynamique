Shader "ColorMask/Transparent/Specular" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 0)
	_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
	_ColorMaskColor ("Mask Color", Color) = (1,0,0,1)
	_MainTex ("Base (RGB) TransGloss (A)", 2D) = "white" {}
	_ColorMask ("ColorMask (A)", 2D) = "white" {}
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 300

CGPROGRAM
#pragma surface surf BlinnPhong alpha

sampler2D _MainTex;
sampler2D _ColorMask;
fixed4 _Color;
float4 _ColorMaskColor;
half _Shininess;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	//o.Albedo = tex.rgb * _Color.rgb;
	fixed texColorMask = tex2D(_ColorMask, IN.uv_MainTex).a;
	o.Albedo = lerp(tex.rgb * _Color.rgb , tex.rgb * _ColorMaskColor, texColorMask);
	o.Gloss = tex.a;
	o.Alpha = tex.a * _Color.a;
	o.Specular = _Shininess;
}
ENDCG
}

Fallback "Transparent/VertexLit"
}
