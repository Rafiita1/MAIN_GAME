//{"Values":["0","NTEC/Screen/Invert","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":0.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Intensity","Effect intensity","1","0","1"],"serial":0,"unique":-1,"type":"FloatSliderField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None"]}
//\	CameraOutput\	2296\	208\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			4\			0\	CameraInput\	944\	96\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			3\			9\		False\			null\			null\		False\			null\			null\		True\			2\			2\	StereoUV\	550\	382\	192\	175\		False\			null\			null\		False\			null\			null\		True\			1\			6\	Custom4\	1470\	208\	192\	535\		/Assets/Post Processing Ultimate/Functions/Invert.hlsl\		/Invert\		/3\		/4\		/Value\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			4\			2\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			1\			3\	Lerp\	1854\	706\	192\	215\		True\			0\			3\		True\			1\			3\		True\			3\			4\		True\			5\			0\	_FloatSlider\	1320\	1078\	192\	95\		/Intensity\		/1\		/-1\		True\			4\			3

Shader "NTEC/Screen/Invert" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
			#include "../../Post Processing Ultimate/Functions/Invert.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half _Intensity;

			half4 Frag (VaryingsDefault i) : SV_Target {
				half4 CameraOutput = 0.0;
				CameraOutput.rgb = lerp(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb,Invert(half4(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb,0.0)),_Intensity);
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}