using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTEC.PPU{
	public class BoolField : VisualProperty{
		
		private Texture2D nodeBack;
		private Texture2D nodeBlack;
		private string[] options = new string[] {"False", "True"};
		
		public BoolField(){
			position = new Rect(0, 0, 212, 89);
			type = "BoolField";
			name = "Bool";
			Values.Add("Bool_Name");
			Values.Add("Tooltip");
			Values.Add("false");
			sele = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		}
		
		public override void Show(){
			base.Show();
			if (unique == 0){
				unique = Random.Range(1, 4096);
			}
			if (nodeBack == null){
				nodeBack = Resources.Load("Nodes/Property") as Texture2D;
				nodeBlack = Resources.Load("Nodes/PropertyBack") as Texture2D;
			}
			EditorGUI.DrawRect(new Rect(position.x, position.y + 10, position.width, position.height - 10), sele);
			GUI.DrawTexture(new Rect(position.x, position.y + 10, position.width, position.height - 10), nodeBlack);
			GUI.DrawTexture(new Rect(position.x, position.y, position.width, 20), nodeBack);
			EditorGUI.LabelField(new Rect(position.x, position.y, position.width, 20), new GUIContent(name, "Bool property"), style[0]);
			Values[0] = EditorGUI.TextField(new Rect(position.x + 2, (position.y + 1 * (20) + 2), position.width - 4, 16), Values[0], style[3]).Replace(" ", "");
			Values[1] = EditorGUI.TextField(new Rect(position.x + 2, (position.y + 2 * (20) + 2), position.width - 4, 16), Values[1], style[3]);
			Values[2] = EditorGUI.Popup(new Rect(position.x + 2, (position.y + 3 * (20) + 2), position.width - 4, 16), bool.Parse(Values[2]) ? 1 : 0, options, style[4]) == 0 ? "false" : "true";
		}
	}
}