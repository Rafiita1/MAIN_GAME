//{"Values":["0","NTEC/Screen/Splatter","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":0.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Intensity","Effect intensity","0","0","1"],"serial":0,"unique":2082,"type":"FloatSliderField"}|{"position":{"serializedVersion":"2","x":0.0,"y":126.0,"width":212.0,"height":69.0},"name":"Texture2D","selected":false,"Values":["Image","Situation texture"],"serial":1,"unique":-1,"type":"Texture2DField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None"]}
//\	CameraOutput\	1451.352\	-719.4456\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			4\			0\	CameraInput\	673.3528\	-701.4457\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			4\			1\		False\			null\			null\		False\			null\			null\		True\			2\			2\	StereoUV\	249.3528\	-573.4456\	192\	175\		False\			null\			null\		False\			null\			null\		True\			8\			1\	_Texture2D\	605.353\	-287.4456\	192\	375\		/Image\		/1\		/-1\		True\			4\			2\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			5\			1\		False\			null\			null\		False\			null\			null\		True\			2\			2\	Lerp\	1296.854\	-403.4457\	192\	215\		True\			0\			3\		True\			1\			3\		True\			3\			0\		True\			13\			0\	Mul\	1261.244\	-113.779\	192\	215\		/4\		True\			13\			1\		True\			3\			4\		True\			11\			0\		False\			null\			null\	_FloatSlider\	-161.2585\	11.22055\	192\	95\		/Intensity\		/1\		/2082\		True\			19\			1\	Value2\	-317.1474\	616.2188\	192\	175\		/0.5\		/0.5\		False\			null\			null\		False\			null\			null\		True\			8\			2\	Sub\	137.8528\	733.1635\	192\	215\		/4\		True\			9\			1\		True\			2\			2\		True\			7\			2\		False\			null\			null\	Abs\	443.1309\	910.3846\	192\	135\		True\			10\			9\		True\			8\			0\	Custom4\	1153.687\	379.8295\	192\	535\		/Assets/Post Processing Ultimate/Functions/Detach.hlsl\		/Detach\		/3\		/4\		/Value\		True\			15\			1\		True\			16\			1\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			9\			0\	Add\	2395.984\	49.67276\	192\	215\		/4\		True\			0\			3\		True\			12\			0\		True\			20\			0\		False\			null\			null\	Custom4\	751.4651\	137.0531\	192\	535\		/Assets/Post Processing Ultimate/Functions/Invert.hlsl\		/Invert\		/3\		/4\		/Value\		True\			11\			1\		False\			10\			1\		False\			10\			2\		False\			null\			null\		False\			null\			null\		True\			19\			0\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\	Saturate\	1800.63\	-72.11197\	192\	135\		True\			0\			0\		True\			5\			0\	Add\	1927.594\	713.4974\	192\	215\		/4\		True\			18\			1\		True\			15\			0\		True\			16\			0\		False\			null\			null\	Pow\	1605.325\	503.5287\	192\	175\		True\			14\			1\		True\			10\			0\		True\			17\			0\	Pow\	1537.825\	888.5287\	192\	175\		True\			14\			2\		True\			10\			1\		True\			17\			0\	Value2\	1180.324\	1073.529\	192\	175\		/2.0\		/1.0\		True\			20\			2\		True\			18\			2\		False\			null\			null\	Pow\	2286.714\	513.8065\	192\	175\		True\			20\			1\		True\			14\			0\		True\			17\			1\	Mul\	328.2366\	318.8545\	192\	215\		/4\		True\			12\			5\		True\			6\			0\		True\			17\			0\		False\			null\			null\	Mul\	2005.715\	248.5709\	192\	215\		/4\		True\			11\			2\		True\			18\			0\		True\			17\			0\		False\			null\			null

Shader "NTEC/Screen/Splatter" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
			#include "../../Post Processing Ultimate/Functions/Detach.hlsl"
			#include "../../Post Processing Ultimate/Functions/Invert.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half _Intensity;
			TEXTURE2D_SAMPLER2D(_Image, sampler_Image);

			half4 Frag (VaryingsDefault i) : SV_Target {
				half4 CameraOutput = 0.0;
				CameraOutput.rgb = lerp(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb,SAMPLE_TEXTURE2D(_Image, sampler_Image, i.texcoordStereo),saturate((SAMPLE_TEXTURE2D(_Image, sampler_Image, i.texcoordStereo).a * (Invert(half4((_Intensity * 2.0), 0.0, 0.0, 0.0)).x + (pow((pow(Detach(abs((half4(i.texcoordStereo,0.0,0.0) - half4(half2(0.5,0.5),0.0,0.0)))).x,2.0) + pow(Detach(abs((half4(i.texcoordStereo,0.0,0.0) - half4(half2(0.5,0.5),0.0,0.0)))).y,2.0)),1.0) * 2.0)))));
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}