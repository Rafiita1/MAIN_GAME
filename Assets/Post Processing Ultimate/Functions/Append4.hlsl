//{"Values":["0","Append4","_MainTex","0",""]}
//\	Output\	1408\	194\	192\	295\		/3\		True\			1\			0\		True\			2\			0\		True\			3\			0\		True\			4\			0\		False\			null\			null\	Input1\	738\	88\	192\	135\		/X\		True\			0\			0\	Input1\	738\	328\	192\	135\		/Y\		True\			0\			1\	Input1\	800\	560\	192\	135\		/Z\		True\			0\			2\	Input1\	1316\	636\	192\	135\		/W\		True\			0\			3
//{"Name":"Append4","OutputSize":4,"InputSize":[1,1,1,1],"InputName":["W","Z","Y","X"]}

half4 Append4(half1 W, half1 Z, half1 Y, half1 X){
	half4 Output = 0.0;
	Output.x = X;
	Output.y = Y;
	Output.z = Z;
	Output.w = W;
	return Output;
}