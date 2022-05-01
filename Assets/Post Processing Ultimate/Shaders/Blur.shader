//{"Values":["0","NTEC/Screen/Blur","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":1.0,"width":212.0,"height":109.0},"name":"IntSlider","selected":false,"Values":["Iterations","Effect Iterations","4","1","16"],"serial":0,"unique":3980,"type":"IntSliderField"}|{"position":{"serializedVersion":"2","x":0.0,"y":120.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Intensity","Effect Intensity","0.5","0","1"],"serial":1,"unique":-1,"type":"FloatSliderField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None","Iterations"],"Input":0,"Output":0,"Pass":0,"Iterations":0,"Variable":1}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None","Iterations"]}
//\	CameraOutput\	3467.716\	449.8438\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			3\			0\	StereoUV\	1718.944\	471.6748\	192\	175\		False\			null\			null\		False\			null\			null\		True\			null\			null\	CameraInput\	2496.173\	434.9128\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			null\			null\		False\			null\			null\		False\			null\			null\		True\			11\			0\	Div\	3133.212\	678.9766\	192\	215\		/4\		True\			null\			null\		True\			5\			3\		True\			6\			0\		False\			null\			null\	Iterator\	915.8579\	890.6826\	192\	95\		True\			null\			null\	VarLoop3\	2808.935\	511.9927\	192\	415\		/0\		/0\		/0\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			null\			null\		True\			2\			3\		True\			6\			0\		False\			null\			null\	Value1\	1761.015\	1010.723\	192\	95\		/90.0\		True\			null\			null\	Custom2\	2209.352\	726.3179\	192\	335\		/Assets/Post Processing Ultimate/Functions/Append2.hlsl\		/Append2\		/1\		/1,1\		/Y,X\		False\			null\			null\		False\			null\			null\		True\			null\			null\		True\			15\			0\		True\			16\			0\	Div\	588.3965\	303.2225\	192\	215\		/4\		True\			null\			null\		True\			21\			0\		True\			9\			0\		False\			null\			null\	Value1\	628.9927\	1029.652\	192\	95\		/3.0\		True\			null\			null\	Floor\	970.5396\	347.032\	192\	135\		True\			null\			null\		True\			8\			0\	Add\	2203.118\	390.0878\	192\	215\		/4\		True\			null\			null\		True\			1\			2\		True\			19\			0\		False\			null\			null\	Value1\	934.1118\	679.2959\	192\	95\		/-0.003\		True\			null\			null\	Mod\	1209.47\	1184.77\	192\	215\		/4\		True\			null\			null\		True\			21\			0\		True\			9\			0\		False\			null\			null\	Value1\	1012.008\	1613.224\	192\	95\		/0.003\		True\			null\			null\	Add\	1884.592\	779.4126\	192\	215\		/4\		True\			null\			null\		True\			23\			0\		True\			17\			0\		False\			null\			null\	Add\	2061.375\	1134.97\	192\	215\		/4\		True\			null\			null\		True\			23\			0\		True\			18\			0\		False\			null\			null\	Mul\	1441.549\	364.8254\	192\	215\		/4\		True\			null\			null\		True\			10\			0\		True\			24\			0\		False\			null\			null\	Mul\	1722.777\	1133.359\	192\	215\		/4\		True\			null\			null\		True\			26\			0\		True\			24\			0\		False\			null\			null\	Mul\	2609.205\	998.9358\	192\	215\		/4\		True\			null\			null\		True\			20\			0\		True\			7\			2\		False\			null\			null\	_FloatSlider\	2328.139\	1204.65\	192\	95\		/Intensity\		/1\		/-1\		True\			null\			null\	Mod\	731.5854\	1161.318\	192\	215\		/4\		True\			null\			null\		True\			4\			0\		True\			22\			0\		False\			null\			null\	Value1\	447.4678\	1477.468\	192\	95\		/9.0\		True\			null\			null\	Mul\	1442.833\	734.1509\	192\	215\		/4\		True\			null\			null\		True\			12\			0\		True\			25\			0\		False\			null\			null\	Mul\	1457.477\	1348.793\	192\	215\		/4\		True\			null\			null\		True\			14\			0\		True\			25\			0\		False\			null\			null\	Div\	952.3569\	1346.769\	192\	215\		/4\		True\			null\			null\		True\			4\			0\		True\			22\			0\		False\			null\			null\	Floor\	1336.045\	1037.667\	192\	135\		True\			null\			null\		True\			13\			0

Shader "NTEC/Screen/Blur" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
			#include "../../Post Processing Ultimate/Functions/Append2.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			uniform half _Iterations;
			uniform half _Intensity;

			half4 Frag (VaryingsDefault i) : SV_Target {
				half IteratorVariable = 0.0;
				half3 var0 = 0.0;
				half4 CameraOutput = 0.0;
				for (IteratorVariable = 0.0; IteratorVariable < (0.0 + 90.0); ++IteratorVariable){ var0 += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, (i.texcoordStereo + (_Intensity * Append2(((-0.003 * (IteratorVariable / 9.0)) + (floor(((IteratorVariable % 9.0) / 3.0)) * (0.003 * (IteratorVariable / 9.0)))), ((-0.003 * (IteratorVariable / 9.0)) + (floor(((IteratorVariable % 9.0) % 3.0)) * (0.003 * (IteratorVariable / 9.0)))))))).rgb; }
				CameraOutput.rgb = (var0 / 90.0);
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}