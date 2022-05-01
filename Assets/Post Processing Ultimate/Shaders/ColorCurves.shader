//{"Values":["0","NTEC/Screen/ColorCurves","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":0.0,"width":212.0,"height":69.0},"name":"Spline","selected":false,"Values":["Red","Red channel curve"],"serial":0,"unique":-1,"type":"SplineField"}|{"position":{"serializedVersion":"2","x":0.0,"y":86.0,"width":212.0,"height":69.0},"name":"Spline","selected":false,"Values":["Green","Green channel curve"],"serial":1,"unique":3646,"type":"SplineField"}|{"position":{"serializedVersion":"2","x":0.0,"y":172.0,"width":212.0,"height":69.0},"name":"Spline","selected":false,"Values":["Blue","Blue channel curve"],"serial":2,"unique":1415,"type":"SplineField"}|{"position":{"serializedVersion":"2","x":0.0,"y":258.0,"width":212.0,"height":89.0},"name":"Color","selected":false,"Values":["Tint","Image filter","1","1","1","0"],"serial":3,"unique":214,"type":"ColorField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None"]}
//\	CameraOutput\	2600.23\	586.429\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			6\			0\	CameraInput\	1246.23\	694.429\	192\	335\		True\			3\			1\		True\			4\			1\		True\			5\			1\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			2\			2\	StereoUV\	905.6591\	851\	192\	175\		False\			null\			null\		False\			null\			null\		True\			1\			6\	_Spline\	1594.23\	696.429\	192\	135\		/Red\		/1\		/-1\		True\			7\			6\		True\			1\			0\	_Spline\	1586.23\	836.429\	192\	135\		/Green\		/2\		/3646\		True\			7\			5\		True\			1\			1\	_Spline\	1576.23\	980.429\	192\	135\		/Blue\		/3\		/1415\		True\			7\			4\		True\			1\			2\	Mul\	2338\	938\	192\	215\		/4\		True\			0\			3\		True\			7\			3\		True\			8\			0\		False\			null\			null\	Custom3\	2068\	488\	192\	415\		/Assets/Post Processing Ultimate/Functions/Append3.hlsl\		/Append3\		/2\		/1,1,1\		/Z,Y,X\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			6\			1\		True\			5\			0\		True\			4\			0\		True\			3\			0\	_Color\	1924\	1188\	192\	255\		/Tint\		/1\		/214\		True\			6\			2\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null

Shader "NTEC/Screen/ColorCurves" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
			#include "../../Post Processing Ultimate/Functions/Append3.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			TEXTURE2D_SAMPLER2D(_Red, sampler_Red);
			TEXTURE2D_SAMPLER2D(_Green, sampler_Green);
			TEXTURE2D_SAMPLER2D(_Blue, sampler_Blue);
			uniform half4 _Tint;

			half4 Frag (VaryingsDefault i) : SV_Target {
				half4 CameraOutput = 0.0;
				CameraOutput.rgb = (Append3(SAMPLE_TEXTURE2D(_Blue, sampler_Blue, half2(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).b, 0.5)).a, SAMPLE_TEXTURE2D(_Green, sampler_Green, half2(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).g, 0.5)).a, SAMPLE_TEXTURE2D(_Red, sampler_Red, half2(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).r, 0.5)).a) * _Tint);
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}