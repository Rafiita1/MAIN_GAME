//{"Values":["0","NTEC/Screen/Sepia","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":0.0,"width":212.0,"height":89.0},"name":"Color","selected":false,"Values":["Color","Sepia color","0.6745098","0.4784314","0.2","0"],"serial":0,"unique":-1,"type":"ColorField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None"]}
//\	CameraOutput\	2051.78\	38.22226\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			4\			0\	CameraInput\	917.7778\	-3.777733\	192\	335\		True\			3\			1\		True\			3\			2\		True\			3\			3\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			2\			2\	StereoUV\	517.7778\	-3.777733\	192\	175\		False\			null\			null\		False\			null\			null\		True\			1\			6\	Av\	1395.779\	244.2222\	192\	255\		/5\		True\			4\			1\		True\			1\			0\		True\			1\			1\		True\			1\			2\		False\			null\			null\	Mul\	1743.779\	428.2223\	192\	215\		/4\		True\			0\			3\		True\			3\			0\		True\			5\			0\		False\			null\			null\	_Color\	1104.445\	600\	192\	255\		/Color\		/1\		/-1\		True\			4\			2\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null

Shader "NTEC/Screen/Sepia" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half4 _Color;

			half4 Frag (VaryingsDefault i) : SV_Target {
				half4 CameraOutput = 0.0;
				CameraOutput.rgb = (((SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).r + SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).g + SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).b) / 3.0) * _Color);
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}