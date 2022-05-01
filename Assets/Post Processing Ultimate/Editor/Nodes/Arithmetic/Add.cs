using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTEC.PPU{
	public class Add : VisualElement{
		
		private Color background = new Color(0, 1, 1, 0.25f);
		private int counter = 0;
		private Texture2D nodeBack;
		
		public Add(){
			position = new Rect(32, 32, 192, 175);
			name = "Add";
			joints.Add(new Joint(1, 4));
			joints.Add(new Joint(0, 4));
			joints.Add(new Joint(0, 4));
			Values.Add("0");
		}
		
		public override void Show(){
			base.Show();
			if (nodeBack == null){
				nodeBack = CombineTextures(Resources.Load("Nodes/Top") as Texture2D, position.height, background);
			}
			for (int i = joints.Count - 1; i >= 0; --i){
				if (joints[i].connected){
					counter = i;
					break;
				}
			}
			if (joints.Count < counter + 2){
				for (int i = 0; i < counter + 2 - joints.Count; ++i){
					joints.Add(new Joint(0, 4));
				}
			}
			for (int i = joints.Count - 1; i >= 3; --i){
				if (!joints[i].connected && !joints[i - 1].connected){
					joints.RemoveAt(i);
				} else {
					break;
				}
			}
			position.height = 55 + joints.Count * 40;
			if (position.height != nodeBack.height){
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
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 1 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("Output", "Output value"), style[2]);
			for (int i = 1; i < joints.Count; ++i){
				EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + (i + 1) * (40) + 8) * scale, (position.width) * scale, 32 * scale), new GUIContent("Value", "Input value"), style[1]);
			}
			Values[0] = joints.Count.ToString();
		}
	}
}