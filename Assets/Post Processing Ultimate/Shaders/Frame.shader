//{"Values":["0","NTEC/Screen/Frame","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":0.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Horizontal","Horizontal frame","0.5","0","1"],"serial":0,"unique":-1,"type":"FloatSliderField"}|{"position":{"serializedVersion":"2","x":0.0,"y":126.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Vertical","Vertical frame","0.5","0","1"],"serial":1,"unique":2427,"type":"FloatSliderField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None"]}
//\	CameraOutput\	3515.006\	524.3573\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			12\			0\	CameraInput\	1914.841\	505.5239\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			4\			9\		False\			null\			null\		False\			null\			null\		True\			2\			2\	StereoUV\	823.728\	476.6351\	192\	175\		True\			9\			1\		True\			10\			1\		True\			1\			6\	Value1\	1908.617\	1021.745\	192\	95\		/0.0\		True\			12\			8\	Compare\	2943.507\	725.3015\	192\	455\		True\			12\			9\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			7\			0\		True\			14\			0\		True\			3\			0\		True\			1\			3\	_FloatSlider\	1371.95\	1522.192\	192\	95\		/Horizontal\		/1\		/-1\		True\			13\			5\	_FloatSlider\	1567.729\	1826.417\	192\	95\		/Vertical\		/2\		/2427\		True\			13\			6\	Abs\	1447.506\	862.1904\	192\	135\		True\			4\			6\		True\			9\			0\	Value1\	746.395\	837.3018\	192\	95\		/0.5\		True\			10\			2\	Sub\	1129.728\	755.5238\	192\	215\		/4\		True\			7\			1\		True\			2\			0\		True\			8\			0\		False\			null\			null\	Sub\	1069.728\	1202.189\	192\	215\		/4\		True\			11\			1\		True\			2\			1\		True\			8\			0\		False\			null\			null\	Abs\	1376.395\	1095.522\	192\	135\		True\			12\			6\		True\			10\			0\	Compare\	3311.844\	936.9677\	192\	455\		True\			0\			3\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			11\			0\		True\			15\			0\		True\			3\			0\		True\			4\			0\	Custom4\	1916.378\	1303.11\	192\	535\		/Assets/Post Processing Ultimate/Functions/Invert.hlsl\		/Invert\		/3\		/4\		/Value\		True\			14\			1\		True\			15\			1\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			5\			0\		True\			6\			0\		False\			null\			null\		False\			null\			null\		False\			null\			null\	Div\	2503.935\	1431.112\	192\	215\		/4\		True\			4\			7\		True\			13\			0\		True\			16\			0\		False\			null\			null\	Div\	2543.936\	1729.114\	192\	215\		/4\		True\			12\			7\		True\			13\			1\		True\			16\			0\		False\			null\			null\	Value1\	2159.934\	1867.115\	192\	95\		/2.0\		True\			15\			2

Shader "NTEC/Screen/Frame" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
			#include "../../Post Processing Ultimate/Functions/Invert.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half _Horizontal;
			uniform half _Vertical;

			half4 Frag (VaryingsDefault i) : SV_Target {
				half4 CameraOutput = 0.0;
				CameraOutput.rgb = (abs((i.texcoordStereo.y - 0.5)) > (Invert(half4(_Horizontal, _Vertical, 0.0, 0.0)).y / 2.0) ? 0.0 : (abs((i.texcoordStereo.x - 0.5)) > (Invert(half4(_Horizontal, _Vertical, 0.0, 0.0)).x / 2.0) ? 0.0 : SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb));
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}