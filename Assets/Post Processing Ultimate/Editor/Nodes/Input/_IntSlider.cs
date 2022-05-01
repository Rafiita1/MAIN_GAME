using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTEC.PPU{
	public class _IntSlider : VisualElement{
		
		private Color background = new Color(0.5f, 0.5f, 0.5f, 0.25f);
		private Texture2D nodeBack;
		
		public _IntSlider(){
			position = new Rect(32, 32, 192, 95);
			name = "_IntSlider";
			joints.Add(new Joint(1, 4));
			options.Add("ZERO (0)");
			uniques.Add(0);
			Values.Add("0");
			Values.Add("0");
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
			if (uniqueChanged){
				uniqueChanged = false;
				Values[1] = "0";
				for (int i = 0; i < uniques.Count; ++i){
					if (Values[2] == uniques[i].ToString()){
						Values[1] = i.ToString();
					}
				}
			}
			Values[1] = EditorGUI.Popup(new Rect(position.x * scale, (position.y + 1 * (40) + 4) * scale, position.width * scale, 32 * scale), Int32.Parse(Values[1]), options.ToArray(), style[4]).ToString();
			try {
				Values[2] = uniques[Int32.Parse(Values[1])].ToString();
			} catch {}
		}
	}
}