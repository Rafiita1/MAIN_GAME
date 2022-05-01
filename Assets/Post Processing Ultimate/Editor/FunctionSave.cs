using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using System.IO;
using System.Globalization;

namespace NTEC.PPU{
	[System.Serializable]
	public class FunctionSave : ShaderSave{
		
		internal override string Save(List<List<VisualElement>> allElements, Settings settings, List<VisualProperty> properties, RenderQueue renderQueue){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			string project = "//";
			project += JsonUtility.ToJson(settings);
			for (int i = 0; i < allElements.Count; ++i){
				project += "\n//";
				for (int j = 0; j < allElements[i].Count; ++j){
					project += "\\\t" + allElements[i][j].name;
					project += "\\\t" + allElements[i][j].position.x + "\\\t" + allElements[i][j].position.y + "\\\t" + allElements[i][j].position.width + "\\\t" + allElements[i][j].position.height;
					for (int k = 0; k < allElements[i][j].Values.Count; ++k){
						project += "\\\t\t/" + allElements[i][j].Values[k];
					}
					for (int k = 0; k < allElements[i][j].joints.Count; ++k){
						project += "\\\t\t" + allElements[i][j].joints[k].connected;
						if (allElements[i][j].joints[k].connections != null){
							project += "\\\t\t\t" + ((int) allElements[i][j].joints[k].connections.serial.x) + "\\\t\t\t" +  ((int) allElements[i][j].joints[k].connections.serial.y);
						} else {
							project += "\\\t\t\tnull";
							project += "\\\t\t\tnull";
						}
					}
				}
			}
			return project;
		}
		
		internal override List<List<VisualElement>> ElementsRead(string database){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			List<List<VisualElement>> elements = new List<List<VisualElement>>();
			List<string> data = new List<string>();
			List<int> newLines = new List<int>();
			int numberOfLines;
			string newData = "";
			for (int i = 0; i < database.Length; ++i){
				if (database[i] != '\r'){
					newData += database[i];
				}
			}
			database = newData;
			for (int i = 0; i < database.Length; ++i){
				if (database[i] == '\n' && database[i + 1] == '\n'){
					database = database.Substring(0, i);
					break;
				}
			}
			if (database != ""){
				numberOfLines = 1;
				for (int i = 0; i < database.Length; ++i){
					if (database[i] == '\n'){
						++numberOfLines;
						newLines.Add(i);
					}
				}
			} else {
				numberOfLines = 0;
			}
			for (int i = 1; i < numberOfLines; ++i){
				if (i == numberOfLines - 1){
					data.Add(database.Substring(newLines[i - 1] + 3, database.Length - (newLines[i - 1] + 3)));
				} else {
					data.Add(database.Substring(newLines[i - 1] + 3, newLines[i] - (newLines[i - 1] + 3)));
				}
			}
			elements.Add(Elements(data[0]));
			return elements;
		}
		
