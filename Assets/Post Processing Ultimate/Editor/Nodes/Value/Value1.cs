using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace NTEC.PPU{
	public class Value1 : VisualElement{
		
		private Color background = new Color(1, 0.25f, 0.25f, 0.25f);
		private Texture2D nodeBack;
		
		public Value1(){
			position = new Rect(32, 32, 192, 95);
			name = "Value1";
			joints.Add(new Joint(1, 4));
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
		}
	}
}