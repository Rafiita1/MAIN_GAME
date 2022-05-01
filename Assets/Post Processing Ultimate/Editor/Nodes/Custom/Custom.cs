using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTEC.PPU{
	public class Custom : VisualElement{
		
		private Color background = new Color(0.15f, 0.15f, 0.35f, 0.25f);
		private Texture2D nodeBack;
		private string label;
		private string tooltip;
		private string newFunction;
		private CustomData data = new CustomData();
		private ShaderSave saver = new ShaderSave();
		private List<Joint> disconnected = new List<Joint>();
		private List<string> inputsLabels = new List<string>();
		private List<string> inputsTooltips = new List<string>();
		private List<string> outputsLabels = new List<string>();
		private List<string> outputsTooltips = new List<string>();
 		
		public Custom(){
			position = new Rect(32, 32, 192, 135);
			name = "Custom";
			Values.Add("EMPTY");
			Values.Add("");
			Values.Add("");
			Values.Add("");
			Values.Add("");
		}
		
		public override void Refresh(){
			if (Values[0] != "EMPTY"){
				data = saver.CustomRead(System.IO.File.ReadAllText(Values[0]));
			} else {
				data = new CustomData();
			}
			Values[1] = data.Name;
			Values[2] = (data.OutputSize - 1).ToString();
			name = "Custom" + data.OutputSize;
			Values[3] = "";
			for (int i = 0; i < data.InputSize.Count; ++i){
				if (i == 0){
					Values[3] += data.InputSize[i].ToString();
				} else {
					Values[3] += "," + data.InputSize[i].ToString();
				}
			}
			Values[4] = "";
			for (int i = 0; i < data.InputName.Count; ++i){
				if (i == 0){
					Values[4] += data.InputName[i];
				} else {
					Values[4] += "," + data.InputName[i];
				}
			}
		}
		
		public override void Arrange(){
			if (Values[0] != "EMPTY"){
				data = saver.CustomRead(System.IO.File.ReadAllText(Values[0]));
			}
			if (joints.Count > 0 && Values[0] == "EMPTY"){
				joints.Clear();
				inputsLabels.Clear();
				inputsTooltips.Clear();
				outputsLabels.Clear();
				outputsTooltips.Clear();
				position.height = 135;
				name = "Custom";
			}
			if (joints.Count == 0 && Values[1] != "" || changed){
				name = Values[0] == "EMPTY" ? "Custom" : "Custom" + data.OutputSize;
				disconnected.Clear();
				for (int i = 0; i < joints.Count; ++i){
					disconnected.Add(joints[i].connections);
				}
				joints.Clear();
				position.height = 135;
				switch (Values[2]){
					case "0":
						joints.Add(new Joint(1, 4));
						position.height += 1 * 40;
					break;
					case "1":
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						position.height += 3 * 40;
					break;
					case "2":
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						position.height += 4 * 40;
					break;
					case "3":
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						position.height += 5 * 40;
					break;
				}
				for (int i = 0; i < data.InputSize.Count; ++i){
					switch ((data.InputSize[i] - 1).ToString()){
						case "0":
							joints.Add(new Joint(0, 4));
							position.height += 1 * 40;
						break;
						case "1":
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count + 1);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 2);
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 3);
							joints[joints.Count - 1].size = 2;
							position.height += 3 * 40;
						break;
						case "2":
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count + 2);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count + 1);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 2);
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 3);
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 4);
							joints[joints.Count - 1].size = 3;
							position.height += 4 * 40;
						break;
						case "3":
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count + 3);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count + 2);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count + 1);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 2);
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 3);
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 4);
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 5);
							joints[joints.Count - 1].size = 4;
							position.height += 5 * 40;
						break;
					}
				}
				for (int i = 0; i < disconnected.Count; ++i){
					if (i < joints.Count){
						joints[i].connections = disconnected[i];
						if (disconnected[i] != null){
							disconnected[i].connections = joints[i];
						}
					} else {
						if (disconnected[i] != null){
							disconnected[i].connected = false;
							disconnected[i].connections = null;
						}
					}
				}
			}
		}
		
		public override void Show(){
			base.Show();
			if (nodeBack == null){
				nodeBack = CombineTextures(Resources.Load("Nodes/Top") as Texture2D, position.height, background);
			}
			if (Values[0] != "EMPTY"){
				data = saver.CustomRead(System.IO.File.ReadAllText(Values[0]));
			}
			if (joints.Count > 0 && Values[0] == "EMPTY"){
				joints.Clear();
				inputsLabels.Clear();
				inputsTooltips.Clear();
				outputsLabels.Clear();
				outputsTooltips.Clear();
				position.height = 135;
				name = "Custom";
				nodeBack = CombineTextures(Resources.Load("Nodes/Top") as Texture2D, position.height, background);
			}
			if (joints.Count == 0 && Values[1] != "" || changed){
				name = Values[0] == "EMPTY" ? "Custom" : "Custom" + data.OutputSize;
				disconnected.Clear();
				for (int i = 0; i < joints.Count; ++i){
					disconnected.Add(joints[i].connections);
				}
				joints.Clear();
				position.height = 135;
				switch (Values[2]){
					case "0":
						joints.Add(new Joint(1, 4));
						position.height += 1 * 40;
					break;
					case "1":
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						position.height += 3 * 40;
					break;
					case "2":
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						position.height += 4 * 40;
					break;
					case "3":
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						joints.Add(new Joint(1, 4));
						position.height += 5 * 40;
					break;
				}
				for (int i = 0; i < data.InputSize.Count; ++i){
					switch ((data.InputSize[i] - 1).ToString()){
						case "0":
							joints.Add(new Joint(0, 4));
							position.height += 1 * 40;
						break;
						case "1":
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count + 1);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 2);
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 3);
							joints[joints.Count - 1].size = 2;
							position.height += 3 * 40;
						break;
						case "2":
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count + 2);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count + 1);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 2);
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 3);
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 4);
							joints[joints.Count - 1].size = 3;
							position.height += 4 * 40;
						break;
						case "3":
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count + 3);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count + 2);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count + 1);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count);
							joints.Add(new Joint(0, 4));
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 2);
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 3);
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 4);
							joints[joints.Count - 1].prohibitions.Add(joints.Count - 5);
							joints[joints.Count - 1].size = 4;
							position.height += 5 * 40;
						break;
					}
				}
				for (int i = 0; i < disconnected.Count; ++i){
					if (i < joints.Count){
						joints[i].connections = disconnected[i];
						if (disconnected[i] != null){
							disconnected[i].connections = joints[i];
						}
					} else {
						if (disconnected[i] != null){
							disconnected[i].connected = false;
							disconnected[i].connections = null;
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
					joints[i].coords = new Vector2(position.x - 48, 8 + position.y + (i + 3) * (40));
					joints[i].Show();
				} else {
					joints[i].coords = new Vector2(position.x + position.width + 24, 8 + position.y + (i + 3) * (40));
					joints[i].Show();
				}
			}
		}
		
		public override void ShowLabels(){
			base.ShowLabels();
			EditorGUI.LabelField(new Rect(position.x * scale, position.y * scale, position.width * scale, 40 * scale), new GUIContent(name, Tooltips.List(name)), style[0]);
			EditorGUI.LabelField(new Rect(position.x * scale, (position.y + 1 * 40) * scale, position.width * scale, 40 * scale), new GUIContent(Values[0], "HLSL Function path"), Values[0] == "EMPTY" ? style[7] : style[2]);
			if (inputsLabels.Count == 0 && Values[1] != "" || changed){
				inputsLabels.Clear();
				inputsTooltips.Clear();
				outputsLabels.Clear();
				outputsTooltips.Clear();
				switch (Values[2]){
					case "0":
						outputsLabels.Add("Output");
						outputsTooltips.Add("Output value");
					break;
					case "1":
						outputsLabels.Add("X Value");
						outputsTooltips.Add("Output X");
						outputsLabels.Add("Y Value");
						outputsTooltips.Add("Output Y");
						outputsLabels.Add("Output");
						outputsTooltips.Add("Output value");
					break;
					case "2":
						outputsLabels.Add("X Value");
						outputsTooltips.Add("Output X");
						outputsLabels.Add("Y Value");
						outputsTooltips.Add("Output Y");
						outputsLabels.Add("Z Value");
						outputsTooltips.Add("Output Z");
						outputsLabels.Add("Output");
						outputsTooltips.Add("Output value");
					break;
					case "3":
						outputsLabels.Add("X Value");
						outputsTooltips.Add("Output X");
						outputsLabels.Add("Y Value");
						outputsTooltips.Add("Output Y");
						outputsLabels.Add("Z Value");
						outputsTooltips.Add("Output Z");
						outputsLabels.Add("W Value");
						outputsTooltips.Add("Output W");
						outputsLabels.Add("Output");
						outputsTooltips.Add("Output value");
					break;
				}
				for (int i = 0; i < data.InputSize.Count; ++i){
					switch ((data.InputSize[i] - 1).ToString()){
						case "0":
							inputsLabels.Add(data.InputName[i]);
							inputsTooltips.Add("Input value");
						break;
						case "1":
							inputsLabels.Add("X " + data.InputName[i]);
							inputsTooltips.Add("Input X");
							inputsLabels.Add("Y " + data.InputName[i]);
							inputsTooltips.Add("Input Y");
							inputsLabels.Add(data.InputName[i]);
							inputsTooltips.Add("Input value");
						break;
						case "2":
							inputsLabels.Add("X " + data.InputName[i]);
							inputsTooltips.Add("Input X");
							inputsLabels.Add("Y " + data.InputName[i]);
							inputsTooltips.Add("Input Y");
							inputsLabels.Add("Z " + data.InputName[i]);
							inputsTooltips.Add("Input Z");
							inputsLabels.Add(data.InputName[i]);
							inputsTooltips.Add("Input value");
						break;
						case "3":
							inputsLabels.Add("X " + data.InputName[i]);
							inputsTooltips.Add("Input X");
							inputsLabels.Add("Y " + data.InputName[i]);
							inputsTooltips.Add("Input Y");
							inputsLabels.Add("Z " + data.InputName[i]);
							inputsTooltips.Add("Input Z");
							inputsLabels.Add("W " + data.InputName[i]);
							inputsTooltips.Add("Input W");
							inputsLabels.Add(data.InputName[i]);
							inputsTooltips.Add("Input value");
						break;
					}
				}
				nodeBack = CombineTextures(Resources.Load("Nodes/Top") as Texture2D, position.height, background);
				changed = false;
			}
			if (GUI.Button(new Rect(position.x * scale, (position.y + 2 * 40) * scale, position.width * scale * (Values[0] != "EMPTY" && Values[0] != null && Values[0] != "" ? 0.75f : 1), 40 * scale), new GUIContent("Select", "Select function"), style[6])){
				selected = false;
				newFunction = EditorUtility.OpenFilePanel("Open PPU function", "Assets/Post Processing Ultimate/Functions", "hlsl");
				if (newFunction != null && newFunction != ""){
					Values[0] = newFunction;
					Values[0] = Values[0].Substring(Values[0].IndexOf("Assets/"));
					Refresh();
				}
				changed = true;
			}
			if (Values[0] != "EMPTY" && Values[0] != null && Values[0] != "" && GUI.Button(new Rect((position.x + position.width * 0.75f) * scale, (position.y + 2 * 40) * scale, position.width * scale * 0.25f, 40 * scale), new GUIContent("X", "Remove function"), style[6])){
				data = new CustomData();
				for (int i = 1; i < Values.Count; ++i){
					Values[i] = "";
				}
				Values[0] = "EMPTY";
				selected = false;
			}
			for (int i = 0; i < outputsLabels.Count; ++i){
				EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + (i + 3) * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent(outputsLabels[i], outputsTooltips[i]), style[2]);
			}
			for (int i = 0; i < inputsLabels.Count; ++i){
				EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + (i + 3 + outputsLabels.Count) * (40) + 8) * scale, (position.width) * scale, 32 * scale), new GUIContent(inputsLabels[i], inputsTooltips[i]), style[1]);
			}
		}
	}
}