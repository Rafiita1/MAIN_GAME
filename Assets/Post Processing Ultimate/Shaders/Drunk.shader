//{"Values":["0","NTEC/Screen/Drunk","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":1.0,"width":212.0,"height":89.0},"name":"Float","selected":false,"Values":["Horizontal","Horizontal wobble","1"],"serial":0,"unique":-1,"type":"FloatField"}|{"position":{"serializedVersion":"2","x":0.0,"y":100.0,"width":212.0,"height":89.0},"name":"Float","selected":false,"Values":["Vertical","Vertical wobble","1"],"serial":1,"unique":843,"type":"FloatField"}|{"position":{"serializedVersion":"2","x":0.0,"y":199.0,"width":212.0,"height":89.0},"name":"Float","selected":false,"Values":["Speed","Animation speed","1"],"serial":2,"unique":1106,"type":"FloatField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None"]}
//\	CameraOutput\	3793.106\	207.5578\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			1\			3\	CameraInput\	3403.103\	189.5576\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			null\			null\		True\			5\			0\		True\			6\			0\		False\			null\			null\	StereoUV\	486.4661\	546.9622\	192\	175\		True\			null\			null\		True\			null\			null\		False\			null\			null\	Sin\	2192.058\	505.367\	192\	135\		True\			null\			null\		True\			13\			0\	Sin\	1977.109\	880.7962\	192\	135\		True\			null\			null\		True\			15\			0\	Add\	3041.258\	180.0178\	192\	215\		/4\		True\			null\			null\		True\			2\			0\		True\			8\			0\		False\			null\			null\	Add\	3116.257\	862.5182\	192\	215\		/4\		True\			null\			null\		True\			2\			1\		True\			7\			0\		False\			null\			null\	Mul\	2624.517\	957.7582\	192\	215\		/4\		True\			null\			null\		True\			4\			0\		True\			16\			0\		False\			null\			null\	Mul\	2621.342\	496.1684\	192\	215\		/4\		True\			null\			null\		True\			3\			0\		True\			16\			0\		False\			null\			null\	Mul\	1892.678\	1582.469\	192\	215\		/4\		True\			null\			null\		True\			20\			0\		True\			17\			0\		False\			null\			null\	_Float\	656.1349\	1525.816\	192\	95\		/Speed\		/3\		/1106\		True\			null\			null\	_Float\	2189.823\	692.9578\	192\	95\		/Horizontal\		/1\		/-1\		True\			null\			null\	_Float\	2224.108\	1067.244\	192\	95\		/Vertical\		/2\		/843\		True\			null\			null\	Mul\	1884.029\	428.791\	192\	255\		/5\		True\			null\			null\		True\			21\			0\		True\			14\			0\		True\			12\			0\		False\			null\			null\	Value1\	1325.714\	828.3148\	192\	95\		/10.0\		True\			null\			null\	Mul\	1776.308\	1054.379\	192\	255\		/5\		True\			null\			null\		True\			22\			0\		True\			14\			0\		True\			11\			0\		False\			null\			null\	Value1\	2320.219\	1518.554\	192\	95\		/0.01\		True\			null\			null\	Value1\	1007.721\	1618.555\	192\	95\		/0.1\		True\			null\			null\	Time\	578.4524\	1130.223\	192\	255\		False\			null\			null\		True\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\	Mul\	993.4913\	1332.644\	192\	215\		/4\		True\			null\			null\		True\			18\			1\		True\			10\			0\		False\			null\			null\	Add\	1499.641\	1416.176\	192\	175\		/3\		True\			null\			null\		True\			19\			0\		False\			null\			null\	Add\	1162.658\	579.0716\	192\	215\		/4\		True\			null\			null\		True\			2\			1\		True\			9\			0\		False\			null\			null\	Add\	965.515\	850.5001\	192\	215\		/4\		True\			null\			null\		True\			2\			0\		True\			9\			0\		False\			null\			null

Shader "NTEC/Screen/Drunk" {

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
			uniform half _Speed;

			half4 Frag (VaryingsDefault i) : SV_Target {
				half4 CameraOutput = 0.0;
				CameraOutput.rgb = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, half2((i.texcoordStereo.x + (sin(((i.texcoordStereo.y + (((_Time.y * _Speed)) * 0.1)) * 10.0 * _Vertical)) * 0.01)),(i.texcoordStereo.y + (sin(((i.texcoordStereo.x + (((_Time.y * _Speed)) * 0.1)) * 10.0 * _Horizontal)) * 0.01)))).rgb;
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}