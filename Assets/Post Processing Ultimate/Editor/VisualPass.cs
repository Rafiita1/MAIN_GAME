using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTEC.PPU{
	[System.Serializable]
	public class VisualPass : System.Object{
	
		public Rect position;
		public string[] InputLabels = new string[]{};
		public string[] OutputLabels = new string[]{};
		public string[] PassLabels = new string[]{};
		public string[] VariableLabels = new string[]{};
		public int Input = 0; 
		public int Output = 0; 
		public int Pass = 0; 
		public int Iterations = 1;
		public int Variable = 0;
		private List<GUIStyle> style = new List<GUIStyle>();
	
		public VisualPass(){
			position = new Rect(0, 0, 212, 24);
		}
		
		public void Show(){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			if (style.Count == 0){
				style.Add(new GUIStyle());
				style[0].fontSize = 10;
				style[0].clipping = TextClipping.Clip;
				style[0].padding = new RectOffset(4, 4, 2, 0);
				style[0].normal.background = Resources.Load("Nodes/Field") as Texture2D;
				style[0].normal.textColor = new Color(0.7f, 0.7f, 0.7f);
				style[0].active.background = Resources.Load("Nodes/Field") as Texture2D;
				style[0].active.textColor = new Color(0.7f, 0.7f, 0.7f);
				style[0].focused.background = Resources.Load("Nodes/Field") as Texture2D;
				style[0].focused.textColor = new Color(0.7f, 0.7f, 0.7f);
				style.Add(new GUIStyle());
				style[1].fontSize = 10;
				style[1].clipping = TextClipping.Clip;
				style[1].padding = new RectOffset(4, 10, 2, 0);
				style[1].normal.background = Resources.Load("Nodes/PopupS") as Texture2D;
				style[1].normal.textColor =	new Color(0.7f, 0.7f, 0.7f);
				style[1].active.background = Resources.Load("Nodes/PopupS") as Texture2D;
				style[1].active.textColor = new Color(0.7f, 0.7f, 0.7f);
				style[1].focused.background = Resources.Load("Nodes/PopupS") as Texture2D;
				style[1].focused.textColor = new Color(0.7f, 0.7f, 0.7f);
			}
			Input = (int) Mathf.Clamp(EditorGUI.Popup(new Rect(0, position.y, 50, 16), Input, InputLabels, style[1]), 0, InputLabels.Length - 1);
			Output = (int) Mathf.Clamp(EditorGUI.Popup(new Rect(54, position.y, 50, 16), Output, OutputLabels, style[1]), 0, OutputLabels.Length - 1);
			if (Input == Output && Input != 0){
				Output = 0;
			}
			Pass = (int) Mathf.Clamp(EditorGUI.Popup(new Rect(108, position.y, 50, 16), Pass, PassLabels, style[1]), 0, PassLabels.Length - 1);
			Iterations = (int) Mathf.Max(EditorGUI.IntField(new Rect(162, position.y, 50, 16), Iterations, style[0]), 0);
			Variable = (int) Mathf.Clamp(EditorGUI.Popup(new Rect(162, position.y + 18, 50, 16), Variable, VariableLabels, style[1]), 0, VariableLabels.Length - 1);
		}	
	}
}
