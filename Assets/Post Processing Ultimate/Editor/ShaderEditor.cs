using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using System.IO;

namespace NTEC.PPU{
	[System.Serializable]
	public class ShaderEditor : PPUEditor{
	
		[MenuItem("Window/Post Processing Ultimate/Shader Editor")]
		private static void Init(){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			ShaderEditor window = (ShaderEditor) GetWindow(typeof(ShaderEditor));
			window.minSize = new Vector2(400, 100);
			window.position = new Rect(UnityEngine.Screen.currentResolution.width * 0.125f, UnityEngine.Screen.currentResolution.height * 0.125f, UnityEngine.Screen.currentResolution.width * 0.75f, UnityEngine.Screen.currentResolution.height * 0.75f);
			window.wantsMouseMove = true;
			window.wantsMouseEnterLeaveWindow = true;
			window.Show();
			window.titleContent = new GUIContent ("Shader Editor");
		}
	}
}