using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;

namespace NTEC.PPU{
	public class ShaderSave {
		
		internal virtual string Save(List<List<VisualElement>> allElements, Settings settings, List<VisualProperty> properties, RenderQueue renderQueue){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			string project = "//";
			project += JsonUtility.ToJson(settings);
			for (int i = 0; i < properties.Count; ++i){
				project += "|" + JsonUtility.ToJson(properties[i]);
			}
			project += "|" + JsonUtility.ToJson(renderQueue);
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
		
		internal Settings SettingsRead(string database){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			Settings settings = new Settings();
			database = database.Substring(2, database.IndexOf("}") - 1);
			settings = JsonUtility.FromJson<Settings>(database);
			return settings;
		}
		
		internal CustomData CustomRead(string database){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			CustomData custom = new CustomData();
			List<string> lines = new List<string>();
			List<int> newLines = new List<int>();
			int numberOfLines;
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
					lines.Add(database.Substring(newLines[i - 1] + 1, database.Length - (newLines[i - 1] + 1)));
				} else {
					lines.Add(database.Substring(newLines[i - 1] + 1, newLines[i] - (newLines[i - 1] + 1)));
				}
			}
			database = lines[1].Substring(2);
			custom = JsonUtility.FromJson<CustomData>(database);
			return custom;
		}
		
		internal List<VisualProperty> PropertiesRead(string database){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			List<VisualProperty> basics = new List<VisualProperty>();
			List<VisualProperty> properties = new List<VisualProperty>();
			List<string> lines = new List<string>();
			List<int> newLines = new List<int>();
			int numberOfLines;
			if (database.IndexOf('|') > 0){
				database = database.Substring(database.IndexOf('|'), database.IndexOf('\n') - database.IndexOf('|'));
				if (database != ""){
					numberOfLines = 1;
					for (int i = 0; i < database.Length; ++i){
						if (database[i] == '|'){
							++numberOfLines;
							newLines.Add(i);
						}
					}
				} else {
					numberOfLines = 0;
				}
				for (int i = 1; i < numberOfLines; ++i){
					if (i == numberOfLines - 1){
						lines.Add(database.Substring(newLines[i - 1] + 1, database.Length - (newLines[i - 1] + 1)));
					} else {
						lines.Add(database.Substring(newLines[i - 1] + 1, newLines[i] - (newLines[i - 1] + 1)));
					}
				}
				for (int i = 0; i < lines.Count - 1; ++i){
					basics.Add(JsonUtility.FromJson<VisualProperty>(lines[i]));
				}
				for (int i = 0; i < basics.Count; ++i){
					switch (basics[i].type){
						case "IntField":
							properties.Add(new IntField());
						break;
						case "FloatField":
							properties.Add(new FloatField());
						break;
						case "IntSliderField":
							properties.Add(new IntSliderField());
						break;
						case "FloatSliderField":
							properties.Add(new FloatSliderField());
						break;
						case "ColorField":
							properties.Add(new ColorField());
						break;
						case "Vector2Field":
							properties.Add(new Vector2Field());
						break;
						case "Vector3Field":
							properties.Add(new Vector3Field());
						break;
						case "Vector4Field":
							properties.Add(new Vector4Field());
						break;
						case "Texture2DField":
							properties.Add(new Texture2DField());
						break;
						case "SplineField":
							properties.Add(new SplineField());
						break;
						case "BoolField":
							properties.Add(new BoolField());
						break;
					}
					properties[i].position = basics[i].position;
					properties[i].name = basics[i].name;
					properties[i].selected = basics[i].selected;
					properties[i].Values = basics[i].Values;
					properties[i].serial = basics[i].serial;
					properties[i].unique = basics[i].unique;
				}
			}
			return properties;
		}
		
