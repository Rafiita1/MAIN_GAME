using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTEC.PPU{
	public class CameraInput : VisualElement{
		
		private Color background = new Color(0.25f, 0.25f, 1, 0.25f);
		private Texture2D nodeBack;
		
		public CameraInput(){
			position = new Rect(32, 32, 192, 335);
			name = "CameraInput";
			joints.Add(new Joint(1, 0));
			joints.Add(new Joint(1, 1));
			joints.Add(new Joint(1, 2));
			joints.Add(new Joint(1, 3));
			joints.Add(new Joint(0, 4));
			joints.Add(new Joint(0, 4));
			joints.Add(new Joint(0, 4));
			joints[4].prohibitions.Add(6);
			joints[5].prohibitions.Add(6);
			joints[6].prohibitions.Add(4);
			joints[6].prohibitions.Add(5);
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
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 1 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("Red", "Texture red channel"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 2 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("Green", "Texture green channel"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 3 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("Blue", "Texture blue channel"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 4 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("RGB", "All texture channels"), style[2]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 5 * (40) + 8) * scale, (position.width) * scale, 32 * scale), new GUIContent("X axis", "Texture X coordinates"), style[1]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 6 * (40) + 8) * scale, (position.width) * scale, 32 * scale), new GUIContent("Y axis", "Texture Y coordinates"), style[1]);
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 7 * (40) + 8) * scale, (position.width) * scale, 32 * scale), new GUIContent("XY", "Both texture coordinates"), style[1]);
		}
	}
}