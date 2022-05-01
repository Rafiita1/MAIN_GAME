using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTEC.PPU{
	public class SafeHDR : VisualElement{
		
		private Color background = new Color(1, 0.25f, 0.75f, 0.25f);
		private Texture2D nodeBack;
		
		public SafeHDR(){
			position = new Rect(32, 32, 192, 455);
			name = "SafeHDR";
			joints.Add(new Joint(1, 0));
			joints.Add(new Joint(1, 1));
			joints.Add(new Joint(1, 2));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 3));
			joints.Add(new Joint(0, 0));
			joints.Add(new Joint(0, 1));
			joints.Add(new Joint(0, 2));
			joints.Add(new Joint(0, 4));
			joints.Add(new Joint(0, 3));
			joints[5].prohibitions.Add(9);
			joints[6].prohibitions.Add(9);
			joints[7].prohibitions.Add(9);
			joints[8].prohibitions.Add(9);
			joints[9].prohibitions.Add(5);
			joints[9].prohibitions.Add(6);
			joints[9].prohibitions.Add(7);
			joints[9].prohibitions.Add(8);
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
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 1 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("Red", "Red output"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 2 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("Green", "Green output"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 3 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("Blue", "Blue output"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 4 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("Alpha", "Alpha output"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 5 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("Color", "Color output"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 6 * (40) + 8) * scale, (position.width) * scale, 32 * scale), new GUIContent("Red", "Red input"), style[1]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 7 * (40) + 8) * scale, (position.width) * scale, 32 * scale), new GUIContent("Green", "Green input"), style[1]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 8 * (40) + 8) * scale, (position.width) * scale, 32 * scale), new GUIContent("Blue", "Blue input"), style[1]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 9 * (40) + 8) * scale, (position.width) * scale, 32 * scale), new GUIContent("Alpha", "Alpha input"), style[1]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 10 * (40) + 8) * scale, (position.width) * scale, 32 * scale), new GUIContent("Color", "Color input"), style[1]);
		}
	}
}