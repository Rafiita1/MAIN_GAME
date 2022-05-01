//{"Values":["0","Append3","_MainTex","0",""]}
//\	Output\	1433\	264.6667\	192\	255\		/2\		True\			1\			0\		True\			2\			0\		True\			3\			0\		False\			null\			null\	Input1\	1048\	185.3333\	192\	135\		/X\		True\			0\			0\	Input1\	979.6665\	363.6667\	192\	135\		/Y\		True\			0\			0\	Input1\	1123.333\	510.6666\	192\	135\		/Z\		True\			0\			2
//{"Name":"Append3","OutputSize":3,"InputSize":[1,1,1],"InputName":["Z","Y","X"]}

half3 Append3(half1 Z, half1 Y, half1 X){
	half3 Output = 0.0;
	Output.x = X;
	Output.y = Y;
	Output.z = Z;
	return Output;
}