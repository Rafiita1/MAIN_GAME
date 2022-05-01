using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NTEC.PPU{
	[Serializable]
	public class Settings : System.Object {
		
		public List<string> Values = new List<string>();
	
		public Settings() {
			Values.Add("0");
			Values.Add("NTEC/Screen/MyShader");
			Values.Add("_MainTex");
			Values.Add("0");
			Values.Add("");
		}
	}
}
