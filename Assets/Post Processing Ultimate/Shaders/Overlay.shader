//{"Values":["0","NTEC/Screen/Overlay","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":0.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Intensity","Effect intensity","0.5","0","1"],"serial":0,"unique":2557,"type":"FloatSliderField"}|{"position":{"serializedVersion":"2","x":0.0,"y":126.0,"width":212.0,"height":69.0},"name":"Texture2D","selected":false,"Values":["Image","Overlay texture"],"serial":1,"unique":-1,"type":"Texture2DField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None"]}
//\	CameraOutput\	1995.111\	126.8333\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			4\			0\	CameraInput\	1217.111\	144.8333\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			4\			1\		False\			null\			null\		False\			null\			null\		True\			2\			2\	StereoUV\	793.1112\	272.8333\	192\	175\		False\			null\			null\		False\			null\			null\		True\			3\			7\	_Texture2D\	1149.111\	558.8333\	192\	375\		/Image\		/1\		/-1\		True\			4\			2\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			5\			1\		False\			null\			null\		False\			null\			null\		True\			2\			2\	Lerp\	1840.611\	442.8333\	192\	215\		True\			0\			3\		True\			1\			3\		True\			3\			0\		True\			5\			0\	Mul\	1627.5\	705.0001\	192\	215\		/4\		True\			4\			3\		True\			3\			4\		True\			6\			0\		False\			null\			null\	_FloatSlider\	1352.5\	1052.5\	192\	95\		/Intensity\		/1\		/2557\		True\			5\			2

Shader "NTEC/Screen/Overlay" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half _Intensity;
			TEXTURE2D_SAMPLER2D(_Image, sampler_Image);

			half4 Frag (VaryingsDefault i) : SV_Target {
				half4 CameraOutput = 0.0;
				CameraOutput.rgb = lerp(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb,SAMPLE_TEXTURE2D(_Image, sampler_Image, i.texcoordStereo),(SAMPLE_TEXTURE2D(_Image, sampler_Image, i.texcoordStereo).a * _Intensity));
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}