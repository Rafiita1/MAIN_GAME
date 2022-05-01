using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTEC.PPU{
	public class VarLoop4 : VisualElement{
		
		private Color background = new Color(0.5f, 0, 0.75f, 0.25f);
		private Texture2D nodeBack;
		internal List<string> options1 = new List<string>();
		
		public VarLoop4(){
			position = new Rect(32, 32, 192, 135 + 8 * 40);
			name = "VarLoop4";
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(0, 4));
			joints.Add(new Joint(0, 4));
			joints.Add(new Joint(0, 4));
			joints.Add(new Joint(0, 4));
			joints[0].prohibitions.Add(4);
			joints[1].prohibitions.Add(4);
			joints[2].prohibitions.Add(4);
			joints[3].prohibitions.Add(4);
			joints[4].prohibitions.Add(0);
			joints[4].prohibitions.Add(1);
			joints[4].prohibitions.Add(2);
			joints[4].prohibitions.Add(3);
			options1.Add("+");
			options1.Add("-");
			options1.Add("*");
			options1.Add("÷");
			options1.Add("%");
			Values.Add("0");
			Values.Add("x");
			Values.Add("0");
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
			Values[0] = EditorGUI.Popup(new Rect(position.x * scale, (position.y + 1 * (40) + 4) * scale, position.width * scale, 32 * scale), Int32.Parse(Values[0]), options.ToArray(), style[4]).ToString();
			EditorGUI.LabelField(new Rect(position.x * scale, position.y * scale, position.width * scale, 40 * scale), new GUIContent(name, Tooltips.List(name)), style[0]);               
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 2 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("X Value", "First output value"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 3 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("Y Value", "Second output value"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 4 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("Z Value", "Third output value"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 5 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("W Value", "Fourth output value"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 6 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("XYZW", "All output values"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 7 * (40) + 8) * scale, (position.width) * scale, 32 * scale), new GUIContent("Input", "Input value"), style[1]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 8 * (40) + 8) * scale, (position.width) * scale, 32 * scale), new GUIContent("Iterations", "Number of loops"), style[1]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 9 * (40) + 8) * scale, (position.width) * scale, 32 * scale), new GUIContent("Start", "Start iterator value"), style[1]);
			Values[2] = EditorGUI.Popup(new Rect(position.x * scale, (position.y + 10 * (40) + 4) * scale, position.width * scale, 32 * scale), Int32.Parse(Values[2]), options1.ToArray(), style[4]).ToString();
		}
	}
}