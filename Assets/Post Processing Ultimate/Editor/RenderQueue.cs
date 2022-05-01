using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NTEC.PPU{
	[Serializable]
	public class RenderQueue : System.Object {
		
		public int tempTextures;
		public List<VisualPass> passes = new List<VisualPass>();
		public List<string> passOptions = new List<string>();
		public List<string> inputOptions = new List<string>();
		public List<string> outputOptions = new List<string>();
		public List<string> variableOptions = new List<string>();
	}
}
