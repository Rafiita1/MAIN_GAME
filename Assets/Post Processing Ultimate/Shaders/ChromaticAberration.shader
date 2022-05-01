//{"Values":["0","NTEC/Screen/ChromaticAberration","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":0.0,"width":212.0,"height":109.0},"name":"IntSlider","selected":false,"Values":["Iterations","Effect iterations","1","1","8"],"serial":0,"unique":2581,"type":"IntSliderField"}|{"position":{"serializedVersion":"2","x":0.0,"y":126.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Offset","Channels offset","0.5","0","1"],"serial":1,"unique":-1,"type":"FloatSliderField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None","Iterations"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":1}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None","Iterations"]}
//\	CameraOutput\	2520.418\	317.4919\	192\	215\		True\			1\			0\		True\			3\			1\		True\			4\			2\		False\			null\			null\	CameraInput\	2076.418\	-53.0081\	192\	335\		True\			0\			0\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			7\			0\		True\			2\			1\		False\			null\			null\	StereoUV\	1238.918\	364.4919\	192\	175\		True\			7\			1\		True\			4\			5\		True\			3\			6\	CameraInput\	2076.418\	307.5919\	192\	335\		False\			null\			null\		True\			0\			1\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			2\			2\	CameraInput\	2076.418\	668.1919\	192\	335\		False\			null\			null\		False\			null\			null\		True\			0\			2\		False\			null\			null\		True\			6\			0\		True\			2\			1\		False\			null\			null\	_FloatSlider\	869.1101\	945.041\	192\	95\		/Offset\		/1\		/-1\		True\			8\			1\	Add\	1742.681\	670.0393\	192\	215\		/4\		True\			4\			4\		True\			2\			0\		True\			8\			0\		False\			null\			null\	Sub\	1746.181\	345.5392\	192\	215\		/4\		True\			1\			4\		True\			2\			0\		True\			8\			0\		False\			null\			null\	Div\	1466\	1066\	192\	215\		/4\		True\			7\			2\		True\			5\			0\		True\			9\			0\		False\			null\			null\	Value1\	1034\	1266\	192\	95\		/100.0\		True\			8\			2

Shader "NTEC/Screen/ChromaticAberration" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half _Iterations;
			uniform half _Offset;

			half4 Frag (VaryingsDefault i) : SV_Target {
				half4 CameraOutput = 0.0;
				CameraOutput.r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, half2((i.texcoordStereo.x - (_Offset / 100.0)),i.texcoordStereo.y)).r;
				CameraOutput.g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).g;
				CameraOutput.b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, half2((i.texcoordStereo.x + (_Offset / 100.0)),i.texcoordStereo.y)).b;
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}