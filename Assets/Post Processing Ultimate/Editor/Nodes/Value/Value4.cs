using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace NTEC.PPU{
	public class Value4 : VisualElement{
		
		private Color background = new Color(1, 0.25f, 0.25f, 0.25f);
		private Texture2D nodeBack;
		
		public Value4(){
			position = new Rect(32, 32, 192, 255);
			name = "Value4";
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			Values.Add("0.0");
			Values.Add("0.0");
			Values.Add("0.0");
			Values.Add("0.0");
		}
		
		public override void Show(){
			base.Show();
			if (nodeBack == null){
				nodeBack = CombineTextures(Resources.Load("Nodes/Top") as Texture2D, position.height, background);
			}
			GUI.DrawTexture(position, nodeBack);
			for (int i = 0; i < joints.Count; ++i){
				if (joints[i].type == 0){
					joints[i].coords = new Vector2(position.x - 48, 8 + position.y + (i + 1) * (40));
					joints[i].Show();
				} else {
					joints[i].coords = new Vector2(position.x + position.width + 24, 8 + position.y + (i + 1) * (40));
					joints[i].Show();
				}
			}
		}
		
		public override void ShowLabels(){
			base.ShowLabels();
			EditorGUI.LabelField(new Rect(position.x * scale, position.y * scale, position.width * scale, 40 * scale), new GUIContent(name, Tooltips.List(name)), style[0]);
			Values[0] = EditorGUI.FloatField(new Rect(position.x * scale, (position.y + 1 * (40) + 4) * scale, position.width * scale, 32 * scale), float.Parse(Values[0], CultureInfo.InvariantCulture), style[3]).ToString();
			Values[1] = EditorGUI.FloatField(new Rect(position.x * scale, (position.y + 2 * (40) + 4) * scale, position.width * scale, 32 * scale), float.Parse(Values[1], CultureInfo.InvariantCulture), style[3]).ToString();
			Values[2] = EditorGUI.FloatField(new Rect(position.x * scale, (position.y + 3 * (40) + 4) * scale, position.width * scale, 32 * scale), float.Parse(Values[2], CultureInfo.InvariantCulture), style[3]).ToString();
			Values[3] = EditorGUI.FloatField(new Rect(position.x * scale, (position.y + 4 * (40) + 4) * scale, position.width * scale, 32 * scale), float.Parse(Values[3], CultureInfo.InvariantCulture), style[3]).ToString();
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 5 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("XYZW", "All values"), style[2]);
		}
	}
}