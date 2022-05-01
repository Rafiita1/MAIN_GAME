using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTEC.PPU{
	[System.Serializable]
	public class VisualElement : System.Object{
	
		public Rect position;
		public string name;
		public List<Joint> joints = new List<Joint>();
		public bool selected;
		public List<string> Values = new List<string>();
		public float scale = 1;
		public float oldScale = 1;
		public List<string> options = new List<string>();
		public List<int> uniques = new List<int>();
		public bool uniqueChanged;
		public bool changed = true;
		internal List<GUIStyle> style = new List<GUIStyle>();
		internal Color sele = new Color(1, 0.5f, 0, 0.5f);
		internal Color alpha = new Color(0, 0, 0, 0.95f);
		internal Texture2D blurry;
		internal Texture2D output;
		internal Texture2D down;
		internal List<Color> getPixels = new List<Color>();
		internal List<Color> getBottom = new List<Color>();
		internal List<Color> singleLine = new List<Color>();
		internal List<Color> getMid = new List<Color>();
		internal Color gradient = new Color(0, 0, 0, 1);
		internal Color current;
		
		public virtual void Refresh(){}
		public virtual void Arrange(){}
		
		public virtual void Show(){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			if (blurry == null || position.height != blurry.height){
				blurry = BlurryShadow(position.height);
			}
			if (selected){
				EditorGUI.DrawRect(new Rect(position.x - 8, position.y - 8, position.width + 16, position.height + 16), sele);
			} else {
				GUI.DrawTexture(new Rect(position.x - 10, position.y - 10, position.width + 20, position.height + 20), blurry);
			}
		}
		
		public virtual void ShowLabels(){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			if (oldScale != scale || style.Count == 0){
				style.Clear();
				style.Add(new GUIStyle());
				style[0].alignment = TextAnchor.MiddleCenter;
				style[0].fontSize = (int) (24 * scale);
				style[0].font = Resources.Load("Fonts/FORCED SQUARE") as Font;
				style.Add(new GUIStyle(style[0]));
				style[1].alignment = TextAnchor.MiddleLeft;
				style[1].normal.textColor = Color.white;
				style.Add(new GUIStyle(style[1]));
				style[2].alignment = TextAnchor.MiddleRight;
				style[2].clipping = TextClipping.Clip;
				style.Add(new GUIStyle());
				style[3].fontSize = (int) (20 * scale);
				style[3].clipping = TextClipping.Clip;
				style[3].padding = new RectOffset((int) (8 * scale), (int) (8 * scale), (int) (4.5f * scale), (int) (0 * scale));
				style[3].normal.background = Resources.Load("Nodes/Field") as Texture2D;
				style[3].normal.textColor = new Color(0.7f, 0.7f, 0.7f);
				style[3].active.background = Resources.Load("Nodes/Field") as Texture2D;
				style[3].active.textColor = new Color(0.7f, 0.7f, 0.7f);
				style[3].focused.background = Resources.Load("Nodes/Field") as Texture2D;
				style[3].focused.textColor = new Color(0.7f, 0.7f, 0.7f);
				style.Add(new GUIStyle());
				style[4].fontSize = (int) (20 * scale);
				style[4].clipping = TextClipping.Clip;
				style[4].padding = new RectOffset((int) (8 * scale), (int) (24 * scale), (int) (4.5f * scale), (int) (0 * scale));
				style[4].normal.background = Resources.Load("Nodes/Popup") as Texture2D;
				style[4].normal.textColor =	new Color(0.7f, 0.7f, 0.7f);
				style[4].active.background = Resources.Load("Nodes/Popup") as Texture2D;
				style[4].active.textColor = new Color(0.7f, 0.7f, 0.7f);
				style[4].focused.background = Resources.Load("Nodes/Popup") as Texture2D;
				style[4].focused.textColor = new Color(0.7f, 0.7f, 0.7f);
				style.Add(new GUIStyle(style[0]));
				style[5].normal.textColor = Color.white;
				style[5].active.textColor = Color.white;
				style[5].focused.textColor = Color.white;
				style.Add(new GUIStyle(GUI.skin.button));
				style[6].fontSize = (int) (20 * scale);
				style[6].font = Resources.Load("Fonts/FORCED SQUARE") as Font;
				style[6].focused.textColor = Color.white;
				style[6].normal.textColor = Color.white;
				style[6].active.textColor = Color.white;
				style[6].hover.textColor = Color.white;
				style[6].alignment = TextAnchor.MiddleCenter;
				style[6].focused.background = Resources.Load("Buttons/GeneralOff") as Texture2D;
				style[6].normal.background = Resources.Load("Buttons/GeneralOff") as Texture2D;
				style[6].active.background = Resources.Load("Buttons/GeneralExe") as Texture2D;
				style[6].hover.background = Resources.Load("Buttons/GeneralOn") as Texture2D;
				style.Add(new GUIStyle(style[2]));
				style[7].alignment = TextAnchor.MiddleCenter;
				oldScale = scale;
			}
		}
		
		[System.Serializable]
		public class Joint : System.Object{
			public Vector2 coords;
			public Joint connections;
			public int type;
			public int color;
			public Vector2 serial;
			public bool connected;
			public List<int> prohibitions = new List<int>();
			public int size = 1;
			private static List<List<Texture2D>> icons = new List<List<Texture2D>>();
			
			public Joint(int side, int tint){
				type = side;
				color = tint;
			}
			
			public void Show(){
				if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
					System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
				}
				if (icons.Count == 0){
					icons.Add(new List<Texture2D>());
					icons.Add(new List<Texture2D>());
					icons[0].Add(Resources.Load("Joints/LRE") as Texture2D);
					icons[0].Add(Resources.Load("Joints/LGE") as Texture2D);
					icons[0].Add(Resources.Load("Joints/LBE") as Texture2D);
					icons[0].Add(Resources.Load("Joints/LWE") as Texture2D);
					icons[0].Add(Resources.Load("Joints/LCE") as Texture2D);
					icons[0].Add(Resources.Load("Joints/LRU") as Texture2D);
					icons[0].Add(Resources.Load("Joints/LGU") as Texture2D);
					icons[0].Add(Resources.Load("Joints/LBU") as Texture2D);
					icons[0].Add(Resources.Load("Joints/LWU") as Texture2D);
					icons[0].Add(Resources.Load("Joints/LCU") as Texture2D);
					icons[1].Add(Resources.Load("Joints/RRE") as Texture2D);
					icons[1].Add(Resources.Load("Joints/RGE") as Texture2D);
					icons[1].Add(Resources.Load("Joints/RBE") as Texture2D);
					icons[1].Add(Resources.Load("Joints/RWE") as Texture2D);
					icons[1].Add(Resources.Load("Joints/RCE") as Texture2D);
					icons[1].Add(Resources.Load("Joints/RRU") as Texture2D);
					icons[1].Add(Resources.Load("Joints/RGU") as Texture2D);
					icons[1].Add(Resources.Load("Joints/RBU") as Texture2D);
					icons[1].Add(Resources.Load("Joints/RWU") as Texture2D);
					icons[1].Add(Resources.Load("Joints/RCU") as Texture2D);
				}
				if (type == 0){
					GUI.DrawTexture(new Rect(coords.x, coords.y, 24, 24), connections == null && !connected ? icons[type][color] : icons[type][color + 5]);
				} else {
					GUI.DrawTexture(new Rect(coords.x, coords.y, 24, 24), !connected ? icons[type][color] : icons[type][color + 5]);
				}
			}
		}
		
		public Texture2D CombineTextures(Texture2D texture, float height, Color tint){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			gradient = Color.black;
			gradient.a = 1;
			output = texture;
			down = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false, true);
			for (int y = 0; y < texture.height; ++y){
				for (int x = 0; x < texture.width; ++x){
					down.SetPixel(x, y, texture.GetPixel(x, y));
				}
			}
			down.Apply();
			getPixels = output.GetPixels().ToList();
			getPixels.Reverse();
			output = new Texture2D(192, (int) height, TextureFormat.RGBA32, false, true) {wrapMode = TextureWrapMode.Clamp};
			getMid = new List<Color>();
			getBottom = new List<Color>();
			height -= 80;
			if (height > 0){
				Texture2D middle = new Texture2D(192, (int) height, TextureFormat.RGBA32, false, true);
				for (int y = 0; y < middle.height; ++y){
					for (int x = 0; x < middle.width; ++x){
						middle.SetPixel(x, y, Color.white);
					}
				}
				middle.Apply();
				getMid = middle.GetPixels().ToList();
				getPixels.AddRange(getMid);
			}
			for (int y = 0; y < down.height; ++y){
				for (int x = 0; x < down.width; ++x){
					down.SetPixel(x, y, down.GetPixel(x, y));
				}
			}
			down.Apply();
			getBottom = down.GetPixels().ToList();
			getPixels.AddRange(getBottom);
			getPixels.Reverse();
			output.SetPixels(getPixels.ToArray());
			for (int y = 0; y < output.height - 40; ++y){
				for (int x = 0; x < output.width; ++x){
					current = output.GetPixel(x, y);
					current = Color.Lerp(current * gradient, current * tint, x / (2f * output.width) + y / (2f * (output.height - 40)));
					if (PlayerSettings.colorSpace == ColorSpace.Linear){
						current = current.linear;
					}
					output.SetPixel(x, y, current);
				}
			}
			output.Apply();
			return output;
		}
		
		public Texture2D BlurryShadow(float height){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			output = Resources.Load("Nodes/Bottom") as Texture2D;
			down = new Texture2D(192, (int) height);
			for (int y = 0; y < (int) height; ++y){
				for (int x = 0; x < 192; ++x){
					down.SetPixel(x, y, output.GetPixel(x, y));
				}
			}
			down.Apply();
			getPixels = output.GetPixels().ToList();
			getPixels.Reverse();
			output = new Texture2D(192, (int) height);
			getBottom = down.GetPixels().ToList();
			singleLine = new List<Color>();
			for (int i = getPixels.Count - 192; i < getPixels.Count; ++i){
				singleLine.Add(getPixels[i]);
			}
			height -= 80;
			if (height > 0){
				for (int i = 0; i < height; ++i){
					getPixels.AddRange(singleLine);
				}
			}
			getPixels.AddRange(getBottom);
			for (int i = 0; i < getPixels.Count; ++i){
				getPixels[i] = getPixels[i] * alpha;
			}
			output.SetPixels(getPixels.ToArray());
			output.Apply();
			return output;
		} 
	}
}
