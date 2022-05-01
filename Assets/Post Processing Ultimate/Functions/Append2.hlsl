//{"Values":["0","Append2","_MainTex","0",""]}
//\	Output\	1408\	194\	192\	215\		/1\		True\			1\			0\		True\			2\			0\		False\			null\			null\	Input1\	738\	88\	192\	135\		/X\		True\			0\			0\	Input1\	738\	328\	192\	135\		/Y\		True\			0\			0
//{"Name":"Append2","OutputSize":2,"InputSize":[1,1],"InputName":["Y","X"]}

half2 Append2(half1 Y, half1 X){
	half2 Output = 0.0;
	Output.x = X;
	Output.y = Y;
	return Output;
}