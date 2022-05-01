//{"Values":["0","NTEC/Screen/Fade","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":0.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Intensity","Effect intensity","0.5","0","1"],"serial":0,"unique":1598,"type":"FloatSliderField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None"]}
//\	CameraOutput\	2235.99\	828.8287\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			4\			0\	StereoUV\	676\	564\	192\	175\		False\			null\			null\		False\			null\			null\		True\			2\			6\	CameraInput\	1564\	366\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			4\			1\		False\			null\			null\		False\			null\			null\		True\			1\			2\	_FloatSlider\	706\	1236\	192\	95\		/Intensity\		/1\		/1598\		True\			5\			5\	Mul\	1768\	1006\	192\	215\		/4\		True\			0\			3\		True\			2\			3\		True\			5\			0\		False\			null\			null\	Custom4\	1292\	792\	192\	535\		/Assets/Post Processing Ultimate/Functions/Invert.hlsl\		/Invert\		/3\		/4\		/Value\		True\			4\			2\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			3\			0\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null

Shader "NTEC/Screen/Fade" {

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
				CameraOutput.rgb = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb * Invert(half4(_Intensity, 0.0, 0.0, 0.0)).x);
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}