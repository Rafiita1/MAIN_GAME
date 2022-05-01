//{"Values":["0","NTEC/Screen/Infrared","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":0.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Intensity","Effect intensity","0.3","0","1"],"serial":0,"unique":-1,"type":"FloatSliderField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None"]}
//\	CameraOutput\	3319.681\	-2393.923\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			6\			0\	CameraInput\	1169.682\	-2413.923\	192\	335\		True\			3\			1\		True\			3\			2\		True\			3\			3\		True\			6\			1\		False\			null\			null\		False\			null\			null\		True\			2\			2\	StereoUV\	799.6818\	-2437.923\	192\	175\		False\			null\			null\		False\			null\			null\		True\			1\			6\	Av\	1713.681\	-2173.922\	192\	255\		/5\		True\			4\			1\		True\			1\			0\		True\			1\			1\		True\			1\			2\		False\			null\			null\	Variable1\	1477.849\	-1533.144\	192\	175\		/0\		/0\		True\			22\			1\		True\			3\			0\	_FloatSlider\	2363.236\	-1869.31\	192\	95\		/Intensity\		/1\		/-1\		True\			6\			3\	Lerp\	2970.737\	-1969.311\	192\	215\		True\			0\			3\		True\			1\			3\		True\			7\			3\		True\			5\			0\	Compare\	2390.736\	-1691.811\	192\	455\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			6\			2\		False\			null\			null\		False\			null\			null\		True\			4\			0\		True\			10\			0\		True\			15\			0\		True\			8\			3\	Compare\	2942.737\	-388.8104\	192\	455\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			7\			9\		False\			null\			null\		False\			null\			null\		True\			4\			0\		True\			11\			0\		True\			14\			0\		True\			9\			3\	Compare\	2897.236\	575.1904\	192\	455\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			8\			9\		False\			null\			null\		False\			null\			null\		True\			4\			0\		True\			12\			0\		True\			13\			0\		True\			12\			0\	Value1\	1007.88\	-1263.954\	192\	95\		/0.33\		True\			19\			2\	Value1\	967.88\	1153.547\	192\	95\		/0.66\		True\			22\			2\	Value1\	1315.952\	817.6893\	192\	95\		/1.0\		True\			13\			2\	Lerp\	2358.879\	939.117\	192\	215\		True\			9\			8\		True\			21\			3\		True\			12\			0\		True\			23\			0\	Lerp\	2511.356\	112.9274\	192\	215\		True\			8\			8\		True\			18\			3\		True\			21\			3\		True\			20\			0\	Lerp\	3011.451\	-901.4539\	192\	215\		True\			7\			8\		False\			null\			null\		True\			18\			3\		True\			16\			0\	Mul\	2408.594\	-764.3111\	192\	215\		/4\		True\			15\			3\		True\			4\			0\		True\			17\			0\		False\			null\			null\	Value1\	836.0226\	874.5467\	192\	95\		/3.0\		True\			23\			2\	Value3\	2371.451\	-1061.454\	192\	215\		/0.0\		/0.0\		/1.0\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			14\			1\	Sub\	1976.975\	-125.3586\	192\	215\		/4\		True\			20\			1\		True\			4\			0\		True\			10\			0\		False\			null\			null\	Mul\	2140.975\	217.9748\	192\	215\		/4\		True\			14\			3\		True\			19\			0\		True\			17\			0\		False\			null\			null\	Value3\	1638.309\	409.3083\	192\	215\		/1.0\		/0.0\		/0.0\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			13\			1\	Sub\	1844.976\	954.3089\	192\	215\		/4\		True\			23\			1\		True\			4\			0\		True\			11\			0\		False\			null\			null\	Mul\	2072.975\	1206.309\	192\	215\		/4\		True\			13\			3\		True\			22\			0\		True\			17\			0\		False\			null\			null

Shader "NTEC/Screen/Infrared" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half _Intensity;

			half4 Frag (VaryingsDefault i) : SV_Target {
				half1 var0 = 0.0;
				half4 CameraOutput = 0.0;
				var0 = ((SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).r + SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).g + SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).b) / 3.0);
				CameraOutput.rgb = lerp(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb,(var0 <= 0.33 ? lerp(0.0,half3(0.0,0.0,1.0),(var0 * 3.0)) : (var0 <= 0.66 ? lerp(half3(0.0,0.0,1.0),half3(1.0,0.0,0.0),((var0 - 0.33) * 3.0)) : (var0 <= 1.0 ? lerp(half3(1.0,0.0,0.0),1.0,((var0 - 0.66) * 3.0)) : 1.0))),_Intensity);
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}