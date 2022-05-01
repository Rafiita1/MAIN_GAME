using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTEC.PPU{
	public class Output : VisualElement{
		
		private Color background = new Color(0.25f, 0.25f, 1, 0.25f);
		private Texture2D nodeBack;
		private string label;
		private string tooltip;
		private List<Joint> disconnected = new List<Joint>();
 		
		public Output(){
			position = new Rect(32, 32, 192, 135);
			name = "Output";
			joints.Add(new Joint(0, 4));
			options.Add("1");
			options.Add("2");
			options.Add("3");
			options.Add("4");
			Values.Add("0");
		}
		
		public override void Show(){
			base.Show();
			if (Int32.Parse(Values[0]) + (Int32.Parse(Values[0]) == 0 ? 1 : 2) != joints.Count){
				disconnected.Clear();
				for (int i = 0; i < joints.Count; ++i){
					disconnected.Add(joints[i].connections);
				}
				joints.Clear();
				position.height = 95;
				switch (Values[0]){
					case "0":
						joints.Add(new Joint(0, 4));
						position.height += 1 * 40;
					break;
					case "1":
						joints.Add(new Joint(0, 4));
						joints.Add(new Joint(0, 4));
						joints.Add(new Joint(0, 4));
						joints[0].prohibitions.Add(2);
						joints[1].prohibitions.Add(2);
						joints[2].prohibitions.Add(0);
						joints[2].prohibitions.Add(1);
						position.height += 3 * 40;
					break;
					case "2":
						joints.Add(new Joint(0, 4));
						joints.Add(new Joint(0, 4));
						joints.Add(new Joint(0, 4));
						joints.Add(new Joint(0, 4));
						joints[0].prohibitions.Add(3);
						joints[1].prohibitions.Add(3);
						joints[2].prohibitions.Add(3);
						joints[3].prohibitions.Add(0);
						joints[3].prohibitions.Add(1);
						joints[3].prohibitions.Add(2);
						position.height += 4 * 40;
					break;
					case "3":
						joints.Add(new Joint(0, 4));
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
						position.height += 5 * 40;
					break;
				}
				for (int i = 0; i < disconnected.Count; ++i){
					if (i < joints.Count){
						joints[i].connections = disconnected[i];
					} else {
						if (disconnected[i] != null){
							disconnected[i].connected = false;
						}
					}
				}
				nodeBack = CombineTextures(Resources.Load("Nodes/Top") as Texture2D, position.height, background);
			}
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
			Values[0] = EditorGUI.Popup(new Rect(position.x * scale, (position.y + 1 * (40) + 4) * scale, position.width * scale, 32 * scale), Int32.Parse(Values[0]), options.ToArray(), style[4]).ToString();                                                                              
			for (int i = 0; i < joints.Count; ++i){
				switch (i) {
					case 0:
						label = "X Value";
						tooltip = "Output X";
					break;
					case 1:
						label = "Y Value";
						tooltip = "Output Y";
					break;
					case 2:
						label = Values[0] == "1" ? "XY" : "Z Value";
						tooltip = Values[0] == "1" ? "Output XY" : "Output Z";
					break;
					case 3:
						label = Values[0] == "2" ? "XYZ" : "W Value";
						tooltip = Values[0] == "2" ? "Output XYZ" : "Output W";
					break;
					case 4:
						label = "XYZW";
						tooltip = "Output XYZW";
					break;
				}
				EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + (i + 2) * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent(label, tooltip), style[1]);
			}
		}
	}
}