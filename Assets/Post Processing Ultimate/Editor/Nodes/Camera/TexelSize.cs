using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTEC.PPU{
	public class TexelSize : VisualElement{
		
		private Color background = new Color(0.25f, 0.25f, 1, 0.25f);
		private Texture2D nodeBack;
		
		public TexelSize(){
			position = new Rect(32, 32, 192, 295);
			name = "TexelSize";
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			options.Add("_MainTex");
			Values.Add(options[0]);
		}
		
		public override void Show(){
			base.Show();
			if (nodeBack == null){
				nodeBack = CombineTextures(Resources.Load("Nodes/Top") as Texture2D, position.height, background);
			}
			GUI.DrawTexture(position, nodeBack);
			for (int i = 0; i < joints.Count; ++i){
				if (joints[i].type == 0){
					joints[i].coords = new Vector2(position.x - 48, 8 + position.y + (i + 2) * (40));
					joints[i].Show();
				} else {
					joints[i].coords = new Vector2(position.x + position.width + 24, 8 + position.y + (i + 2) * (40));
					joints[i].Show();
				}
			}
		}
		
		public override void ShowLabels(){
			base.ShowLabels();
			EditorGUI.LabelField(new Rect(position.x * scale, position.y * scale, position.width * scale, 40 * scale), new GUIContent(name, Tooltips.List(name)), style[0]);                                                                                            
			Values[0] = options[EditorGUI.Popup(new Rect(position.x * scale, (position.y + 1 * (40) + 4) * scale, position.width * scale, 32 * scale), PopupPosition(), options.ToArray(), style[4])];
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 2 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("X Value", "Texel X value"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 3 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("Y Value", "Texel Y value"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 4 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("Z Value", "Texel Z value"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 5 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("W Value", "Texel W value"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 6 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("XYZW", "Texel size"), style[2]);
		}
		
		internal int PopupPosition(){
			for (int i = 0; i < options.Count; ++i){
				if (Values[0] == options[i]){
					return i;
				}
			}
			return 0;
		}
	}
}