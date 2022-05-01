//{"Values":["0","NTEC/Screen/Pixelize","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":0.0,"width":212.0,"height":89.0},"name":"Int","selected":false,"Values":["Horizontal","Horizontal resolution","16"],"serial":0,"unique":-1,"type":"IntField"}|{"position":{"serializedVersion":"2","x":0.0,"y":106.0,"width":212.0,"height":89.0},"name":"Int","selected":false,"Values":["Vertical","Vertical resolution","16"],"serial":1,"unique":3622,"type":"IntField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None","Horizontal","Vertical"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None","Horizontal","Vertical"]}
//\	CameraOutput\	2904.332\	216.1267\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			12\			0\	CameraInput\	2043.952\	643.2932\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			12\			2\		True\			3\			0\		True\			9\			0\		False\			null\			null\	StereoUV\	540.809\	381.9362\	192\	175\		True\			4\			1\		True\			10\			1\		True\			6\			6\	Div\	1623.952\	722.7932\	192\	215\		/4\		True\			1\			4\		True\			5\			0\		True\			7\			0\		False\			null\			null\	Mul\	903.951\	674.7932\	192\	215\		/4\		True\			5\			1\		True\			2\			0\		True\			7\			0\		False\			null\			null\	Floor\	1249.951\	710.7932\	192\	135\		True\			3\			1\		True\			4\			0\	CameraInput\	1765.285\	217.1266\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			12\			3\		False\			null\			null\		False\			null\			null\		True\			2\			2\	_Int\	462.3245\	830.517\	192\	95\		/Horizontal\		/1\		/-1\		True\			13\			1\	_Int\	402.3242\	1102.558\	192\	95\		/Vertical\		/2\		/3622\		True\			13\			2\	Div\	1623.952\	1204.461\	192\	215\		/4\		True\			1\			5\		True\			11\			0\		True\			8\			0\		False\			null\			null\	Mul\	903.951\	1156.459\	192\	215\		/4\		True\			11\			1\		True\			2\			1\		True\			8\			0\		False\			null\			null\	Floor\	1249.951\	1192.461\	192\	135\		True\			9\			1\		True\			10\			0\	If\	2685.618\	699.5249\	192\	215\		True\			0\			3\		True\			13\			0\		True\			1\			3\		True\			6\			3\	And\	2351.81\	1032.859\	192\	175\		True\			12\			1\		True\			7\			0\		True\			8\			0

Shader "NTEC/Screen/Pixelize" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half _Horizontal;
			uniform half _Vertical;

			half4 Frag (VaryingsDefault i) : SV_Target {
				half4 CameraOutput = 0.0;
				CameraOutput.rgb = ((_Horizontal && _Vertical) ? SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, half2((floor((i.texcoordStereo.x * _Horizontal)) / _Horizontal),(floor((i.texcoordStereo.y * _Vertical)) / _Vertical))).rgb : SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb);
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}