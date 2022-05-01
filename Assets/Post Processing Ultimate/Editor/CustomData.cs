using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NTEC.PPU{
	[Serializable]
	public class CustomData : System.Object {
		
		public string Name = "";
		public int OutputSize = 0;
		public List<int> InputSize = new List<int>(); 
		public List<string> InputName = new List<string>(); 
	}
}
