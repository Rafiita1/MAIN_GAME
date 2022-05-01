using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTEC.PPU{
	public class Compare : VisualElement{
		
		private Color background = new Color(0.75f, 1, 0.15f, 0.15f);
		private Texture2D nodeBack;
		
		public Compare(){
			position = new Rect(32, 32, 192, 455);
			name = "Compare";
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(0, 4));
			joints.Add(new Joint(0, 4));
			joints.Add(new Joint(0, 4));
			joints.Add(new Joint(0, 4));
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
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 1 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("X > Y", "X greater than Y"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 2 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("X >= Y", "X greater or equal to Y"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 3 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("X < Y", "X smaller than Y"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 4 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("X <= Y", "X smaller or equal to Y"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 5 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("X == Y", "X equal to Y"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 6 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("X != Y", "X not equal to Y"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 7 * (40) + 8) * scale, (position.width) * scale, 32 * scale), new GUIContent("X Value", "The first value"), style[1]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 8 * (40) + 8) * scale, (position.width) * scale, 32 * scale), new GUIContent("Y Value", "The second value"), style[1]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 9 * (40) + 8) * scale, (position.width) * scale, 32 * scale), new GUIContent("True", "True value"), style[1]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 10 * (40) + 8) * scale, (position.width) * scale, 32 * scale), new GUIContent("False", "False value"), style[1]);
		}
	}
}