using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTEC.PPU{
	[System.Serializable]
	public class VisualProperty : System.Object{
	
		public Rect position;
		public string name;
		public bool selected;
		public List<string> Values = new List<string>();
		public int serial;
		public int unique = -1;
		public string type;
		internal List<GUIStyle> style = new List<GUIStyle>();
		internal Color sele = new Color(1, 1, 1, 0.5f);
		
		public virtual void Show(){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			if (style.Count == 0){
				style.Clear();
				style.Add(new GUIStyle());
				style[0].alignment = TextAnchor.MiddleCenter;
				style[0].fontSize = 12;
				style[0].font = Resources.Load("Fonts/FORCED SQUARE") as Font;
				style.Add(new GUIStyle(style[0]));
				style[1].alignment = TextAnchor.MiddleLeft;
				style[1].normal.textColor = new Color(0.7f, 0.7f, 0.7f);;
				style.Add(new GUIStyle(style[1]));
				style[2].alignment = TextAnchor.MiddleRight;
				style.Add(new GUIStyle());
				style[3].clipping = TextClipping.Clip;
				style[3].padding = new RectOffset(4, 4, 2, 0);
				style[3].normal.background = Resources.Load("Nodes/Field") as Texture2D;
				style[3].normal.textColor = new Color(0.7f, 0.7f, 0.7f);
				style[3].active.background = Resources.Load("Nodes/Field") as Texture2D;
				style[3].active.textColor = new Color(0.7f, 0.7f, 0.7f);
				style[3].focused.background = Resources.Load("Nodes/Field") as Texture2D;
				style[3].focused.textColor = new Color(0.7f, 0.7f, 0.7f);
				style.Add(new GUIStyle());
				style[4].clipping = TextClipping.Clip;
				style[4].border = new RectOffset(16, 16, 0, 0);
				style[4].padding = new RectOffset(4, 12, 2, 0);
				style[4].normal.background = Resources.Load("Nodes/PopupL") as Texture2D;
				style[4].normal.textColor =	new Color(0.7f, 0.7f, 0.7f);
				style[4].active.background = Resources.Load("Nodes/PopupL") as Texture2D;
				style[4].active.textColor = new Color(0.7f, 0.7f, 0.7f);
				style[4].focused.background = Resources.Load("Nodes/PopupL") as Texture2D;
				style[4].focused.textColor = new Color(0.7f, 0.7f, 0.7f);
			}
		}
	}
}
