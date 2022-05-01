//{"Values":["0","NTEC/Screen/Distortion","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":0.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Intensity","Effect intensity","0.5","0","1"],"serial":0,"unique":3095,"type":"FloatSliderField"}|{"position":{"serializedVersion":"2","x":0.0,"y":126.0,"width":212.0,"height":69.0},"name":"Texture2D","selected":false,"Values":["Coords","New coords"],"serial":1,"unique":-1,"type":"Texture2DField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None"]}
//\	CameraOutput\	2602.389\	138.5558\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			1\			3\	CameraInput\	2210.389\	222.5558\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			0\			3\		False\			null\			null\		False\			null\			null\		True\			5\			0\	StereoUV\	621.8888\	215.5557\	192\	175\		False\			null\			null\		False\			null\			null\		True\			6\			2\	_Texture2D\	977.3889\	767.0557\	192\	375\		/Coords\		/1\		/-1\		False\			null\			null\		True\			4\			3\		True\			4\			4\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			2\			2\	Custom2\	1494.889\	849.5557\	192\	335\		/Assets/Post Processing Ultimate/Functions/Append2.hlsl\		/Append2\		/1\		/1,1\		/Y,X\		False\			null\			null\		False\			null\			null\		True\			8\			1\		True\			3\			1\		True\			3\			2\	Add\	1879.778\	296.0002\	192\	215\		/4\		True\			1\			6\		True\			2\			2\		True\			8\			0\		False\			null\			null\	Sub\	1457.5\	1210\	192\	215\		/4\		True\			8\			2\		True\			7\			2\		True\			2\			2\		False\			null\			null\	Value2\	1395\	460\	192\	175\		/0.5\		/0.5\		False\			null\			null\		False\			null\			null\		True\			6\			1\	Mul\	1860\	912.5001\	192\	255\		/5\		True\			5\			2\		True\			4\			2\		True\			6\			0\		True\			9\			0\		False\			null\			null\	_FloatSlider\	1746\	1314\	192\	95\		/Intensity\		/1\		/3095\		True\			8\			3

Shader "NTEC/Screen/Distortion" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
			#include "../../Post Processing Ultimate/Functions/Append2.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half _Intensity;
			TEXTURE2D_SAMPLER2D(_Coords, sampler_Coords);

			half4 Frag (VaryingsDefault i) : SV_Target {
				half4 CameraOutput = 0.0;
				CameraOutput.rgb = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, (i.texcoordStereo + (Append2(SAMPLE_TEXTURE2D(_Coords, sampler_Coords, i.texcoordStereo).r, SAMPLE_TEXTURE2D(_Coords, sampler_Coords, i.texcoordStereo).g) * (half2(0.5,0.5) - i.texcoordStereo) * _Intensity))).rgb;
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}