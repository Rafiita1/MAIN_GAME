//{"Values":["0","NTEC/Screen/Noise","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":0.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Intensity","Effect intenisty","0.5","0","1"],"serial":0,"unique":-1,"type":"FloatSliderField"}|{"position":{"serializedVersion":"2","x":0.0,"y":126.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Saturation","Noise saturation","0.5","0","1"],"serial":1,"unique":3552,"type":"FloatSliderField"}|{"tempTextures":1,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game","tempRT0"],"OutputLabels":["Screen","tempRT0"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":1,"Pass":0,"Iterations":1,"Variable":0},{"position":{"serializedVersion":"2","x":0.0,"y":72.0,"width":212.0,"height":16.0},"InputLabels":["Game","tempRT0"],"OutputLabels":["Screen","tempRT0"],"PassLabels":["0"],"VariableLabels":["None"],"Input":1,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game","tempRT0"],"outputOptions":["Screen","tempRT0"],"variableOptions":["None"]}
//\	CameraOutput\	4235.877\	176.6274\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			19\			0\	CameraInput\	2693.039\	146.1514\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			19\			1\		False\			null\			null\		False\			null\			null\		True\			2\			2\	StereoUV\	610.8955\	357.937\	192\	175\		True\			7\			1\		True\			13\			1\		True\			1\			6\	Mul\	3563.832\	444.2459\	192\	215\		/4\		True\			19\			2\		True\			1\			3\		True\			24\			0\		False\			null\			null\	Sin\	2843.516\	628.6907\	192\	135\		True\			23\			1\		True\			5\			0\	Mul\	2482.881\	548.0554\	192\	295\		/6\		True\			4\			1\		True\			10\			0\		True\			9\			0\		True\			6\			0\		True\			11\			0\		False\			null\			null\	Add\	2022.999\	626.7852\	192\	295\		/6\		True\			25\			4\		True\			7\			0\		True\			8\			0\		True\			12\			0\		True\			12\			1\		False\			null\			null\	Sin\	1579.942\	564.3302\	192\	135\		True\			11\			1\		True\			2\			0\	Sin\	1572.443\	761.8302\	192\	135\		True\			25\			5\		True\			13\			0\	Sub\	1989.191\	926.6683\	192\	295\		/6\		True\			5\			2\		True\			7\			0\		True\			8\			0\		True\			12\			0\		True\			12\			1\		False\			null\			null\	Value1\	2026.93\	478.5733\	192\	95\		/512.0\		True\			5\			1\	Div\	2458.239\	1111.194\	192\	255\		/5\		True\			5\			4\		True\			7\			0\		True\			8\			0\		True\			12\			0\		False\			null\			null\	Screen\	1335.381\	930.4783\	192\	255\		True\			11\			3\		True\			9\			4\		False\			null\			null\		True\			25\			6\		False\			null\			null\	Mul\	1029.95\	630.5493\	192\	255\		/5\		True\			8\			1\		True\			2\			1\		True\			14\			0\		True\			17\			0\		False\			null\			null\	Value1\	567.0935\	676.2637\	192\	95\		/4096.0\		True\			13\			2\	Abs\	509.9495\	1010.55\	192\	135\		True\			17\			1\		True\			16\			3\	SinTime\	152.8075\	807.6924\	192\	255\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			15\			1\		False\			null\			null\	Add\	887.0935\	964.8352\	192\	215\		/4\		True\			13\			3\		True\			15\			0\		True\			18\			0\		False\			null\			null\	Value1\	495.6644\	827.6924\	192\	95\		/1.0\		True\			17\			2\	Lerp\	3935.536\	227.4144\	192\	215\		True\			0\			3\		True\			1\			3\		True\			3\			0\		True\			20\			0\	_FloatSlider\	3835.266\	718.3651\	192\	95\		/Intensity\		/1\		/-1\		True\			19\			3\	_FloatSlider\	2655.556\	933.6195\	192\	95\		/Saturation\		/2\		/3552\		True\			22\			1\	Mul\	2938.771\	1073.62\	192\	215\		/4\		True\			23\			2\		True\			21\			0\		True\			25\			3\		False\			null\			null\	Add\	3122.581\	812.6675\	192\	215\		/4\		True\			24\			1\		True\			4\			0\		True\			22\			0\		False\			null\			null\	Saturate\	3517.057\	962.7625\	192\	135\		True\			3\			2\		True\			23\			0\	Custom3\	2545.127\	1460.065\	192\	415\		/Assets/Post Processing Ultimate/Functions/Append3.hlsl\		/Append3\		/2\		/1,1,1\		/Z,Y,X\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			22\			2\		True\			6\			0\		True\			8\			0\		True\			12\			3

Shader "NTEC/Screen/Noise" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
			#include "../../Post Processing Ultimate/Functions/Append3.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half _Intensity;
			uniform half _Saturation;

			half4 Frag (VaryingsDefault i) : SV_Target {
				half4 CameraOutput = 0.0;
				CameraOutput.rgb = lerp(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb,(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb * saturate((sin((512.0 * (sin(i.texcoordStereo.x) - sin((i.texcoordStereo.y * 4096.0 * (abs(_SinTime.w) + 1.0))) - _ScreenParams.x - _ScreenParams.y) * (sin(i.texcoordStereo.x) + sin((i.texcoordStereo.y * 4096.0 * (abs(_SinTime.w) + 1.0))) + _ScreenParams.x + _ScreenParams.y) * (sin(i.texcoordStereo.x) / sin((i.texcoordStereo.y * 4096.0 * (abs(_SinTime.w) + 1.0))) / _ScreenParams.x))) + (_Saturation * Append3((sin(i.texcoordStereo.x) + sin((i.texcoordStereo.y * 4096.0 * (abs(_SinTime.w) + 1.0))) + _ScreenParams.x + _ScreenParams.y), sin((i.texcoordStereo.y * 4096.0 * (abs(_SinTime.w) + 1.0))), _ScreenParams.w))))),_Intensity);
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}