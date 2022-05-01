//{"Values":["0","Invert","_MainTex","0",""]}
//\	Output\	1598\	96\	192\	295\		/3\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			3\			0\	Input4\	656\	412\	192\	295\		/Value\		False\			null\			null\		False\			null\			null\		False\			null\			null\		False\			null\			null\		True\			0\			0\	Value1\	992\	282\	192\	95\		/1.0\		True\			0\			0\	Sub\	1330\	674\	192\	215\		/4\		True\			null\			null\		True\			2\			0\		True\			1\			4\		False\			null\			null
//{"Name":"Invert","OutputSize":4,"InputSize":[4],"InputName":["Value"]}

half4 Invert(half4 Value){
	half4 Output = 0.0;
	Output = (1.0 - Value);
	return Output;
}