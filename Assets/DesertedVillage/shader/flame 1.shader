// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.32 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.32;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-4684-OUT,alpha-6084-OUT,voffset-7453-OUT;n:type:ShaderForge.SFN_Tex2d,id:1643,x:31753,y:33439,ptovrint:False,ptlb:Utils_map,ptin:_Utils_map,varname:node_1643,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3573-UVOUT;n:type:ShaderForge.SFN_Multiply,id:4684,x:32469,y:32871,varname:node_4684,prsc:2|A-657-RGB,B-9019-OUT;n:type:ShaderForge.SFN_Tex2d,id:657,x:32221,y:32738,ptovrint:False,ptlb:flame_basecolor,ptin:_flame_basecolor,varname:_Utils_map_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:6572,x:31901,y:32930,ptovrint:False,ptlb:emission,ptin:_emission,varname:node_6572,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_Multiply,id:9019,x:32303,y:32955,varname:node_9019,prsc:2|A-9055-OUT,B-7016-OUT;n:type:ShaderForge.SFN_Panner,id:3573,x:31209,y:33300,varname:node_3573,prsc:2,spu:0.1,spv:0.05|UVIN-7081-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:7081,x:30810,y:33300,varname:node_7081,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:7453,x:32303,y:33148,varname:node_7453,prsc:2|A-4676-OUT,B-6722-OUT;n:type:ShaderForge.SFN_Vector1,id:6084,x:32433,y:33057,varname:node_6084,prsc:2,v1:0.5;n:type:ShaderForge.SFN_TexCoord,id:9876,x:31352,y:33040,varname:node_9876,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:6923,x:32074,y:33417,varname:node_6923,prsc:2|A-8505-OUT,B-1643-RGB;n:type:ShaderForge.SFN_Multiply,id:9709,x:31700,y:32997,varname:node_9709,prsc:2|A-9876-V,B-728-OUT;n:type:ShaderForge.SFN_Vector1,id:728,x:31402,y:33200,varname:node_728,prsc:2,v1:1.5;n:type:ShaderForge.SFN_ValueProperty,id:8505,x:31926,y:33638,ptovrint:False,ptlb:flicker,ptin:_flicker,varname:node_8505,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ObjectPosition,id:3733,x:31781,y:33225,varname:node_3733,prsc:2;n:type:ShaderForge.SFN_Sin,id:770,x:32520,y:33478,varname:node_770,prsc:2|IN-599-OUT;n:type:ShaderForge.SFN_Multiply,id:599,x:32325,y:33478,varname:node_599,prsc:2|A-6923-OUT,B-6680-OUT;n:type:ShaderForge.SFN_Vector1,id:6680,x:32074,y:33599,varname:node_6680,prsc:2,v1:0.01;n:type:ShaderForge.SFN_Multiply,id:6722,x:32303,y:33299,varname:node_6722,prsc:2|A-770-OUT,B-4033-OUT;n:type:ShaderForge.SFN_Vector3,id:4033,x:31890,y:33342,varname:node_4033,prsc:2,v1:1,v2:3,v3:1;n:type:ShaderForge.SFN_Multiply,id:7016,x:32128,y:33014,varname:node_7016,prsc:2|A-6572-OUT,B-657-A;n:type:ShaderForge.SFN_Vector1,id:9055,x:32102,y:32955,varname:node_9055,prsc:2,v1:50;n:type:ShaderForge.SFN_Clamp01,id:4676,x:31870,y:33035,varname:node_4676,prsc:2|IN-9709-OUT;proporder:1643-657-6572-8505;pass:END;sub:END;*/

Shader "TripleBrick/flame" {
    Properties {
        _Utils_map ("Utils_map", 2D) = "white" {}
        _flame_basecolor ("flame_basecolor", 2D) = "white" {}
        _emission ("emission", Float ) = 3
        _flicker ("flicker", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles n3ds wiiu 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _Utils_map; uniform float4 _Utils_map_ST;
            uniform sampler2D _flame_basecolor; uniform float4 _flame_basecolor_ST;
            uniform float _emission;
            uniform float _flicker;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_843 = _Time + _TimeEditor;
                float2 node_3573 = (o.uv0+node_843.g*float2(0.1,0.05));
                float4 _Utils_map_var = tex2Dlod(_Utils_map,float4(TRANSFORM_TEX(node_3573, _Utils_map),0.0,0));
                v.vertex.xyz += (saturate((o.uv0.g*1.5))*(sin(((_flicker*_Utils_map_var.rgb)*0.01))*float3(1,3,1)));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
////// Lighting:
////// Emissive:
                float4 _flame_basecolor_var = tex2D(_flame_basecolor,TRANSFORM_TEX(i.uv0, _flame_basecolor));
                float3 emissive = (_flame_basecolor_var.rgb*(50.0*(_emission*_flame_basecolor_var.a)));
                float3 finalColor = emissive;
                return fixed4(finalColor,0.5);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles n3ds wiiu 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _Utils_map; uniform float4 _Utils_map_ST;
            uniform float _flicker;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 node_1056 = _Time + _TimeEditor;
                float2 node_3573 = (o.uv0+node_1056.g*float2(0.1,0.05));
                float4 _Utils_map_var = tex2Dlod(_Utils_map,float4(TRANSFORM_TEX(node_3573, _Utils_map),0.0,0));
                v.vertex.xyz += (saturate((o.uv0.g*1.5))*(sin(((_flicker*_Utils_map_var.rgb)*0.01))*float3(1,3,1)));
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
