using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace NTEC.PPU {
	public class ReviewWindow : EditorWindow {
		
		internal void OnGUI(){
			var style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};
			EditorGUI.LabelField(new Rect(4, 4, position.width, 16), "You've used PPU over 30 times!", style);
			EditorGUI.LabelField(new Rect(4, 20, position.width, 16), "I'm so glad that it's useful for You.", style);
			EditorGUI.LabelField(new Rect(4, 36, position.width, 16), "Please consider writing a review on the Asset Store.", style);
		}
	}
}