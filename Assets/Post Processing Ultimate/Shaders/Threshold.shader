//{"Values":["0","NTEC/Screen/Threshold","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":0.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Intensity","Effect visibility","0.5","0","1"],"serial":0,"unique":-1,"type":"FloatSliderField"}|{"position":{"serializedVersion":"2","x":0.0,"y":126.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Level","Threshold level","0.5","0","1"],"serial":1,"unique":1179,"type":"FloatSliderField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None"]}
//\	CameraOutput\	3311.785\	148.111\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			3\			0\	CameraInput\	929.087\	413.0313\	192\	335\		True\			5\			1\		True\			5\			2\		True\			5\			3\		True\			3\			1\		False\			null\			null\		False\			null\			null\		True\			2\			2\	StereoUV\	529.0869\	413.0313\	192\	175\		False\			null\			null\		False\			null\			null\		True\			1\			6\	Lerp\	2663.977\	341.9199\	192\	215\		True\			0\			3\		True\			1\			3\		True\			8\			1\		True\			4\			0\	_FloatSlider\	1965.626\	634.7147\	192\	95\		/Intensity\		/1\		/-1\		True\			3\			3\	Add\	1329.055\	939.8574\	192\	255\		/5\		True\			6\			1\		True\			1\			0\		True\			1\			1\		True\			1\			2\		False\			null\			null\	Div\	1834.77\	905.572\	192\	215\		/4\		True\			8\			6\		True\			5\			0\		True\			7\			0\		False\			null\			null\	Value1\	1394.769\	1299.857\	192\	95\		/3.0\		True\			6\			2\	Compare\	2442.39\	836.9998\	192\	455\		False\			null\			null\		True\			3\			2\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			6\			0\		True\			9\			0\		True\			10\			0\		False\			null\			null\	_FloatSlider\	2018.223\	1255.333\	192\	95\		/Level\		/2\		/1179\		True\			8\			7\	Value1\	2049.968\	1119.935\	192\	95\		/1.0\		True\			8\			8

Shader "NTEC/Screen/Threshold" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half _Intensity;
			uniform half _Level;

			half4 Frag (VaryingsDefault i) : SV_Target {
				half4 CameraOutput = 0.0;
				CameraOutput.rgb = lerp(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb,(((SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).r + SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).g + SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).b) / 3.0) >= _Level ? 1.0 : 0.0),_Intensity);
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}