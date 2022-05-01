//{"Values":["0","NTEC/Screen/Fog","_MainTex","0",""]}|{"position":{"serializedVersion":"2","x":0.0,"y":0.0,"width":212.0,"height":89.0},"name":"Color","selected":false,"Values":["Color","Fog color","0.7843137","0.7843137","0.7843137","0.7843137"],"serial":0,"unique":3124,"type":"ColorField"}|{"position":{"serializedVersion":"2","x":0.0,"y":106.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Intensity","Fog visibility","0.5","0","1"],"serial":1,"unique":2153,"type":"FloatSliderField"}|{"position":{"serializedVersion":"2","x":0.0,"y":232.0,"width":212.0,"height":109.0},"name":"FloatSlider","selected":false,"Values":["Distance","Distance at which fog should appear","1","0","1"],"serial":2,"unique":-1,"type":"FloatSliderField"}|{"position":{"serializedVersion":"2","x":0.0,"y":358.0,"width":212.0,"height":89.0},"name":"Float","selected":false,"Values":["Height","Fog height","3"],"serial":3,"unique":147,"type":"FloatField"}|{"tempTextures":0,"passes":[{"position":{"serializedVersion":"2","x":0.0,"y":36.0,"width":212.0,"height":16.0},"InputLabels":["Game"],"OutputLabels":["Screen"],"PassLabels":["0"],"VariableLabels":["None"],"Input":0,"Output":0,"Pass":0,"Iterations":1,"Variable":0}],"passOptions":["0"],"inputOptions":["Game"],"outputOptions":["Screen"],"variableOptions":["None"]}
//\	CameraOutput\	2322.017\	872.1846\	192\	215\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			10\			0\	StereoUV\	-664.6865\	1167.422\	192\	175\		False\			null\			null\		False\			null\			null\		True\			3\			6\	CameraDepth\	-228.772\	1435.269\	192\	215\		True\			13\			1\		False\			null\			null\		False\			null\			null\		True\			1\			2\	CameraInput\	697.5967\	1076.734\	192\	335\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			9\			1\		False\			null\			null\		False\			null\			null\		True\			1\			2\	Mul\	1644.578\	1341.829\	192\	255\		/5\		True\			8\			1\		True\			7\			0\		True\			5\			0\		True\			21\			0\		False\			null\			null\	_FloatSlider\	1530.533\	1980.593\	192\	95\		/Intensity\		/1\		/2153\		True\			4\			2\	_FloatSlider\	27.17198\	1986.441\	192\	95\		/Distance\		/2\		/-1\		True\			15\			2\	Mul\	1236.745\	1563.455\	192\	215\		/4\		True\			4\			1\		True\			22\			0\		True\			18\			0\		False\			null\			null\	Saturate\	2043.953\	1507.675\	192\	135\		True\			24\			1\		True\			4\			0\	Saturate\	1434.408\	838.5518\	192\	135\		True\			10\			1\		True\			3\			3\	Lerp\	1946.682\	833.9617\	192\	215\		True\			0\			3\		True\			9\			0\		True\			23\			0\		True\			24\			0\	WorldPosition\	-133.525\	2185.482\	192\	215\		False\			null\			null\		True\			19\			1\		False\			null\			null\		False\			null\			null\	_Float\	80.36461\	2546.314\	192\	95\		/Height\		/1\		/147\		True\			17\			2\	Linear01Depth\	131.1966\	1646.041\	192\	135\		True\			14\			1\		True\			2\			0\	Sub\	663.9347\	1559.056\	192\	215\		/4\		True\			22\			1\		True\			13\			0\		True\			15\			0\		False\			null\			null\	Sub\	428.3797\	1812.389\	192\	215\		/4\		True\			14\			2\		True\			16\			0\		True\			6\			0\		False\			null\			null\	Value1\	-213.842\	1830.167\	192\	95\		/1.0\		True\			17\			1\	Sub\	710.6017\	2314.61\	192\	215\		/4\		True\			18\			2\		True\			16\			0\		True\			12\			0\		False\			null\			null\	Sub\	981.7136\	1863.5\	192\	215\		/4\		True\			7\			2\		True\			19\			0\		True\			17\			0\		False\			null\			null\	Mul\	517.2687\	2147.942\	192\	215\		/4\		True\			18\			1\		True\			11\			1\		True\			20\			0\		False\			null\			null\	Value1\	135.0466\	2343.498\	192\	95\		/-1.0\		True\			19\			2\	Value1\	1355.047\	1836.834\	192\	95\		/2.0\		True\			4\			3\	Saturate\	961.7136\	1412.39\	192\	135\		True\			7\			1\		True\			14\			0\	_Color\	1320\	1046.667\	192\	255\		/Color\		/1\		/3124\		True\			10\			2\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			24\			2\	Mul\	2351.111\	1288.889\	192\	215\		/4\		True\			10\			3\		True\			8\			0\		True\			23\			4\		False\			null\			null

Shader "NTEC/Screen/Fog" {

	SubShader {
		Cull Off ZWrite Off ZTest Always

		Pass {
			HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
			uniform float4x4 _ProjMat;
			
			struct VaryingsExtended
			{
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoordStereo : TEXCOORD1;
				float3 worldDirection : TEXCOORD2;
			};
			
			VaryingsExtended Vert(AttributesDefault v)
			{
				VaryingsExtended o;
				o.vertex = float4(v.vertex.xy, 0.0, 1.0);
				o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);
				#if UNITY_UV_STARTS_AT_TOP
					o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
				#endif
				o.texcoordStereo = TransformStereoScreenSpaceTex(o.texcoord, 1.0);
				float4 clip = float4(o.texcoord.xy * 2 - 1, 0.0, 1.0);
				o.worldDirection = mul(_ProjMat, clip) - _WorldSpaceCameraPos;
				return o;
			}

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
			uniform half4 _Color;
			uniform half _Intensity;
			uniform half _Distance;
			uniform half _Height;

			half4 Frag (VaryingsExtended i) : SV_Target {
				half4 CameraOutput = 0.0;
				CameraOutput.rgb = lerp(saturate(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo).rgb),_Color,(saturate(((saturate((Linear01Depth(SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoordStereo).x) - (1.0 - _Distance))) * (((i.worldDirection * LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoordStereo)) + _WorldSpaceCameraPos).y * -1.0) - (1.0 - _Height))) * _Intensity * 2.0)) * _Color.a));
				return CameraOutput;
			}
			ENDHLSL
		}
	}
}