//{"Values":["0","NTEC/Screen/CrossedEyes","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":2.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Shift","Horizontal shift","0.5","0","1"],"serial":0,"unique":239,"type":"FloatSliderField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None"]}
//\	CameraOutput\	2709.683\	11.76782\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			3\			0\	CameraInput\	2185.682\	-18.23218\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			null\			null\		True\			10\			0\		True\			2\			1\		False\			null\			null\	StereoUV\	1134.682\	200.2679\	192\	175\		True\			null\			null\		True\			null\			null\		False\			null\			null\	Lerp\	2589.5\	405.2224\	192\	215\		True\			null\			null\		True\			1\			3\		True\			6\			3\		True\			9\			0\	Add\	1829\	732.2224\	192\	215\		/4\		True\			null\			null\		True\			2\			0\		True\			7\			0\		False\			null\			null\	_FloatSlider\	815.9999\	686.2225\	192\	95\		/Shift\		/1\		/239\		True\			null\			null\	CameraInput\	2189.682\	367.7678\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			null\			null\		True\			4\			0\		True\			2\			1\		False\			null\			null\	Div\	1262.5\	850.723\	192\	215\		/4\		True\			null\			null\		True\			5\			0\		True\			8\			0\		False\			null\			null\	Value1\	771.4999\	1107.224\	192\	95\		/50.0\		True\			null\			null\	Value1\	2287.5\	960.0001\	192\	95\		/0.5\		True\			null\			null\	Sub\	1775\	362.5001\	192\	215\		/4\		True\			null\			null\		True\			2\			0\		True\			7\			0\		False\			null\			null

Shader "NTEC/Screen/CrossedEyes" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half _Shift;

			half4 Frag (VaryingsDefault i) : SV_Target {
				half4 CameraOutput = 0.0;
				CameraOutput.rgb = lerp(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, half2((i.texcoordStereo.x - (_Shift / 50.0)),i.texcoordStereo.y)).rgb,SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, half2((i.texcoordStereo.x + (_Shift / 50.0)),i.texcoordStereo.y)).rgb,0.5);
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}