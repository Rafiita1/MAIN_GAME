//{"Values":["0","NTEC/Screen/Posterize","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":0.0,"width":212.0,"height":89.0},"name":"Int","selected":false,"Values":["Colors","Number of colors in each channel","8"],"serial":0,"unique":-1,"type":"IntField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None","Colors"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None","Colors"]}
//\	CameraOutput\	3282.152\	251.2776\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			2\			0\	StereoUV\	918.6266\	417.0871\	192\	175\		False\			null\			null\		False\			null\			null\		True\			5\			6\	Div\	3076.77\	702.9441\	192\	215\		/4\		True\			0\			3\		True\			4\			0\		True\			6\			0\		False\			null\			null\	Mul\	2356.769\	654.9441\	192\	215\		/4\		True\			4\			1\		True\			5\			3\		True\			6\			0\		False\			null\			null\	Floor\	2702.769\	690.9441\	192\	135\		True\			2\			1\		True\			3\			0\	CameraInput\	1878.103\	329.7775\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			3\			1\		False\			null\			null\		False\			null\			null\		True\			1\			2\	_Int\	840.1421\	865.6679\	192\	95\		/Colors\		/1\		/-1\		True\			3\			2

Shader "NTEC/Screen/Posterize" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half _Colors;

			half4 Frag (VaryingsDefault i) : SV_Target {
				half4 CameraOutput = 0.0;
				CameraOutput.rgb = (floor((SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb * _Colors)) / _Colors);
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}