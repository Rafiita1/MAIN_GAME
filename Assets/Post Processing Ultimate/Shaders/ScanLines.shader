//{"Values":["0","NTEC/Screen/ScanLines","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":0.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Intensity","Effect intensity","0.5","0","1"],"serial":0,"unique":3352,"type":"FloatSliderField"}|{"position":{"serializedVersion":"2","x":0.0,"y":126.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Width","Line width","0.5","0","1"],"serial":1,"unique":-1,"type":"FloatSliderField"}|{"position":{"serializedVersion":"2","x":0.0,"y":252.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Gap","Gap between lines","0.5","0","1"],"serial":2,"unique":3249,"type":"FloatSliderField"}|{"tempTextures":1,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game","tempRT0"],"OutputLabels":["Screen","tempRT0"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game","tempRT0"],"outputOptions":["Screen","tempRT0"],"variableOptions":["None"]}
//\	CameraOutput\	4201.017\	-143.1889\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			12\			0\	CameraInput\	2322.267\	-91.61737\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			12\			1\		False\			null\			null\		False\			null\			null\		True\			2\			2\	StereoUV\	803.2183\	27.66783\	192\	175\		False\			null\			null\		True\			8\			1\		True\			1\			6\	Mul\	2907.55\	143.5789\	192\	215\		/4\		True\			12\			2\		True\			1\			3\		True\			16\			0\		False\			null\			null\	Abs\	2546.24\	505.8404\	192\	135\		True\			11\			1\		True\			5\			0\	Sin\	2238.147\	535.8403\	192\	135\		True\			4\			1\		True\			6\			0\	Mul\	1794.814\	479.1738\	192\	255\		/5\		True\			5\			1\		True\			8\			0\		True\			22\			0\		True\			7\			0\		False\			null\			null\	Value1\	1330.053\	727.9828\	192\	95\		/16.0\		True\			20\			1\	Add\	1117.434\	295.1252\	192\	215\		/4\		True\			6\			1\		True\			2\			1\		True\			9\			0\		False\			null\			null\	Time\	497.6317\	400.4052\	192\	255\		True\			8\			2\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\	Floor\	2938.344\	546.476\	192\	135\		True\			13\			1\		True\			11\			0\	Mul\	2724.453\	746.0192\	192\	215\		/4\		True\			13\			2\		True\			4\			0\		True\			20\			0\		False\			null\			null\	Lerp\	3254.776\	-162.4521\	192\	215\		True\			0\			3\		True\			1\			3\		True\			3\			0\		True\			15\			0\	Lerp\	3371.559\	779.5699\	192\	215\		True\			16\			1\		True\			10\			0\		True\			11\			0\		True\			14\			0\	Value1\	2966.558\	909.5686\	192\	95\		/0.5\		True\			13\			3\	_FloatSlider\	3698.558\	521.5704\	192\	95\		/Intensity\		/1\		/3352\		True\			12\			3\	Saturate\	3416.514\	492.2695\	192\	135\		True\			3\			2\		True\			13\			0\	_FloatSlider\	700.3241\	1054.365\	192\	95\		/Width\		/2\		/-1\		True\			19\			2\	Value1\	1020.324\	754.3649\	192\	95\		/1.1\		True\			null\			null\	Sub\	1246.039\	1017.222\	192\	215\		/4\		True\			20\			2\		True\			18\			0\		True\			17\			0\		False\			null\			null\	Mul\	1639.967\	774.722\	192\	215\		/4\		True\			11\			2\		True\			7\			0\		True\			19\			0\		False\			null\			null\	_FloatSlider\	1347.945\	533.4125\	192\	95\		/Gap\		/3\		/3249\		True\			22\			2\	Sub\	1709.254\	239.9599\	192\	215\		/4\		True\			6\			2\		True\			23\			0\		True\			21\			0\		False\			null\			null\	Value1\	1320\	191.1111\	192\	95\		/2.0\		True\			22\			1

Shader "NTEC/Screen/ScanLines" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half _Intensity;
			uniform half _Width;
			uniform half _Gap;

			half4 Frag (VaryingsDefault i) : SV_Target {
				half4 CameraOutput = 0.0;
				CameraOutput.rgb = lerp(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb,(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb * saturate(lerp(floor((abs(sin(((i.texcoordStereo.y + _Time.x) * (2.0 - _Gap) * 16.0))) * (16.0 * (1.1 - _Width)))),(abs(sin(((i.texcoordStereo.y + _Time.x) * (2.0 - _Gap) * 16.0))) * (16.0 * (1.1 - _Width))),0.5))),_Intensity);
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}