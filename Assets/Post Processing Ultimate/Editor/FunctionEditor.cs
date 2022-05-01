using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using System.IO;

namespace NTEC.PPU{
	[System.Serializable]
	public class FunctionEditor : PPUEditor{
		
		internal new FunctionCompile compiler = new FunctionCompile();
		internal new FunctionSave saver = new FunctionSave();
	
		[MenuItem("Window/Post Processing Ultimate/Function Editor")]
		private static void Init(){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			FunctionEditor window = (FunctionEditor) GetWindow(typeof(FunctionEditor));
			window.minSize = new Vector2(400, 100);
			window.position = new Rect(UnityEngine.Screen.currentResolution.width * 0.125f, UnityEngine.Screen.currentResolution.height * 0.125f, UnityEngine.Screen.currentResolution.width * 0.75f, UnityEngine.Screen.currentResolution.height * 0.75f);
			window.wantsMouseMove = true;
			window.wantsMouseEnterLeaveWindow = true;
			window.Show();
			window.titleContent = new GUIContent ("Function Editor");
		}
		
		internal override void Awake(){
			base.Awake();
			settings.Values[1] = "MyFunction";
			lastDir = "Assets/Post Processing Ultimate/Functions";
		}
		
		internal override void New(){
			allElements.Clear();
			elements.Clear();
			allElements.Add(elements);
			elements.Add(new Output());
			elements[0].position.x = 272 / scale + 1 * 400;
			elements[0].position.y = 48;
			elements.Add(new Input1());
			elements[1].position.x = 272 / scale;
			elements[1].position.y = 48;
			Connect(elements[0].joints[0], elements[1].joints[0]);
			CheckConnections();
			settings.Values[1] = "MyFunction";
			settings.Values[2] = "_MainTex";
			properties.Clear();
			renderQueue.passes.Clear();
			renderQueue.passes.Add(new VisualPass());
			DisposePasses();
			path = null;
		}
		
		internal override void Open(){
			oldPath = path;
			path = EditorUtility.OpenFilePanel("Open PPU function", lastDir, "hlsl");
			path = path == "" ? null : path;
			if (path != null){
				oldPath = path;
				settings = saver.SettingsRead(System.IO.File.ReadAllText(path));
				allElements = saver.ElementsRead(System.IO.File.ReadAllText(path));
				elements = allElements[0];
				Serialize();
				CheckConnections();
				DisposeProperties();
				for (int i = path.Length - 1; i >= 0; --i){
					if (path[i] == '/'){
						lastDir = path.Substring(i + 1);
						break;
					}
				}
			} else {
				path = oldPath;
			}
		}
		
