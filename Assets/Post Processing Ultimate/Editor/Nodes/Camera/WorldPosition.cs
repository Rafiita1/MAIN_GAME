using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTEC.PPU{
	public class WorldPosition : VisualElement{
		
		private Color background = new Color(0.25f, 0.25f, 1, 0.25f);
		private Texture2D nodeBack;
		
		public WorldPosition(){
			position = new Rect(32, 32, 192, 215);
			name = "WorldPosition";
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
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
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 1 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("X Value", "Global transform pixel X position"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 2 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("Y Value", "Global transform pixel Y position"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 3 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("Z Value", "Global transform pixel Z position"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 4 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("XYZ", "Global transform pixel position"), style[2]);
		}
	}
}