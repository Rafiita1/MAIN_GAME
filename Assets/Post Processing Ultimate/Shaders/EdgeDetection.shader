//{"Values":["0","NTEC/Screen/EdgeDetection","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":0.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Edge","Edge Offset","0.5","0","1"],"serial":0,"unique":-1,"type":"FloatSliderField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None"]}
//\	CameraOutput\	6178.727\	-696.2085\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			20\			0\	CameraInput\	2340.871\	-686.1611\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			20\			1\		False\			null\			null\		False\			null\			null\		True\			2\			2\	StereoUV\	830.0494\	-470.5268\	192\	175\		True\			6\			1\		True\			7\			1\		True\			1\			6\	Add\	2007.863\	171.8765\	192\	215\		/4\		True\			10\			5\		True\			2\			1\		True\			16\			0\		False\			null\			null\	Sub\	3671.568\	-324.0988\	192\	215\		/4\		True\			18\			2\		True\			1\			3\		True\			14\			0\		False\			null\			null\	Add\	1800.572\	-289.4557\	192\	215\		/4\		True\			10\			4\		True\			2\			0\		True\			16\			0\		False\			null\			null\	Sub\	1630.427\	917.9368\	192\	215\		/4\		True\			12\			4\		True\			2\			0\		True\			16\			0\		False\			null\			null\	Sub\	1958.79\	609.4053\	192\	215\		/4\		True\			12\			5\		True\			2\			1\		True\			16\			0\		False\			null\			null\	_FloatSlider\	366.4576\	-12.98822\	192\	95\		/Edge\		/1\		/-1\		True\			16\			1\	CameraInput\	2459.085\	-275.8046\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			13\			1\		True\			5\			0\		True\			3\			0\		False\			null\			null\	CameraInput\	2474.513\	75.62286\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			13\			2\		True\			5\			0\		True\			3\			0\		False\			null\			null\	CameraInput\	2508.798\	478.4774\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			13\			3\		True\			6\			0\		True\			7\			0\		False\			null\			null\	CameraInput\	2497.37\	855.6198\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			13\			4\		True\			6\			0\		True\			7\			0\		False\			null\			null\	Add\	3163.36\	-22.12299\	192\	295\		/6\		True\			14\			1\		True\			9\			3\		True\			10\			3\		True\			11\			3\		True\			12\			3\		False\			null\			null\	Div\	3563.363\	311.2093\	192\	215\		/4\		True\			4\			2\		True\			13\			0\		True\			15\			0\		False\			null\			null\	Value1\	3160.026\	761.2059\	192\	95\		/4.0\		True\			14\			2\	Div\	1225.081\	327.0081\	192\	215\		/4\		True\			7\			2\		True\			8\			0\		True\			17\			0\		False\			null\			null\	Value1\	1018.413\	691.451\	192\	95\		/300.0\		True\			16\			2\	Sub\	4150.029\	-307.7338\	192\	215\		/4\		True\			22\			7\		True\			19\			0\		True\			4\			0\		False\			null\			null\	Value1\	3709.22\	-694.8043\	192\	95\		/1.0\		True\			23\			9\	Mul\	5730.561\	-656.395\	192\	215\		/4\		True\			0\			3\		True\			1\			3\		True\			23\			2\		False\			null\			null\	Av\	4875.991\	-368.3415\	192\	255\		/5\		True\			23\			6\		True\			22\			0\		True\			22\			1\		True\			22\			2\		False\			null\			null\	Variable3\	4518.214\	-379.4528\	192\	415\		/0\		/0\		True\			21\			1\		True\			21\			2\		True\			21\			3\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			18\			0\	Compare\	5275.991\	-417.2304\	192\	455\		False\			null\			null\		False\			null\			null\		True\			20\			2\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			21\			0\		True\			24\			0\		False\			null\			null\		True\			19\			0\	Value1\	4863.325\	11.04742\	192\	95\		/0.9\		True\			23\			7

Shader "NTEC/Screen/EdgeDetection" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half _Edge;

			half4 Frag (VaryingsDefault i) : SV_Target {
				half3 var0 = 0.0;
				half4 CameraOutput = 0.0;
				var0 = (1.0 - (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb - ((SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, half2((i.texcoordStereo.x + (_Edge / 300.0)),(i.texcoordStereo.y + (_Edge / 300.0)))).rgb + SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, half2((i.texcoordStereo.x + (_Edge / 300.0)),(i.texcoordStereo.y + (_Edge / 300.0)))).rgb + SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, half2((i.texcoordStereo.x - (_Edge / 300.0)),(i.texcoordStereo.y - (_Edge / 300.0)))).rgb + SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, half2((i.texcoordStereo.x - (_Edge / 300.0)),(i.texcoordStereo.y - (_Edge / 300.0)))).rgb) / 4.0)));
				CameraOutput.rgb = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb * (((var0.x + var0.y + var0.z) / 3.0) < 0.9 ? 0.0 : 1.0));
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}