		internal RenderQueue RenderRead(string database){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			RenderQueue renderQueue = new RenderQueue();
			List<string> lines = new List<string>();
			List<int> newLines = new List<int>();
			int numberOfLines;
			if (database.IndexOf('|') > 0){
				database = database.Substring(database.IndexOf('|'), database.IndexOf('\n') - database.IndexOf('|'));
				if (database != ""){
					numberOfLines = 1;
					for (int i = 0; i < database.Length; ++i){
						if (database[i] == '|'){
							++numberOfLines;
							newLines.Add(i);
						}
					}
				} else {
					numberOfLines = 0;
				}
				for (int i = 1; i < numberOfLines; ++i){
					if (i == numberOfLines - 1){
						lines.Add(database.Substring(newLines[i - 1] + 1, database.Length - (newLines[i - 1] + 1)));
					} else {
						lines.Add(database.Substring(newLines[i - 1] + 1, newLines[i] - (newLines[i - 1] + 1)));
					}
				}
				renderQueue = JsonUtility.FromJson<RenderQueue>(lines[lines.Count - 1]);
			}
			return renderQueue;
		}
		
		internal virtual List<List<VisualElement>> ElementsRead(string database){
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
			for (int i = 0; i < data.Count; ++i){
				elements.Add(Elements(data[i]));
			}
			return elements;
		}
		
		internal virtual List<VisualElement> Elements(string database){
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
				if (elements[i].name.Contains("Custom")){
					elements[i].Refresh();
					elements[i].Arrange();
					elements[i].changed = false;
					
				}
			}
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].name == "Add" || elements[i].name == "Av" || elements[i].name == "Sub" || elements[i].name == "Mul" || elements[i].name == "Div" || elements[i].name == "Mod"){
						elements[i].joints.RemoveRange(1, elements[i].joints.Count - 1);
						for (int j = 1; j < (int) float.Parse(elements[i].Values[0], CultureInfo.InvariantCulture); ++j){
							elements[i].joints.Add(new VisualElement.Joint(0, 4));
							try {
								elements[i].joints[j].connected = bool.Parse(lines[newElements[i] + 5 + elements[i].Values.Count + j * 3].Substring(2));
							} catch {
								Debug.Log("Some data was not loaded due to loading errors, that may occur due to custom function changes");
							}
							if (!lines[newElements[i] + 6 + elements[i].Values.Count + j * 3].Contains("null") && elements[i].joints[j].type == 0){
								if (((int) float.Parse(lines[newElements[i] + 7 + elements[i].Values.Count + j * 3].Substring(3), CultureInfo.InvariantCulture)) < elements[(int) float.Parse(lines[newElements[i] + 6 + elements[i].Values.Count + j * 3].Substring(3), CultureInfo.InvariantCulture)].joints.Count){
									try {
										elements[i].joints[j].connections = elements[(int) float.Parse(lines[newElements[i] + 6 + elements[i].Values.Count + j * 3].Substring(3), CultureInfo.InvariantCulture)].joints[(int) float.Parse(lines[newElements[i] + 7 + elements[i].Values.Count + j * 3].Substring(3), CultureInfo.InvariantCulture)];
									} catch {
										Debug.Log("Some data was not loaded due to loading errors, that may occur due to custom function changes");
									}
								}
							}
						}
				} else {
					for (int j = 0; j < elements[i].joints.Count; ++j){
						try {
							elements[i].joints[j].connected = bool.Parse(lines[newElements[i] + 5 + elements[i].Values.Count + j * 3].Substring(2));
						} catch {
							Debug.Log("Some data was not loaded due to loading errors, that may occur due to custom function changes");
						}
						if (!lines[newElements[i] + 6 + elements[i].Values.Count + j * 3].Contains("null") && elements[i].joints[j].type == 0){
							if (((int) float.Parse(lines[newElements[i] + 7 + elements[i].Values.Count + j * 3].Substring(3), CultureInfo.InvariantCulture)) < elements[(int) float.Parse(lines[newElements[i] + 6 + elements[i].Values.Count + j * 3].Substring(3), CultureInfo.InvariantCulture)].joints.Count){
								try {
									elements[i].joints[j].connections = elements[(int) float.Parse(lines[newElements[i] + 6 + elements[i].Values.Count + j * 3].Substring(3), CultureInfo.InvariantCulture)].joints[(int) float.Parse(lines[newElements[i] + 7 + elements[i].Values.Count + j * 3].Substring(3), CultureInfo.InvariantCulture)];
								} catch {
									Debug.Log("Some data was not loaded due to loading errors, that may occur due to custom function changes");
								}
							}
						}
					}
				}
			}
			return elements;
		}
	}
}