		internal override List<VisualElement> Elements(string database){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			List<VisualElement> elements = new List<VisualElement>();
			List<string> lines = new List<string>();
			List<int> newLines = new List<int>();
			List<int> newElements = new List<int>();
			int numberOfLines;
			if (database != ""){
				numberOfLines = 1;
				for (int i = 0; i < database.Length; ++i){
					if (database[i] == '\\'){
						++numberOfLines;
						newLines.Add(i);
					}
				}
			} else {
				numberOfLines = 0;
			}
			for (int i = 0; i < numberOfLines; ++i){
				if (i == 0){
					lines.Add(database.Substring(0, newLines[0]));
				} else if (i == numberOfLines - 1){
					lines.Add(database.Substring(newLines[i - 1] + 1, database.Length - (newLines[i - 1] + 1)));
				} else {
					lines.Add(database.Substring(newLines[i - 1] + 1, newLines[i] - (newLines[i - 1] + 1)));
				}
			}
			for (int i = 1; i < lines.Count; ++i){
				if (i == 1 || (lines[i][1] != '\t' && lines[i - 1].Length > 2 && (lines[i - 1][2] == '\t' || lines[i - 1][2] == '/'))){
					elements.Add(Creator.List(lines[i].Substring(1)));
					newElements.Add(i);
				}
			}
			for (int i = 0; i < elements.Count; ++i){
				elements[i].position.x = float.Parse(lines[newElements[i] + 1].Substring(1), CultureInfo.InvariantCulture);
				elements[i].position.y = float.Parse(lines[newElements[i] + 2].Substring(1), CultureInfo.InvariantCulture);
				for (int j = 0; j < elements[i].Values.Count; ++j){
					elements[i].Values[j] = lines[newElements[i] + 5 + j].Substring(3);
				}
				if (elements[i].name == "Add" || elements[i].name == "Av" || elements[i].name == "Sub" || elements[i].name == "Mul" || elements[i].name == "Div" || elements[i].name == "Mod" || elements[i].name == "Output"){
					if (elements[i].name == "Output"){
						elements[i].joints.Clear();
						elements[i].position.height = 95;
						switch (elements[i].Values[0]){
							case "0":
								elements[i].joints.Add(new VisualElement.Joint(0, 4));
								elements[i].position.height += 1 * 40;
							break;
							case "1":
								elements[i].joints.Add(new VisualElement.Joint(0, 4));
								elements[i].joints.Add(new VisualElement.Joint(0, 4));
								elements[i].joints.Add(new VisualElement.Joint(0, 4));
								elements[i].joints[0].prohibitions.Add(2);
								elements[i].joints[1].prohibitions.Add(2);
								elements[i].joints[2].prohibitions.Add(0);
								elements[i].joints[2].prohibitions.Add(1);
								elements[i].position.height += 3 * 40;
							break;
							case "2":
								elements[i].joints.Add(new VisualElement.Joint(0, 4));
								elements[i].joints.Add(new VisualElement.Joint(0, 4));
								elements[i].joints.Add(new VisualElement.Joint(0, 4));
								elements[i].joints.Add(new VisualElement.Joint(0, 4));
								elements[i].joints[0].prohibitions.Add(3);
								elements[i].joints[1].prohibitions.Add(3);
								elements[i].joints[2].prohibitions.Add(3);
								elements[i].joints[3].prohibitions.Add(0);
								elements[i].joints[3].prohibitions.Add(1);
								elements[i].joints[3].prohibitions.Add(2);
								elements[i].position.height += 4 * 40;
							break;
							case "3":
								elements[i].joints.Add(new VisualElement.Joint(0, 4));
								elements[i].joints.Add(new VisualElement.Joint(0, 4));
								elements[i].joints.Add(new VisualElement.Joint(0, 4));
								elements[i].joints.Add(new VisualElement.Joint(0, 4));
								elements[i].joints.Add(new VisualElement.Joint(0, 4));
								elements[i].joints[0].prohibitions.Add(4);
								elements[i].joints[1].prohibitions.Add(4);
								elements[i].joints[2].prohibitions.Add(4);
								elements[i].joints[3].prohibitions.Add(4);
								elements[i].joints[4].prohibitions.Add(0);
								elements[i].joints[4].prohibitions.Add(1);
								elements[i].joints[4].prohibitions.Add(2);
								elements[i].joints[4].prohibitions.Add(3);
								elements[i].position.height += 5 * 40;
							break;
						}
						for (int j = 0; j < elements[i].joints.Count; ++j){
						elements[i].joints[j].connected = bool.Parse(lines[newElements[i] + 5 + elements[i].Values.Count + j * 3].Substring(2));
							if (!lines[newElements[i] + 6 + elements[i].Values.Count + j * 3].Contains("null")){
								elements[i].joints[j].connections = elements[(int) float.Parse(lines[newElements[i] + 6  + elements[i].Values.Count + j * 3].Substring(3), CultureInfo.InvariantCulture)].joints[(int) float.Parse(lines[newElements[i] + 7 + elements[i].Values.Count + j * 3].Substring(3), CultureInfo.InvariantCulture)];
							}
						}
					} else {
						elements[i].joints.RemoveRange(1, elements[i].joints.Count - 1);
						for (int j = 1; j < (int) float.Parse(elements[i].Values[0], CultureInfo.InvariantCulture); ++j){
							elements[i].joints.Add(new VisualElement.Joint(0, 4));
							elements[i].joints[j].connected = bool.Parse(lines[newElements[i] + 5 + elements[i].Values.Count + j * 3].Substring(2));
							if (!lines[newElements[i] + 6 + elements[i].Values.Count + j * 3].Contains("null")){
								elements[i].joints[j].connections = elements[(int) float.Parse(lines[newElements[i] + 6 + elements[i].Values.Count + j * 3].Substring(3), CultureInfo.InvariantCulture)].joints[(int) float.Parse(lines[newElements[i] + 7 + elements[i].Values.Count + j * 3].Substring(3), CultureInfo.InvariantCulture)];
							}
						}
					}
				} else {
					for (int j = 0; j < elements[i].joints.Count; ++j){
					elements[i].joints[j].connected = bool.Parse(lines[newElements[i] + 5 + elements[i].Values.Count + j * 3].Substring(2));
						if (!lines[newElements[i] + 6 + elements[i].Values.Count + j * 3].Contains("null")){
							elements[i].joints[j].connections = elements[(int) float.Parse(lines[newElements[i] + 6  + elements[i].Values.Count + j * 3].Substring(3), CultureInfo.InvariantCulture)].joints[(int) float.Parse(lines[newElements[i] + 7 + elements[i].Values.Count + j * 3].Substring(3), CultureInfo.InvariantCulture)];
						}
					}
				}
			}
			return elements;
		}
	}
}
