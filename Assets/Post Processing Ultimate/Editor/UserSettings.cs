using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NTEC.PPU{
	[Serializable]
	public class UserSettings : System.Object {
		
		public bool Flow = true;
		public bool SwitchMMB = false;
		public int Starts = 0;
		public bool AutoSave = true;
	}
}
