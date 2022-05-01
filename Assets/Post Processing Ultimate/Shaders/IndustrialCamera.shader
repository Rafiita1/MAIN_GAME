//{"Values":["0","NTEC/Screen/IndustrialCamera","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":1.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Intensity","Effect intensity","0.5","0","1"],"serial":0,"unique":3352,"type":"FloatSliderField"}|{"position":{"serializedVersion":"2","x":0.0,"y":120.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Lines","Lines visibility","0.5","0","1"],"serial":1,"unique":-1,"type":"FloatSliderField"}|{"tempTextures":1,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game","tempRT0"],"OutputLabels":["Screen","tempRT0"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game","tempRT0"],"outputOptions":["Screen","tempRT0"],"variableOptions":["None"]}
//\	CameraOutput\	4076.619\	114.8591\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			21\			0\	CameraInput\	1517.86\	263.5735\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			null\			null\		False\			null\			null\		False\			null\			null\		True\			2\			2\	StereoUV\	678.81\	285.7158\	192\	175\		False\			null\			null\		True\			null\			null\		True\			null\			null\	Variable3\	1893.74\	148.6497\	192\	415\		/0\		/0\		True\			null\			null\		True\			null\			null\		True\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			1\			3\	Add\	2298.182\	259.7607\	192\	255\		/5\		True\			null\			null\		True\			3\			0\		True\			3\			1\		True\			3\			2\		False\			null\			null\	Div\	2602.628\	401.9829\	192\	215\		/4\		True\			null\			null\		True\			4\			0\		True\			6\			0\		False\			null\			null\	Value1\	2271.516\	606.4275\	192\	95\		/3.0\		True\			null\			null\	Mul\	3188.858\	555.9128\	192\	215\		/4\		True\			null\			null\		True\			5\			0\		True\			19\			0\		False\			null\			null\	Abs\	2421.835\	763.8887\	192\	135\		True\			null\			null\		True\			9\			0\	Sin\	2113.739\	793.8887\	192\	135\		True\			null\			null\		True\			10\			0\	Mul\	1670.406\	737.2219\	192\	215\		/4\		True\			null\			null\		True\			12\			0\		True\			11\			0\		False\			null\			null\	Value1\	1275.644\	811.0313\	192\	95\		/16.0\		True\			null\			null\	Add\	993.0249\	553.1733\	192\	215\		/4\		True\			null\			null\		True\			2\			1\		True\			13\			0\		False\			null\			null\	Time\	373.2228\	658.4531\	192\	255\		True\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\	Floor\	2813.938\	804.5242\	192\	135\		True\			null\			null\		True\			15\			0\	Mul\	2527.549\	981.5671\	192\	215\		/4\		True\			null\			null\		True\			8\			0\		True\			16\			0\		False\			null\			null\	Value1\	2164.653\	1098.454\	192\	95\		/3.0\		True\			null\			null\	Lerp\	3130.367\	95.59595\	192\	215\		True\			null\			null\		True\			5\			0\		True\			7\			0\		True\			18\			0\	_FloatSlider\	2776.08\	281.3103\	192\	95\		/Lines\		/2\		/-1\		True\			null\			null\	Lerp\	3247.152\	1037.618\	192\	215\		True\			null\			null\		True\			14\			0\		True\			15\			0\		True\			20\			0\	Value1\	2842.153\	1167.618\	192\	95\		/0.5\		True\			null\			null\	Lerp\	3840.152\	537.6191\	192\	215\		True\			null\			null\		True\			1\			3\		True\			17\			0\		True\			22\			0\	_FloatSlider\	3574.152\	779.6191\	192\	95\		/Intensity\		/1\		/3352\		True\			null\			null

Shader "NTEC/Screen/IndustrialCamera" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half _Intensity;
			uniform half _Lines;

			half4 Frag (VaryingsDefault i) : SV_Target {
				half3 var0 = 0.0;
				half4 CameraOutput = 0.0;
				var0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb;
				CameraOutput.rgb = lerp(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb,lerp(((var0.x + var0.y + var0.z) / 3.0),(((var0.x + var0.y + var0.z) / 3.0) * lerp(floor((abs(sin(((i.texcoordStereo.y + _Time.x) * 16.0))) * 3.0)),(abs(sin(((i.texcoordStereo.y + _Time.x) * 16.0))) * 3.0),0.5)),_Lines),_Intensity);
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}