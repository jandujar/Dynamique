Shader "Custom/StaticShader" {
    Properties {
    	//Property specifications for 1st pass
        _NoiseTex("Noise (RGBA)", 2D) = "white" {}
        _Color("Tint Color", COLOR) = (0.5,0.5,0.5,0.5)
        _Noise("Noise", FLOAT) = 0.2
        _Speed("Speed", FLOAT) = 0.2
        _FallOff("FallOff", FLOAT) = 2
        _Lines("Lines", FLOAT) = 0.2
        _Width("Width", FLOAT) = 0.2
        
        //Property specifications for 2nd pass
        _NoiseTex2("Noise2 (RGBA)", 2D) = "white" {}
        _Color2("Tint Color2", COLOR) = (0.5,0.5,0.5,0.5)
        _Noise2("Noise2", FLOAT) = 0.2
        _Speed2("Speed2", FLOAT) = 0.2
        _FallOff2("FallOff2", FLOAT) = 2
        _Lines2("Lines2", FLOAT) = 0.2
        _Width2("Width2", FLOAT) = 0.2
        
        //Property specifications for 3rd pass
        _NoiseTex3("Noise3 (RGBA)", 2D) = "white" {}
        _Color3("Tint Color3", COLOR) = (0.5,0.5,0.5,0.5)
        _Noise3("Noise3", FLOAT) = 0.2
        _Speed3("Speed3", FLOAT) = 0.2
        _FallOff3("FallOff3", FLOAT) = 2 
        _Lines3("Lines3", FLOAT) = 0.2 
        _Width3("Width3", FLOAT) = 0.2
    }

    SubShader { 
        Pass{
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _NoiseTex_ST;
            sampler2D _NoiseTex;
            float _Speed;
            half4 _Color;
            float _FallOff;
            float _Lines;
            float _Noise;
            float _Width;  

            struct data{
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD;
                float3 normal : NORMAL;
            };

            struct v2f{
                float4 position : POSITION;
                float2 uv : TEXCOORD;
                float viewAngle : TEXCOORD1;
                float ypos : TEXCOORD2;
            };

            v2f vert(data v){
                v2f o;
                o.position = mul(UNITY_MATRIX_MVP, v.vertex + float4(v.normal, 0) * _Width);
                o.uv = TRANSFORM_TEX(v.texcoord, _NoiseTex);
                o.viewAngle = 1-abs(dot(v.normal, normalize(ObjSpaceViewDir(v.vertex))));
                o.ypos = o.position.y;
                return o;
            }

            half4 frag(v2f i) : COLOR{
                float2 uvOffset1 = _Time.xy*_Noise;
                float2 uvOffset2 = -_Time.xx*_Noise;
                half4 noise1 = tex2D(_NoiseTex, i.uv + uvOffset1);
                half4 noise2 = tex2D(_NoiseTex, i.uv + uvOffset2);
                float noise = (dot(noise1, noise2) - 1) * _Noise;
                half4 col = sin((i.ypos*_Lines + _Time.x*_Speed + noise)*100);   
                noise1 = tex2D(_NoiseTex, i.uv*6 + uvOffset1);
                noise2 = tex2D(_NoiseTex, i.uv*6 + uvOffset2);
                col.a *= saturate(1.3-(noise1.g+noise2.g)) * pow(1-i.viewAngle,_FallOff) * 15;
                return col * _Color * 2;
            }
            ENDCG
        }
        Pass{
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _NoiseTex2_ST;
            sampler2D _NoiseTex2;
            float _Speed2;
            half4 _Color2;
            float _FallOff2;
            float _Lines2;
            float _Noise2;
            float _Width2;  

            struct data{
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD;
                float3 normal : NORMAL;
            };

            struct v2f{
                float4 position : POSITION;
                float2 uv : TEXCOORD;
                float viewAngle : TEXCOORD1;
                float ypos : TEXCOORD2;
            };

            v2f vert(data v){
                v2f o;
                o.position = mul(UNITY_MATRIX_MVP, v.vertex + float4(v.normal, 0) * _Width2);
                o.uv = TRANSFORM_TEX(v.texcoord, _NoiseTex2);
                o.viewAngle = 1-abs(dot(v.normal,normalize(ObjSpaceViewDir(v.vertex))));
                o.ypos = o.position.y;
                return o;
            }

            half4 frag(v2f i) : COLOR{
                float2 uvOffset1 = _Time.xy*_Noise2;
                float2 uvOffset2 = -_Time.xx*_Noise2;
                half4 noise1 = tex2D(_NoiseTex2, i.uv + uvOffset1);
                half4 noise2 = tex2D(_NoiseTex2, i.uv + uvOffset2);
                float noise = (dot(noise1, noise2) - 1) * _Noise2;
                half4 col = sin((i.ypos*_Lines2 + _Time.x*_Speed2 + noise)*100);   
                noise1 = tex2D(_NoiseTex2, i.uv*6 + uvOffset1);
                noise2 = tex2D(_NoiseTex2, i.uv*6 + uvOffset2);
                col.a *= saturate(1.3-(noise1.g+noise2.g)) * pow(1-i.viewAngle,_FallOff2) * 15;
                return col * _Color2 * 2;
            }
            ENDCG
        }
        Pass{
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _NoiseTex3_ST;
            sampler2D _NoiseTex3;
            float _Speed3;
            half4 _Color3;
            float _FallOff3;
            float _Lines3;
            float _Noise3;
            float _Width3;  

            struct data{
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD;
                float3 normal : NORMAL;
            };

            struct v2f{
                float4 position : POSITION;
                float2 uv : TEXCOORD;
                float viewAngle : TEXCOORD1;
                float ypos : TEXCOORD2;
            };

            v2f vert(data v){
                v2f o;
                o.position = mul(UNITY_MATRIX_MVP, v.vertex + float4(v.normal, 0) * _Width3);
                o.uv = TRANSFORM_TEX(v.texcoord, _NoiseTex3);
                o.viewAngle = abs(dot(v.normal,normalize(ObjSpaceViewDir(v.vertex))));
                o.ypos = o.position.y;
                return o;
            }

            half4 frag(v2f i) : COLOR{
                float2 uvOffset1 = _Time.xy*_Noise3;
                float2 uvOffset2 = -_Time.xx*_Noise3;
                half4 noise1 = tex2D(_NoiseTex3, i.uv + uvOffset1);
                half4 noise2 = tex2D(_NoiseTex3, i.uv + uvOffset2);
                float noise = (dot(noise1, noise2) - 1) * _Noise3;
                half4 col = sin((i.ypos*_Lines3 + _Time.x*_Speed3 + noise)*100);   
                noise1 = tex2D(_NoiseTex3, i.uv*6 + uvOffset1);
                noise2 = tex2D(_NoiseTex3, i.uv*6 + uvOffset2);
                col.a *= saturate(1.3-(noise1.g+noise2.g)) * pow(1-i.viewAngle,_FallOff3) * 15;
                return col * _Color3 * 2;
            }
            ENDCG
        }

    }

}