//{"Values":["0","Detach","_MainTex","0",""]}
//\	Output\	1342\	298\	192\	295\		/3\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			1\			4\	Input4\	670\	200\	192\	295\		/Value\		False\			0\			0\		False\			0\			1\		False\			0\			2\		False\			null\			null\		True\			0\			4
//{"Name":"Detach","OutputSize":4,"InputSize":[4],"InputName":["Value"]}

half4 Detach(half4 Value){
	half4 Output = 0.0;
	Output = Value;
	return Output;
}