		internal override void Compile(){
			Serialize();
			oldPath = path;
			path = EditorUtility.SaveFilePanel("Write PPU Function", lastDir, settings.Values[1], "hlsl");
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].name == "Input"){
					elements[i].Values[1] = "in" + i;
				}
			}
			allElements[0] = elements;
			if (path != null && path != ""){
				compiler.Compile(allElements, settings, properties, path, renderQueue);
				oldPath = path;
				for (int i = path.Length - 1; i >= 0; --i){
					if (path[i] == '/'){
						lastDir = path.Substring(i + 1);
						break;
					}
				}
			} else {
				path = oldPath;
			}
		}
		
		internal override void Write(){
			Serialize();
			Enquenque();
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].name == "Input"){
					elements[i].Values[1] = "in" + i;
				}
			}
			allElements[0] = elements;
			if (path != null && path != ""){
				compiler.Compile(allElements, settings, properties, path, renderQueue);
			}
		}
		
		internal override void DrawToolbar(){
			if (stylesUI.Count == 5){
				stylesUI.Add(new GUIStyle(GUI.skin.button));
				stylesUI[5].focused.textColor = new Color(0.7f, 0.7f, 0.7f);
				stylesUI[5].normal.textColor = new Color(0.7f, 0.7f, 0.7f);
				stylesUI[5].active.textColor = new Color(0.7f, 0.7f, 0.7f);
				stylesUI[5].hover.textColor = new Color(0.7f, 0.7f, 0.7f);
				stylesUI[5].alignment = TextAnchor.MiddleCenter;
				stylesUI[5].focused.background = Resources.Load("Buttons/GeneralOff") as Texture2D;
				stylesUI[5].normal.background = Resources.Load("Buttons/GeneralOff") as Texture2D;
				stylesUI[5].active.background = Resources.Load("Buttons/GeneralExe") as Texture2D;
				stylesUI[5].hover.background = Resources.Load("Buttons/GeneralOn") as Texture2D;
			}
			EditorGUI.DrawRect(new Rect(0, 0, 228, position.height), new Color(0, 0, 0, 0.85f));
			EditorGUI.DrawRect(new Rect(228, 0, 4, position.height), new Color(0, 0, 0, 1));
			toolbarScroll = GUI.BeginScrollView(new Rect(0, 0, 228, position.height), toolbarScroll, new Rect(0, 0, 228, 4 * 36 + 11 * 18 + 9 + propertiesWindow + passesWindow), false, false, GUIStyle.none, GUIStyle.none);
			if (GUI.Button(new Rect (0, 0 * 36 + 9, 227, 36), new GUIContent("", "Reset current window"), stylesUI[1])){
				New();
			}
			if (GUI.Button(new Rect (0, 1 * 36 + 9, 227, 36), new GUIContent("", "Open shader file"), stylesUI[2])){
				try {
					Open();
				} catch {
					Debug.Log("Wrong file");
					path = oldPath;
				}
			}
			if (GUI.Button(new Rect (0, 2 * 36 + 9, 227, 36), new GUIContent("", "Write shader to new file"), stylesUI[3])){
				Compile();
			}
			if (GUI.Button(new Rect (0, 3 * 36 + 9, 227, 36), new GUIContent("", "Overwrite current file"), stylesUI[4])){
				if (path != null && path != ""){
					Write();
				} else {
					Compile();
				}
			}
			stylesUI[0].alignment = TextAnchor.MiddleCenter;
			stylesUI[0].clipping = TextClipping.Clip;
			GUI.DrawTexture(new Rect(8, 4 * 36 + 18, 208, 3), toolbarTextures[11]);
			if (!(path == null || path == "")){
				stylesUI[0].alignment = TextAnchor.MiddleRight;
			}
			EditorGUI.LabelField(new Rect(4, 4 * 36 + 1 * 18 + 9, 220, 18), new GUIContent(path == null || path == "" ? "FUNCTION FILE NOT SELECTED" : path, "Current shader file path"),  stylesUI[0]);
			stylesUI[0].alignment = TextAnchor.MiddleLeft;
			GUI.DrawTexture(new Rect(8, 4 * 36 + 2 * 18 + 18, 208, 3), toolbarTextures[11]);
			EditorGUI.LabelField(new Rect(8, 4 * 36 + 3 * 18 + 9, 96, 18), new GUIContent("NAME:", "Name of shader file"),  stylesUI[0]);
			settings.Values[1] = EditorGUI.TextField(new Rect (96, 4 * 36 + 3 * 18 + 9, 124, 16), settings.Values[1], toolbar[1]);
			EditorGUI.LabelField(new Rect(8, 4 * 36 + 4 * 18 + 9, 96, 18), new GUIContent("FLOW:", "Visual representation of data flow"),  stylesUI[0]);
			flow = EditorGUI.Toggle(new Rect(96, 4 * 36 + 4 * 18 + 9, 16, 16), flow, toolbar[2]);
			EditorGUI.LabelField(new Rect(8, 4 * 36 + 5 * 18 + 9, 96, 18), new GUIContent("PRECISION:", "Destined shader precision"),  stylesUI[0]);
			settings.Values[3] = EditorGUI.Popup(new Rect(96, 4 * 36 + 5 * 18 + 9, 124, 16), Int32.Parse(settings.Values[3]), options, toolbar[0]).ToString();
			GUI.EndScrollView();
		}
		
		internal override void DrawContext(Vector2 coords){
			if (!context){
				contextPosition = new Vector2(-128, 0);
				for (int i = 0; i < foldouts.Count; ++i){
					foldouts[i] = false;
				}
			}
			if (context && Event.current.type != EventType.ContextClick){
				if (stylesContext.Count == 1){
					stylesContext.Add(new GUIStyle(EditorStyles.foldout));
					stylesContext[1].focused.textColor = Color.black;
					stylesContext[1].normal.textColor = Color.black;
					stylesContext[1].active.textColor = Color.black;
					stylesContext[1].onActive.textColor = Color.black;
					stylesContext[1].onFocused.textColor = Color.black;
					stylesContext[1].onNormal.textColor = Color.black;
					stylesContext[1].focused.background = Resources.Load("Icons/FoldoutOff") as Texture2D;
					stylesContext[1].normal.background = Resources.Load("Icons/FoldoutOff") as Texture2D;
					stylesContext[1].active.background = Resources.Load("Icons/FoldoutOff") as Texture2D;
					stylesContext[1].onFocused.background = Resources.Load("Icons/FoldoutOn") as Texture2D;
					stylesContext[1].onNormal.background = Resources.Load("Icons/FoldoutOn") as Texture2D;
					stylesContext[1].onActive.background = Resources.Load("Icons/FoldoutOff") as Texture2D;
				}
				float visibleHeight = 16;
				GUI.DrawTexture(new Rect(coords.x - 10, coords.y - 10, 148, 232), Resources.Load("Backgrounds/Context") as Texture2D);
				search2 = search;
				search = GUI.TextField(new Rect(coords.x, coords.y, 128, 16), search, toolbar[1]);
				if (search != search2){
					contextScroll.y = 0;
				}
				searching = search != "Search..." && search != "";
				visibleHeight = Mathf.Clamp(position.height - coords.y, 19, 196);
				contextScroll = GUI.BeginScrollView(new Rect(coords.x, coords.y + 16, 128, visibleHeight), contextScroll, new Rect(0, 0, 128, lastHeight), false, false, GUIStyle.none, GUIStyle.none);
				lastHeight = 0;
				for (int i = 0; i < foldouts.Count; ++i){
					if (foldouts[i]){
						lastHeight += 23;
					} else {
						lastHeight += 22;
					}
				}
				lastHeight += 8;
				EditorGUI.DrawRect(new Rect(contextScroll.x, contextScroll.y, 128, visibleHeight), new Color(0.7f, 0.7f, 0.7f, 0.5f));
				if (CheckSearch(true, 4, 0)){
					foldouts[4] = EditorGUILayout.Foldout(foldouts[4], "Arithmetic", stylesContext[1]);
				}
				if (foldouts[4] || searching){
					for (int i = 0; i < contextLabels[4].Count; ++i){
						if (CheckSearch(false, 4, i)){
							if (GUILayout.Button(new GUIContent(contextLabels[4][i], Tooltips.List(contextLabels[4][i])), stylesContext[0], GUILayout.Width(128))){
								AddElement(contextLabels[4][i]);
								context = false;
							}
							lastHeight += 13;
						}
					}
				}
				if (CheckSearch(true, 5, 0)){
					foldouts[5] = EditorGUILayout.Foldout(foldouts[5], "Constant", stylesContext[1]);
				}
				if (foldouts[5] || searching){
					for (int i = 0; i < contextLabels[5].Count; ++i){
						if (CheckSearch(false, 5, i)){
							if (GUILayout.Button(new GUIContent(contextLabels[5][i], Tooltips.List(contextLabels[5][i])), stylesContext[0], GUILayout.Width(128))){
								AddElement(contextLabels[5][i]);
								context = false;
							}
							lastHeight += 13;
						}
					}
				}
				if (CheckSearch(true, 6, 0)){
					foldouts[6] = EditorGUILayout.Foldout(foldouts[6], "Data", stylesContext[1]);
				}
				if (foldouts[6] || searching){
					for (int i = 0; i < contextLabels[6].Count; ++i){
						if (CheckSearch(false, 6, i)){
							if (GUILayout.Button(new GUIContent(contextLabels[6][i], Tooltips.List(contextLabels[6][i])), stylesContext[0], GUILayout.Width(128))){
								AddElement(contextLabels[6][i]);
								context = false;
							}
							lastHeight += 13;
						}
					}
				}
				if (CheckSearch(true, 3, 0)){
					foldouts[3] = EditorGUILayout.Foldout(foldouts[3], "HLSL Function", stylesContext[1]);
				}
				if (foldouts[3] || searching){
					for (int i = 0; i < contextLabels[3].Count; ++i){
						if (CheckSearch(false, 3, i)){
							if (GUILayout.Button(new GUIContent(contextLabels[3][i], Tooltips.List(contextLabels[3][i])), stylesContext[0], GUILayout.Width(128))){
								AddElement(contextLabels[3][i]);
								context = false;
							}
							lastHeight += 13;
						}
					}
				}
				if (CheckSearch(true, 9, 0)){
					foldouts[9] = EditorGUILayout.Foldout(foldouts[9], "Input", stylesContext[1]);
				}
				if (foldouts[9] || searching){
					for (int i = 0; i < contextLabels[9].Count; ++i){
						if (CheckSearch(false, 9, i)){
							if (GUILayout.Button(new GUIContent(contextLabels[9][i], Tooltips.List(contextLabels[9][i])), stylesContext[0], GUILayout.Width(128))){
								AddElement(contextLabels[9][i]);
								Enquenque();
								context = false;
							}
							lastHeight += 13;
						}
					}
				}
				if (CheckSearch(true, 7, 0)){
					foldouts[7] = EditorGUILayout.Foldout(foldouts[7], "Logic", stylesContext[1]);
				}
				if (foldouts[7] || searching){
					for (int i = 0; i < contextLabels[7].Count; ++i){
						if (CheckSearch(false, 7, i)){
							if (GUILayout.Button(new GUIContent(contextLabels[7][i], Tooltips.List(contextLabels[7][i])), stylesContext[0], GUILayout.Width(128))){
								AddElement(contextLabels[7][i]);
								context = false;
							}
							lastHeight += 13;
						}
					}
				}
				if (CheckSearch(true, 11, 0)){
					foldouts[11] = EditorGUILayout.Foldout(foldouts[11], "PPS Function", stylesContext[1]);
				}
				if (foldouts[11] || searching){
					for (int i = 0; i < contextLabels[11].Count; ++i){
						if (CheckSearch(false, 11, i)){
							if (GUILayout.Button(new GUIContent(contextLabels[11][i], Tooltips.List(contextLabels[11][i])), stylesContext[0], GUILayout.Width(128))){
								AddElement(contextLabels[11][i]);
								context = false;
							}
							lastHeight += 13;
						}
					}
				}
				if (CheckSearch(true, 12, 0)){
					foldouts[12] = EditorGUILayout.Foldout(foldouts[12], "Predefined Macro", stylesContext[1]);
				}
				if (foldouts[12] || searching){
					for (int i = 0; i < contextLabels[12].Count; ++i){
						if (CheckSearch(false, 12, i)){
							if (GUILayout.Button(new GUIContent(contextLabels[12][i], Tooltips.List(contextLabels[12][i])), stylesContext[0], GUILayout.Width(128))){
								AddElement(contextLabels[12][i]);
								Enquenque();
								context = false;
							}
							lastHeight += 13;
						}
					}
				}
				if (CheckSearch(true, 2, 0)){
					foldouts[2] = EditorGUILayout.Foldout(foldouts[2], "Value", stylesContext[1]);
				}
				if (foldouts[2] || searching){
					for (int i = 0; i < contextLabels[2].Count; ++i){
						if (CheckSearch(false, 2, i)){
							if (GUILayout.Button(new GUIContent(contextLabels[2][i], Tooltips.List(contextLabels[2][i])), stylesContext[0], GUILayout.Width(128))){
								AddElement(contextLabels[2][i]);
								context = false;
							}
							lastHeight += 13;
						}
					}
				}
				if (CheckSearch(true, 8, 0)){
					foldouts[8] = EditorGUILayout.Foldout(foldouts[8], "Variable", stylesContext[1]);
				}
				if (foldouts[8] || searching){
					for (int i = 0; i < contextLabels[8].Count; ++i){
						if (CheckSearch(false, 8, i)){
							if (GUILayout.Button(new GUIContent(contextLabels[8][i], Tooltips.List(contextLabels[8][i])), stylesContext[0], GUILayout.Width(128))){
								AddElement(contextLabels[8][i]);
								Enquenque();
								context = false;
							}
							lastHeight += 13;
						}
					}
				}
				GUI.EndScrollView();
			}
		}
		
		public override void OnBeforeSerialize(){
			Serialize();
			serialization = saver.Save(allElements, settings, properties, renderQueue);
		}
		
		public override void OnAfterDeserialize() {
			if (serialization != null){;
				settings = saver.SettingsRead(serialization);
				allElements = saver.ElementsRead(serialization);
				elements = allElements[0];
			}
		}
		
		internal override void DragUpdated(){
			if (DragAndDrop.paths[0].Contains(".hlsl")){
				DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
			}
		}
		
		internal override void DragExited(){
			if (DragAndDrop.paths[0].Contains(".hlsl")){
				path = DragAndDrop.paths[0];
				if (path != null){
					oldPath = path;
					renderQueue.passes.Clear();
					renderQueue = saver.RenderRead(System.IO.File.ReadAllText(path));
					DisposePasses();
					settings = saver.SettingsRead(System.IO.File.ReadAllText(path));
					properties = saver.PropertiesRead(System.IO.File.ReadAllText(path));
					allElements = saver.ElementsRead(System.IO.File.ReadAllText(path));
					elements = allElements[0];
					pass = 0;
					currentPass = 0;
					Serialize();
					CheckConnections();
					DisposeProperties();
				}
			}
		}
	}
}
