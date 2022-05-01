//{"Values":["0","NTEC/Screen/SelectColor","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":2.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Intensity","Effect intensity","0.7","0","1"],"serial":0,"unique":271,"type":"FloatSliderField"}|{"position":{"serializedVersion":"2","x":0.0,"y":119.0,"width":212.0,"height":89.0},"name":"Color","selected":false,"Values":["Selected","Selected color","1","0","0","0"],"serial":1,"unique":-1,"type":"ColorField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None"]}
//\	CameraOutput\	2953.419\	-34.87292\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			12\			0\	CameraInput\	27.70132\	206.0793\	192\	335\		True\			1\			0\		True\			null\			null\		True\			null\			null\		True\			null\			null\		False\			null\			null\		False\			null\			null\		True\			2\			2\	StereoUV\	-300.869\	23.22235\	192\	175\		False\			null\			null\		False\			null\			null\		True\			null\			null\	_Color\	-309.2025\	758.3889\	192\	255\		/Selected\		/1\		/-1\		False\			null\			null\		True\			null\			null\		True\			null\			null\		True\			null\			null\		False\			null\			null\	Sub\	475.7974\	578.3889\	192\	215\		/4\		True\			null\			null\		True\			3\			1\		True\			1\			0\		False\			null\			null\	Sub\	470.7974\	828.3889\	192\	215\		/4\		True\			null\			null\		True\			3\			2\		True\			1\			1\		False\			null\			null\	Sub\	415.7974\	1070.89\	192\	215\		/4\		True\			null\			null\		True\			3\			3\		True\			1\			2\		False\			null\			null\	Abs\	798.2976\	575.8889\	192\	135\		True\			null\			null\		True\			4\			0\	Abs\	813.2976\	760.8889\	192\	135\		True\			null\			null\		True\			5\			0\	Abs\	810.7976\	1075.891\	192\	135\		True\			null\			null\		True\			6\			0\	Av\	1467.583\	780.8889\	192\	255\		/5\		True\			null\			null\		True\			7\			0\		True\			8\			0\		True\			9\			0\		False\			null\			null\	Av\	1344.726\	409.4604\	192\	255\		/5\		True\			null\			null\		True\			1\			0\		True\			1\			1\		True\			1\			2\		False\			null\			null\	Lerp\	2776.088\	379.8887\	192\	215\		True\			null\			null\		True\			1\			3\		True\			11\			0\		True\			16\			0\	Mul\	1852.894\	839.3059\	192\	255\		/5\		True\			null\			null\		True\			14\			0\		True\			10\			0\		True\			17\			0\		False\			null\			null\	_FloatSlider\	1567.18\	1102.164\	192\	95\		/Intensity\		/1\		/271\		True\			null\			null\	Pow\	2228.006\	866.02\	192\	175\		True\			16\			1\		True\			13\			0\		True\			17\			0\	Saturate\	2646\	814\	192\	135\		True\			12\			3\		True\			15\			0\	Value1\	1422\	1260\	192\	95\		/4.0\		True\			15\			2

Shader "NTEC/Screen/SelectColor" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half _Intensity;
			uniform half4 _Selected;

			half4 Frag (VaryingsDefault i) : SV_Target {
				half4 CameraOutput = 0.0;
				CameraOutput.rgb = lerp(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb,((SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).r + SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).g + SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).b) / 3.0),saturate(pow((_Intensity * ((abs((_Selected.r - SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).r)) + abs((_Selected.g - SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).g)) + abs((_Selected.b - SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).b))) / 3.0) * 4.0),4.0)));
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}