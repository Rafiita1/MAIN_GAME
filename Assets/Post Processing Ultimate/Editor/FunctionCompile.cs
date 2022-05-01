using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using System.IO;

namespace NTEC.PPU{
	[System.Serializable]
	public class FunctionCompile : ShaderCompile{
		
		internal new FunctionSave saver = new FunctionSave();
		internal string outputSize;
		internal CustomData custom;
	
		internal override void Compile(List<List<VisualElement>> allElements, Settings settings, List<VisualProperty> properties, string path, RenderQueue renderQueue){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			custom = new CustomData();
			custom.Name = settings.Values[1];
			custom.OutputSize = Int32.Parse(allElements[0][0].Values[0]) + 1;
			precision = settings.Values[3] == "0" ? "half" : "float";
			name = path.Substring(0, path.Length - 5);
			outputSize = (Int32.Parse(allElements[0][0].Values[0]) + 1).ToString();
			for (int i = name.Length - 1; i >= 0; --i){
				if (name[i] == '/'){
					name = name.Substring(i + 1);
					break;
				}
			}
			List<string> frag = new List<string>();
			frag = Frag(allElements[0]);
			List<string> arguments = new List<string>();
			arguments = Arguments(FilterInputs(allElements[0]));
			string output = saver.Save(allElements, settings, properties, renderQueue) + "\n//";
			output += JsonUtility.ToJson(custom) + "\n\n";
			output += precision + outputSize + " " + name + "(";
			for (int i = 0; i < arguments.Count; ++i){
				output += arguments[i];
			}
			output += "){";
			for (int i = 0; i < frag.Count; ++i){
				output += "\n\t" + frag[i];
			}
			output += "\n}";
			File.WriteAllBytes(path, System.Text.Encoding.UTF8.GetBytes(output));
			AssetDatabase.Refresh();
			ShaderEditor[] windows = Resources.FindObjectsOfTypeAll<ShaderEditor>();
			if (windows.Length == 1){
				windows[0].Refresh();
			} else {
				AssetDatabase.ImportAsset("Assets/Post Processing Ultimate/Shaders", ImportAssetOptions.ImportRecursive);
			}
		}
		
		internal List<VisualElement> FilterInputs(List<VisualElement> elements){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			List<VisualElement> output = new List<VisualElement>();
			bool check;
			for (int i = elements.Count - 1; i >= 0; --i){
				if (i > 0){
					check = true;
					for (int j = i - 1; j >= 0; --j){
						if (elements[i].name.Contains("Input") && elements[j].name.Contains("Input") && elements[i].Values[0] == elements[j].Values[0]){
							check = false;
							Debug.Log("Same input names found: " + elements[i].Values[0]);
							break;
						}
					}
					if (check){
						output.Add(elements[i]);
					}
				} else {
					output.Add(elements[0]);
				}
			}
			return output;
		}
		
		internal List<string> Arguments(List<VisualElement> elements){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			List<string> arguments = new List<string>();
			string inputSize;
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].name.Contains("Input")){
					inputSize = elements[i].name.Substring(5);
					custom.InputSize.Add(Int32.Parse(inputSize));
					custom.InputName.Add(elements[i].Values[0]);
					if (arguments.Count == 0){
						arguments.Add(precision + inputSize + " " + elements[i].Values[0]);
					} else {
						arguments.Add(", " + precision + inputSize + " " + elements[i].Values[0]);
					}
				}
			}
			return arguments;
		}
		
		internal override List<string> Frag(List<VisualElement> elements){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			List<string> frag = new List<string>();
			List<int> variables = Variables(elements);
			List<int> pres = Pres(elements);
			for (int i = 0; i < variables.Count; ++i){
				frag.Add(precision + elements[variables[i]].name.Substring(8) + " var" + elements[variables[i]].Values[0] + " = 0.0;");
			}
			frag.Add(precision + outputSize + " Output = 0.0;");
			for (int i = 0; i < pres.Count; ++i){
				frag.AddRange(MacroCom2(elements, pres[i]));
			}
			for (int i = 0; i < variables.Count; ++i){
				frag.AddRange(VariableCom2(elements, variables[i]));
			}
			frag.AddRange(OutputCom(elements, 0));
			frag.Add("return Output;");
			return frag;
		}
		
		//=====FE=====
		internal List<string> OutputCom(List<VisualElement> elements, int element){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			string label = "";
			string size = "";
			for (int i = 0; i < me.joints.Count; ++i){
				if (me.joints[i].connections != null){
					switch (i) {
					case 0:
						label = me.Values[0] == "0" ? "Output = " : "Output.x = ";
					break;
					case 1:
						label = "Output.y = ";
					break;
					case 2:
						label = me.Values[0] == "1" ? "Output.xy = " : "Output.z = ";
					break;
					case 3:
						label = me.Values[0] == "2" ? "Output.xyz = " : "Output.w = ";
					break;
					case 4:
						label = "Output = ";
					break;
					}
				}
				if (me.joints[i].connections != null){
					switch (i) {
					case 0:
						size = "Output";
					break;
					case 1:
						size = "Output";
					break;
					case 2:
						size = me.Values[0] == "1" ? "Output2" : "Output";
					break;
					case 3:
						size = me.Values[0] == "2" ? "Output3 = " : "Output";
					break;
					case 4:
						size = "Output4";
					break;
					}
				}
				if (me.joints[i].connections != null){
					output.Add(label + Demand(elements, (int) me.joints[i].connections.serial.x, (int) me.joints[i].connections.serial.y, size, i, 1) + ";");
				}
			}
			return output;
		}
	}
}
