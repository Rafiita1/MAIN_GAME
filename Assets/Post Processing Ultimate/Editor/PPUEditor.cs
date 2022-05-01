using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using System.IO;

namespace NTEC.PPU {
	public class PPUEditor : EditorWindow, ISerializationCallbackReceiver {
		
		internal List<VisualElement> elements = new List<VisualElement>();
		internal List<VisualElement> selectedElements = new List<VisualElement>();
		internal VisualElement focusedElement;
		internal VisualElement.Joint focusedJoint;
		internal Vector2 mouseCoords;
		internal Vector2 propCoords;
		internal List<Texture2D> backgrounds = new List<Texture2D>();
		internal bool tempLine = false;
		internal bool copyTime = false;
		internal VisualElement.Joint tempJoint;
		internal Texture2D lineTexture;
		internal bool selecting;
		internal Rect selectingBox = new Rect(0, 0, 0, 0);
		internal VisualElement tempElement;
		internal int lastEvent;
		internal float scale = 0.5f;
		internal string[] options = new string[] {"Half", "Float"};
		internal string[] targets = new string[] {"After Stack", "Before Stack", "Before Transparent"};
		[SerializeField] internal string path;
		[SerializeField] internal string oldPath;
		[SerializeField] internal string state;
		internal string lastDir = "Assets/Post Processing Ultimate/Shaders";
		internal int lineColor = 5;
		internal bool off;
		internal bool ready;
		internal Vector2 oldSeries = new Vector2(0, 0);
		internal Matrix4x4 defaultGUI;
		internal BindingFlags fullBinding;
		internal float offset;
		internal bool docked;
		internal Vector2 contextPosition = new Vector2(0,0);
		internal Vector2 contextScroll = new Vector2(0,0);
		internal Vector2 toolbarScroll = new Vector2(0,0);
		internal string search = "Search...";
		internal string search2 = "Search...";
		internal List<bool> foldouts = new List<bool>();
		internal bool context;
		internal int lastHeight = 64;
		internal List<List<string>> contextLabels = new List<List<string>>();
		internal List<Color> beziers = new List<Color>();
		internal List<Vector3> vec = new List<Vector3>();
		internal bool focusElement = false;
		internal bool focusJoint = false;
		internal VisualElement.Joint used;
		internal List<Vector4> serials = new List<Vector4>();
		internal List<int> copy = new List<int>();
		internal List<int> copyied = new List<int>();
		internal MethodInfo isDockedMethod;
		internal bool output;
		internal List<Texture2D> lineTextures = new List<Texture2D>();
		internal List<Texture2D> toolbarTextures = new List<Texture2D>();
		internal List<VisualElement.Joint> loose = new List<VisualElement.Joint>();
		internal List<List<Texture2D>> arrows = new List<List<Texture2D>>();
		internal int arrowColor;
		internal List<Vector3> points = new List<Vector3>();
		internal Vector3[] twoPoints = new Vector3[2];
		internal bool scroll;
		internal List<Vector2> distance = new List<Vector2>();
		internal bool flow = true;
		internal List<List<List<Texture2D>>> flowDots = new List<List<List<Texture2D>>>();
		internal float time;
		internal float pointsDistance;
		internal int flowPoints;
		internal RenderQueue renderQueue = new RenderQueue(); 
		internal List<VisualProperty> properties = new List<VisualProperty>();
		internal ReorderableList inputs;
		internal List<List<VisualProperty>> specifiedProperties = new List<List<VisualProperty>>();
		internal string[] propNames = new string[] {"Int", "Float", "IntSlider", "FloatSlider", "Color", "Vector2", "Vector3", "Vector4", "Texture", "Spline", "Bool"};
		internal int newProp = 0;
		internal float propertiesWindow = 16;
		internal float passesWindow = 48;
		internal List<List<string>> popupOptions = new List<List<string>>();
		internal List<List<int>> popupUniques = new List<List<int>>();
		internal int pass = 0;
		internal int currentPass = 0;
		internal List<List<VisualElement>> allElements = new List<List<VisualElement>>();
		internal Settings settings = new Settings();
		internal ShaderCompile compiler = new ShaderCompile();
		internal ShaderSave saver = new ShaderSave();
		internal List<KeyCode> pressed = new List<KeyCode>();
		internal bool loaded;
		internal static bool loadedS;
		internal List<GUIStyle> toolbar = new List<GUIStyle>();
		internal List<Vector2> offsets = new List<Vector2>();
		internal List<GUIStyle> stylesUI = new List<GUIStyle>();
		internal List<GUIStyle> stylesContext = new List<GUIStyle>();
		internal UserSettings uSets;
		internal bool searching;
		[SerializeField] internal string serialization;
		
		internal virtual void Awake(){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			LoadSettings();
			lastDir = "Assets/Post Processing Ultimate/Shaders";
			if (path == null){
				New();
			}
			renderQueue.inputOptions.Add("Game");
			renderQueue.outputOptions.Add("Screen");
			renderQueue.variableOptions.Add("None");
			toolbarTextures.Add(Resources.Load("Buttons/NewOn") as Texture2D);
			toolbarTextures.Add(Resources.Load("Buttons/NewOff") as Texture2D);
			toolbarTextures.Add(Resources.Load("Buttons/NewExe") as Texture2D);
			toolbarTextures.Add(Resources.Load("Buttons/OpenOn") as Texture2D);
			toolbarTextures.Add(Resources.Load("Buttons/OpenOff") as Texture2D);
			toolbarTextures.Add(Resources.Load("Buttons/CompileOn") as Texture2D);
			toolbarTextures.Add(Resources.Load("Buttons/CompileOff") as Texture2D);
			toolbarTextures.Add(Resources.Load("Buttons/WriteOn") as Texture2D);
			toolbarTextures.Add(Resources.Load("Buttons/WriteOff") as Texture2D);
			toolbarTextures.Add(Resources.Load("Buttons/WriteExe") as Texture2D);
			toolbarTextures.Add(Resources.Load("Icons/Popup") as Texture2D);
			toolbarTextures.Add(Resources.Load("Icons/Break") as Texture2D);
			stylesUI.Add(new GUIStyle());
			stylesUI[0].alignment = TextAnchor.MiddleCenter;
			stylesUI[0].clipping = TextClipping.Clip;
			stylesUI[0].normal.textColor = new Color(0.7f, 0.7f, 0.7f);
			stylesUI.Add(new GUIStyle());
			stylesUI[1].focused.background = toolbarTextures[1];
			stylesUI[1].normal.background = toolbarTextures[1];
			stylesUI[1].active.background = toolbarTextures[2];
			stylesUI[1].hover.background = toolbarTextures[0];
			stylesUI.Add(new GUIStyle());
			stylesUI[2].focused.background = toolbarTextures[4];
			stylesUI[2].normal.background = toolbarTextures[4];
			stylesUI[2].active.background = toolbarTextures[3];
			stylesUI[2].hover.background = toolbarTextures[3];
			stylesUI.Add(new GUIStyle());
			stylesUI[3].focused.background = toolbarTextures[6];
			stylesUI[3].normal.background = toolbarTextures[6];
			stylesUI[3].active.background = toolbarTextures[5];
			stylesUI[3].hover.background = toolbarTextures[5];
			stylesUI.Add(new GUIStyle());
			stylesUI[4].focused.background = toolbarTextures[8];
			stylesUI[4].normal.background = toolbarTextures[8];
			stylesUI[4].active.background = toolbarTextures[9];
			stylesUI[4].hover.background = toolbarTextures[7];
			stylesContext.Add(new GUIStyle());
			stylesContext[0].alignment = TextAnchor.MiddleCenter;
			stylesContext[0].hover.background = Texture2D.whiteTexture;
			backgrounds.Add(Resources.Load("Backgrounds/BigBackground") as Texture2D);
			backgrounds.Add(Resources.Load("Backgrounds/Vignette") as Texture2D);
			backgrounds.Add(Resources.Load("Backgrounds/Welcome") as Texture2D);
			distance.Add(Vector2.zero);
			distance.Add(Vector2.zero);
			distance.Add(Vector2.zero);
			vec.Add(Vector3.zero);
			vec.Add(Vector3.zero);
			vec.Add(Vector3.zero);
			vec.Add(Vector3.zero);
			beziers.Add(new Color(1, 1, 1, 0));
			beziers.Add(new Color(1, 1, 1, 0.5f));
			beziers.Add(new Color(1, 1, 1, 1));
			beziers.Add(Color.red);
			beziers.Add(Color.green);
			beziers.Add(new Color(0, 0.35f, 1));
			beziers.Add(Color.white);
			beziers.Add(Color.cyan);
			lineTextures.Add(new Texture2D(1, 7));
			lineTextures[0].SetPixel(0, 1, beziers[0]);
			lineTextures[0].SetPixel(0, 2, beziers[1]);
			lineTextures[0].SetPixel(0, 3, beziers[2]);
			lineTextures[0].SetPixel(1, 4, beziers[2]);
			lineTextures[0].SetPixel(0, 5, beziers[2]);
			lineTextures[0].SetPixel(0, 6, beziers[1]);
			lineTextures[0].SetPixel(0, 7, beziers[0]);
			lineTextures[0].Apply();
			lineTextures.Add(new Texture2D(1, 2));
			lineTextures[1].SetPixel(0, 0, beziers[0]);
			lineTextures[1].SetPixel(0, 1, beziers[2]);
			lineTextures[1].Apply();
			foldouts.Add(false);
			foldouts.Add(false);
			foldouts.Add(false);
			foldouts.Add(false);
			foldouts.Add(false);
			foldouts.Add(false);
			foldouts.Add(false);
			foldouts.Add(false);
			foldouts.Add(false);
			foldouts.Add(false);
			foldouts.Add(false);
			foldouts.Add(false);
			foldouts.Add(false);
			fullBinding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
			lineTexture = new Texture2D(1, 2);
			lineTexture.SetPixel(0, 0, new Color(1, 1, 1, 0));
			lineTexture.SetPixel(0, 1, new Color(1, 1, 1, 1));
			lineTexture.Apply();
			if (elements.Count == 0){
				New();
			}
			Serialize();
			AssetDatabase.Refresh();
			toolbar.Add(new GUIStyle());
		}
		
		internal virtual void New(){
			allElements.Clear();
			OrganizePasses();
			elements.Clear();
			allElements.Add(elements);
			elements.Add(new CameraOutput());
			elements[0].position.x = 272 / scale + 2 * 400;
			elements[0].position.y = 48 / scale;
			elements.Add(new CameraInput());
			elements[1].position.x = 272 / scale + 1 * 400;
			elements[1].position.y = 48 / scale;
			elements.Add(new StereoUV());
			elements[2].position.x = 272 / scale;
			elements[2].position.y = 48 / scale;
			Connect(elements[1].joints[3], elements[0].joints[3]);
			Connect(elements[2].joints[2], elements[1].joints[6]);
			CheckConnections();
			settings.Values[1] = "NTEC/Screen/MyShader";
			flow = uSets.Flow;
			properties.Clear();
			renderQueue.passes.Clear();
			renderQueue.passes.Add(new VisualPass());
			DisposePasses();
			path = null;
		}
		
		internal virtual void Open(){
			GUIUtility.keyboardControl = 0;
			oldPath = path;
			path = EditorUtility.OpenFilePanel("Open PPU shader", lastDir, "shader");
			path = path == "" ? null : path;
			if (path != null){
				settings = saver.SettingsRead(System.IO.File.ReadAllText(path));
				renderQueue.passes.Clear();
				renderQueue = saver.RenderRead(System.IO.File.ReadAllText(path));
				DisposePasses();
				properties = saver.PropertiesRead(System.IO.File.ReadAllText(path));
				allElements = saver.ElementsRead(System.IO.File.ReadAllText(path));
				elements = allElements[0];
				pass = 0;
				currentPass = 0;
				Serialize();
				DisposeProperties();
				TexelTextures();
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
		
		internal virtual void Compile(){
			Serialize();
			allElements[pass] = elements;
			oldPath = path;
			path = EditorUtility.SaveFilePanel("Write PPU Shader", lastDir, settings.Values[1].Contains("Screen/") ? settings.Values[1].Substring(settings.Values[1].IndexOf("Screen/") + 7) : settings.Values[1], "shader");
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].Values.Count > 1 && elements[i].Values[1] != "0") {
					switch (elements[i].name){
						case "_Int":
							elements[i].Values[0] = specifiedProperties[0][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_Float":
							elements[i].Values[0] = specifiedProperties[1][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_IntSlider":
							elements[i].Values[0] = specifiedProperties[2][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_FloatSlider":
							elements[i].Values[0] = specifiedProperties[3][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_Color":
							elements[i].Values[0] = specifiedProperties[4][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_Vector2":
							elements[i].Values[0] = specifiedProperties[5][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_Vector3":
							elements[i].Values[0] = specifiedProperties[6][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_Vector4":
							elements[i].Values[0] = specifiedProperties[7][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_Texture2D":
							elements[i].Values[0] = specifiedProperties[8][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_Spline":
							elements[i].Values[0] = specifiedProperties[9][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_Bool":
							elements[i].Values[0] = specifiedProperties[10][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
					}
				}
			}
			Enquenque();
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
		
		internal virtual void Write(){
			Serialize();
			allElements[pass] = elements;
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].Values.Count > 1 && elements[i].Values[1] != "0") {
					switch (elements[i].name){
						case "_Int":
							elements[i].Values[0] = specifiedProperties[0][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_Float":
							elements[i].Values[0] = specifiedProperties[1][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_IntSlider":
							elements[i].Values[0] = specifiedProperties[2][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_FloatSlider":
							elements[i].Values[0] = specifiedProperties[3][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_Color":
							elements[i].Values[0] = specifiedProperties[4][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_Vector2":
							elements[i].Values[0] = specifiedProperties[5][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_Vector3":
							elements[i].Values[0] = specifiedProperties[6][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_Vector4":
							elements[i].Values[0] = specifiedProperties[7][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_Texture2D":
							elements[i].Values[0] = specifiedProperties[8][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_Spline":
							elements[i].Values[0] = specifiedProperties[9][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
						case "_Bool":
							elements[i].Values[0] = specifiedProperties[10][Int32.Parse(elements[i].Values[1]) - 1].Values[0];
						break;
					}
				}
			}
			Enquenque();
			if (path != null && path != ""){
				compiler.Compile(allElements, settings, properties, path, renderQueue);
			}
		}

		internal void AddElement(object element){
			elements.Add(Creator.List(((VisualElement) element).name));
			elements[elements.Count - 1].position.x = mouseCoords.x;
			elements[elements.Count - 1].position.y = mouseCoords.y;
			elements[elements.Count - 1].selected = false;
			Serialize();
		}
		
		internal void AddElement(string element){
			elements.Add(Creator.List(element));
			elements[elements.Count - 1].position.x = mouseCoords.x;
			elements[elements.Count - 1].position.y = mouseCoords.y;
			elements[elements.Count - 1].selected = false;
			Serialize();
		}
		
		internal void SelectionHadle(){
			switch (Event.current.type){
				case EventType.MouseUp:
				selecting = false;
				selectingBox = new Rect(0, 0, 0, 0);
				break;
			}
		}
		
		internal void TexelTextures(){
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].name == "TexelSize"){
					elements[i].options.Clear();
					elements[i].options = compiler.Texel(properties, allElements, elements);
				}
			}
		}
		
		internal void MouseHandle(){
			focusElement = false;
			focusJoint = false;
			mouseCoords.x = Event.current.mousePosition.x;
			mouseCoords.y = Event.current.mousePosition.y;
			used = new VisualElement.Joint(0, 0);
			serials = new List<Vector4>();
			copy = new List<int>();
			copyied = new List<int>();
			switch (Event.current.type){
				case EventType.MouseDrag:
					if (Event.current.button == 0){
						if (mouseCoords.x > contextPosition.x - 5 && mouseCoords.x < (contextPosition.x + 133) / scale && mouseCoords.y > contextPosition.y + 26 && mouseCoords.y < (contextPosition.y + 217) / scale){
							contextPosition.x += Event.current.delta.x;
							contextPosition.y += Event.current.delta.y;
						} else if (focusedElement != null){
							focusedElement.position.x += Event.current.delta.x;
							focusedElement.position.y += Event.current.delta.y;
						}
						if (focusedJoint != null){
							if (focusedJoint.type == 0 && focusedJoint.connections != null && focusedJoint.connections != tempJoint){
								oldSeries = new Vector2(focusedJoint.serial.x, focusedJoint.serial.y);
								used = focusedJoint.connections;
								Disconnect(focusedJoint, focusedJoint.connections);
								focusedJoint = used;
								off = true;
							}
							for (int i = 0; i < elements.Count; ++i){
								for (int j = 0; j < elements[i].joints.Count; ++j){
									if (elements[i].joints[j].type == 0){
										if (mouseCoords.x >= elements[i].joints[j].coords.x && mouseCoords.x <= elements[i].joints[j].coords.x + 48 && mouseCoords.y >= elements[i].joints[j].coords.y - 4 && mouseCoords.y <= elements[i].joints[j].coords.y + 28){
											if ((!off || ready) && focusedJoint.serial.x != i && focusedJoint.type == 1){
												lineColor = 1;
												loose.Add(elements[i].joints[j]);
												for (int k = 0; k < elements[i].joints[j].prohibitions.Count; ++k){
													if (loose.Count < elements[i].joints[j].prohibitions.Count + 1){
														loose.Add(elements[i].joints[elements[i].joints[j].prohibitions[k]]);
													}
												}
											}
											break;
										} else {
											if (off){
												lineColor = 0;
												loose.Clear();
												if (elements[i].joints[j].serial == oldSeries && !(mouseCoords.x >= elements[i].joints[j].coords.x - 4 && mouseCoords.x <= elements[i].joints[j].coords.x + 48 && mouseCoords.y >= elements[i].joints[j].coords.y - 4 && mouseCoords.y <= elements[i].joints[j].coords.y + 28)){
													ready = true;
												}
											} else {
												lineColor = 5;
												loose.Clear();
											}
										}
									} else {
										if (mouseCoords.x >= elements[i].joints[j].coords.x - 48 && mouseCoords.x <= elements[i].joints[j].coords.x + 24 && mouseCoords.y >= elements[i].joints[j].coords.y - 4 && mouseCoords.y <= elements[i].joints[j].coords.y + 28){
											if ((!off || ready) && focusedJoint.serial.x != i && focusedJoint.type == 0){
												lineColor = 1;
												for (int k = 0; k < focusedJoint.prohibitions.Count; ++k){
													if (loose.Count < focusedJoint.prohibitions.Count + 1){
														loose.Add(elements[(int) focusedJoint.serial.x].joints[elements[(int) focusedJoint.serial.x].joints[(int) focusedJoint.serial.y].prohibitions[k]]);
													}
												}
											}
											break;
										} else {
											if (off){
												lineColor = 0;
												loose.Clear();
												if (elements[i].joints[j].serial == oldSeries && !(mouseCoords.x >= elements[i].joints[j].coords.x - 24 && mouseCoords.x <= elements[i].joints[j].coords.x + 28 && mouseCoords.y >= elements[i].joints[j].coords.y - 4 && mouseCoords.y <= elements[i].joints[j].coords.y + 28)){
													ready = true;
												}
											} else {
												lineColor = 5;
												loose.Clear();
											}
										}
									}
								}
								if (lineColor == 1){
									break;
								}
							}
							if (tempJoint == null){
								tempJoint = new VisualElement.Joint(1 - focusedJoint.type, focusedJoint.color);
							}
							tempJoint.coords = mouseCoords;
							if (!tempLine){
								tempLine = true;
								Connect(focusedJoint, tempJoint);
							}
						}
						if (focusedElement == null && focusedJoint == null && mouseCoords.y > 0 && !(mouseCoords.x > contextPosition.x && mouseCoords.x < contextPosition.x + 128 / scale && mouseCoords.y > contextPosition.y && mouseCoords.y < contextPosition.y + 212 / scale)){
							selectingBox.width = -1 * (selectingBox.x - mouseCoords.x);
							selectingBox.height = -1 * (selectingBox.y - mouseCoords.y);
							if (mouseCoords.x > 224 / scale){
								selecting = true;
							}
							for (int i = 0; i < elements.Count; ++i){
								if (CheckSelection(selectingBox, elements[i].position)){
									elements[i].selected = true;
								} else {
									elements[i].selected = false;
								}
							}
						}
						lastEvent = 0;
						if (!(NothingSelected() || copyTime || selecting) && uSets.SwitchMMB){
							for (int i = 0; i < elements.Count; ++i){
								if (elements[i].selected && elements[i] != focusedElement){
									elements[i].position.x += Event.current.delta.x;
									elements[i].position.y += Event.current.delta.y;
								}
							}
						}
					} else if (Event.current.button == 2){
						if (NothingSelected() || copyTime || uSets.SwitchMMB){
							for (int i = 0; i < elements.Count; ++i){
								elements[i].position.x += Event.current.delta.x;
								elements[i].position.y += Event.current.delta.y;
							}
						} else {
							for (int i = 0; i < elements.Count; ++i){
								if (elements[i].selected){
									elements[i].position.x += Event.current.delta.x;
									elements[i].position.y += Event.current.delta.y;
								}
							}
						}
					}
				break;
				case EventType.MouseMove:
					if (copyTime){
						int y = 0;
						for (int i = 0; i < elements.Count; ++i){
							if (elements[i].selected){
								elements[i].position.x = offsets[y].x + mouseCoords.x;
								elements[i].position.y = offsets[y].y + mouseCoords.y;
								++y;
							}
						}
					}
				break;
				case EventType.MouseDown:
					if (Event.current.button == 0){
						TexelTextures();
						Shortcut();
						selectingBox.x = mouseCoords.x;
						selectingBox.y = mouseCoords.y;
						selecting = false;
						if (!(mouseCoords.x > contextPosition.x && mouseCoords.x < contextPosition.x + 128 / scale && mouseCoords.y > contextPosition.y && mouseCoords.y < contextPosition.y + 212 / scale)){
							context = false;
						}
						for (int i = 0; i < elements.Count; ++i){
							if (mouseCoords.x >= elements[i].position.x && mouseCoords.x <= elements[i].position.x + elements[i].position.width && mouseCoords.y >= elements[i].position.y && mouseCoords.y <= elements[i].position.y + elements[i].position.height){
								focusedElement = elements[i];
								focusElement = true;
							} else {
								for (int j = 0; j < elements[i].joints.Count; ++j){
									if (elements[i].joints[j].type == 0){
										if (mouseCoords.x >= elements[i].joints[j].coords.x && mouseCoords.x <= elements[i].joints[j].coords.x + 48 && mouseCoords.y >= elements[i].joints[j].coords.y - 4 && mouseCoords.y <= elements[i].joints[j].coords.y + 28){
											focusedJoint = elements[i].joints[j];
											focusJoint = true;
										}
									} else {
										if (mouseCoords.x >= elements[i].joints[j].coords.x - 48 && mouseCoords.x <= elements[i].joints[j].coords.x + 24 && mouseCoords.y >= elements[i].joints[j].coords.y - 4 && mouseCoords.y <= elements[i].joints[j].coords.y + 28){
											focusedJoint = elements[i].joints[j];
											focusJoint = true;
										}
									}
								}
							}
						}
						if (!focusElement){
							focusedElement = null;
						}
						if (!focusJoint){
							focusedJoint = null;
						}
						lastEvent = 1;
					}
				break;
				case EventType.MouseUp:
					if (Event.current.button == 0){
						DisposeProperties();
						if (mouseCoords.x > contextPosition.x && mouseCoords.x < contextPosition.x + 128 / scale && mouseCoords.y > contextPosition.y && mouseCoords.y < contextPosition.y + 21 / scale && search == "Search..."){
							search = "";
						} else if (search == ""){
							search = "Search...";
						}
						Enquenque();
						focusElement = false;
						focusJoint = false;
						tempLine = false;
						off = false;
						ready = false;
						lineColor = 5;
						loose.Clear();
						for (int i = 0; i < elements.Count; ++i){
							if (lastEvent != 0){
								elements[i].selected = false;
								if (mouseCoords.x >= elements[i].position.x && mouseCoords.x <= elements[i].position.x + elements[i].position.width && mouseCoords.y >= elements[i].position.y && mouseCoords.y <= elements[i].position.y + elements[i].position.height){
									elements[i].selected = true;
									selectedElements.Clear();
									tempElement = Creator.List(elements[i].name);
									tempElement.position = elements[i].position;
									selectedElements.Add(tempElement);
								}
							}
						}
						if (focusedJoint != null){
							if (focusedJoint.connections == tempJoint){
								focusedJoint.connections = null;
							}
							for (int i = 0; i < elements.Count; ++i){
								for (int j = 0; j < elements[i].joints.Count; ++j){
									if (elements[i].joints[j].type == 0){
										if (mouseCoords.x >= elements[i].joints[j].coords.x && mouseCoords.x <= elements[i].joints[j].coords.x + 48 && mouseCoords.y >= elements[i].joints[j].coords.y - 4 && mouseCoords.y <= elements[i].joints[j].coords.y + 28){
											if (Connect(focusedJoint, elements[i].joints[j])){
												for (int k = 0; k < elements[i].joints[j].prohibitions.Count; ++k){
													elements[i].joints[elements[i].joints[j].prohibitions[k]].connections = null;
												}
											}
										}
									} else {
										if (mouseCoords.x >= elements[i].joints[j].coords.x - 48 && mouseCoords.x <= elements[i].joints[j].coords.x + 24 && mouseCoords.y >= elements[i].joints[j].coords.y - 4 && mouseCoords.y <= elements[i].joints[j].coords.y + 28){
											if (Connect(focusedJoint, elements[i].joints[j])){
												Serialize();
												for (int k = 0; k < focusedJoint.prohibitions.Count; ++k){
													elements[(int) focusedJoint.serial.x].joints[focusedJoint.prohibitions[k]].connections = null;
												}
											}
										}
									}
								}
							}
							AssetDatabase.Refresh();
							focusedJoint = null;
						}
						focusedElement = null;
						CheckConnections();
						lastEvent = 2;
						if (copyTime){
							copyTime = false;
							for (int i = 0; i < elements.Count; ++i){
								elements[i].selected = false;
							}
						}
						if (mouseCoords.x > 230 / scale && uSets.AutoSave){
							Write();
						}
					}
				break;
				case EventType.MouseLeaveWindow:
					selecting = false;
				break;
				case EventType.ContextClick:
					if (mouseCoords.x * scale > 228 && !copyTime){
						context = true;
						contextPosition = mouseCoords;
					}
					if (copyTime){
						copyTime = false;
						List<VisualElement> deleteList = new List<VisualElement>();
						List<int> deletedList = new List<int>();
						deleteList.Add(elements[0]);
						for (int i = 1; i < elements.Count; ++i){
							if (!elements[i].selected){
								deleteList.Add(elements[i]);
							} else {
								deletedList.Add(i);
							}
						}
						for (int i = 0; i < elements.Count; ++i){
							for (int j = 0; j < elements[i].joints.Count; ++j){
								if (elements[i].joints[j].type == 0 && elements[i].joints[j].connections != null){
									for (int k = 0; k < deletedList.Count; ++k){
										if (elements[i].joints[j].connections.serial.x == deletedList[k]){
											elements[i].joints[j].connections = null;
											break;
										}
									}
								}
							}
						}
						elements = deleteList;
						CheckConnections();
						Serialize();
						if (mouseCoords.x > 230 / scale && uSets.AutoSave){
							Write();
						}
					}
				break;
				case EventType.ScrollWheel:
					if (mouseCoords.x * scale > 228 && (!(mouseCoords.x > contextPosition.x && mouseCoords.x < contextPosition.x + 128 / scale && mouseCoords.y > contextPosition.y && mouseCoords.y < contextPosition.y + 212 / scale) || !context)){
						scroll = true;
						if (Event.current.delta.y > 0){
							scale -= 0.05f;
						} else {
							scale += 0.05f;
						}
						scale = Mathf.Clamp(scale, 0.1f, 1);
					}
				break;
				case EventType.DragUpdated:
					DragUpdated();
				break;
				case EventType.DragExited:
					try {
						DragExited();
					} catch {
						Debug.Log("Wrong file");
					}
				break;
				case EventType.KeyDown:
					if (Event.current.keyCode != KeyCode.None && (pressed.Count == 0 || (pressed.Count > 0 && pressed[pressed.Count - 1] != Event.current.keyCode))){
						pressed.Add(Event.current.keyCode);
					}
					switch (Event.current.keyCode){
						case KeyCode.Delete:
						List<VisualElement> deleteList = new List<VisualElement>();
						List<int> deletedList = new List<int>();
						deleteList.Add(elements[0]);
						for (int i = 1; i < elements.Count; ++i){
							if (!elements[i].selected){
								deleteList.Add(elements[i]);
							} else {
								deletedList.Add(i);
							}
						}
						for (int i = 0; i < elements.Count; ++i){
							for (int j = 0; j < elements[i].joints.Count; ++j){
								if (elements[i].joints[j].type == 0 && elements[i].joints[j].connections != null){
									for (int k = 0; k < deletedList.Count; ++k){
										if (elements[i].joints[j].connections.serial.x == deletedList[k]){
											elements[i].joints[j].connections = null;
											break;
										}
									}
								}
							}
						}
						elements = deleteList;
						CheckConnections();
						Serialize();
						break;
					}
				break;
				case EventType.KeyUp:
				for (int i = 0; i < pressed.Count; ++i){
					if (pressed[i] == Event.current.keyCode){
						pressed.RemoveAt(i);
						break;
					}
					switch (Event.current.keyCode){
						case KeyCode.Delete:
							if (uSets.AutoSave){
								Write();
							}
						break;
					}
				}
				break;
				case EventType.ValidateCommand:
					string eventName = Event.current.commandName;
					switch (eventName){
						case "Copy":
							Vector2 firstElement;
							selectedElements.Clear();
							Serialize();
							for (int i = 0; i < elements.Count; ++i){
								if (elements[i].selected && i != 0){
									tempElement = Creator.List(elements[i].name);
									tempElement.position = elements[i].position;
									tempElement.Values.Clear();
									for (int j = 0; j < elements[i].Values.Count; ++j){
										tempElement.Values.Add(elements[i].Values[j]);
									}
									tempElement.joints.Clear();
									for (int j = 0; j < elements[i].joints.Count; ++j){
										tempElement.joints.Add(new VisualElement.Joint(elements[i].joints[j].type, elements[i].joints[j].color));
										tempElement.joints[j].prohibitions = elements[i].joints[j].prohibitions;
									}
									copy.Add(i);
									for (int j = 0; j < elements[i].joints.Count; ++j){
										if (elements[i].joints[j].connections != null){
											serials.Add(new Vector4(i, j, elements[i].joints[j].connections.serial.x, elements[i].joints[j].connections.serial.y));
										}
									}
									selectedElements.Add(tempElement);
								}
								copyied.Add(-1);
							}
							for (int i = 0; i < copy.Count; ++i){
								copyied[copy[i]] = i;
							}
							for (int i = 0; i < serials.Count; ++i){
								if (copyied[(int) serials[i].x] != -1 && copyied[(int) serials[i].z] != -1){
									Connect(selectedElements[copyied[(int) serials[i].x]].joints[(int) serials[i].y], selectedElements[copyied[(int) serials[i].z]].joints[(int) serials[i].w]);
								}
							}
							if (selectedElements.Count > 0){
								firstElement = new Vector2(selectedElements[0].position.x, selectedElements[0].position.y);
								for (int i = 0; i < selectedElements.Count; ++i){
									if (selectedElements[i].position.x < firstElement.x){
										firstElement = new Vector2(selectedElements[i].position.x, selectedElements[i].position.y);
									}
								}
								for (int i = 0; i < elements.Count; ++i){
									elements[i].selected = false;
								}
								offsets.Clear();
								for (int i = 0; i < selectedElements.Count; ++i){
									offsets.Add(new Vector2(selectedElements[i].position.x - firstElement.x, selectedElements[i].position.y - firstElement.y));
									selectedElements[i].selected = true;
								}
								elements.AddRange(selectedElements);
								Serialize();
								CheckConnections();
								copyTime = true;
							}
						break;
					}
				break;
			}
			Repaint();
		}
		
		internal bool NothingSelected(){
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].selected){
					return false;
				}
			}
			return true;
		}
		
		internal bool CheckSelection(Rect selection, Rect element){
			List<Vector2> selections = new List<Vector2>();
			List<Vector2> elements = new List<Vector2>();
			List<Vector3> elementsVer = new List<Vector3>();
			List<Vector3> elementsHor = new List<Vector3>();
			List<Vector3> selectionsVer = new List<Vector3>();
			List<Vector3> selectionsHor = new List<Vector3>();
			elements.Add(new Vector2(element.x, element.y));
			elements.Add(new Vector2(element.x + element.width, element.y));
			elements.Add(new Vector2(element.x, element.y + element.height));
			elements.Add(new Vector2(element.x + element.width, element.y + element.height));
			if (selection.height > 0){
				if (selection.width > 0){
					selections.Add(new Vector2(selection.x, selection.y));
					selections.Add(new Vector2(selection.x + selection.width, selection.y));
					selections.Add(new Vector2(selection.x, selection.y + selection.height));
					selections.Add(new Vector2(selection.x + selection.width, selection.y + selection.height));
				} else {
					selections.Add(new Vector2(selection.x + selection.width, selection.y));
					selections.Add(new Vector2(selection.x, selection.y));
					selections.Add(new Vector2(selection.x + selection.width, selection.y + selection.height));
					selections.Add(new Vector2(selection.x, selection.y + selection.height));
				}
			} else {
				if (selection.width > 0){
					selections.Add(new Vector2(selection.x, selection.y + selection.height));
					selections.Add(new Vector2(selection.x + selection.width, selection.y + selection.height));
					selections.Add(new Vector2(selection.x, selection.y));
					selections.Add(new Vector2(selection.x + selection.width, selection.y));
				} else {
					selections.Add(new Vector2(selection.x + selection.width, selection.y + selection.height));
					selections.Add(new Vector2(selection.x, selection.y + selection.height));
					selections.Add(new Vector2(selection.x + selection.width, selection.y));
					selections.Add(new Vector2(selection.x, selection.y));
				}
			}
			elementsVer.Add(new Vector3(elements[0].x, elements[0].y, elements[2].y));
			elementsVer.Add(new Vector3(elements[1].x, elements[1].y, elements[3].y));
			elementsHor.Add(new Vector3(elements[0].x, elements[0].y, elements[1].x));
			elementsHor.Add(new Vector3(elements[2].x, elements[2].y, elements[3].x));
			selectionsVer.Add(new Vector3(selections[0].x, selections[0].y, selections[2].y));
			selectionsVer.Add(new Vector3(selections[1].x, selections[1].y, selections[3].y));
			selectionsHor.Add(new Vector3(selections[0].x, selections[0].y, selections[1].x));
			selectionsHor.Add(new Vector3(selections[2].x, selections[2].y, selections[3].x));
			if (selectionsVer[0].x >= elementsHor[0].x && selectionsVer[0].x <= elementsHor[0].z && selectionsVer[0].y <= elementsHor[0].y && selectionsVer[0].z >= elementsHor[0].y){
				return true;
			}
			if (selectionsVer[1].x >= elementsHor[0].x && selectionsVer[1].x <= elementsHor[0].z && selectionsVer[1].y <= elementsHor[0].y && selectionsVer[1].z >= elementsHor[0].y){
				return true;
			}
			if (selectionsVer[1].x >= elementsHor[1].x && selectionsVer[1].x <= elementsHor[1].z && selectionsVer[1].y <= elementsHor[1].y && selectionsVer[1].z >= elementsHor[1].y){
				return true;
			}
			if (selectionsVer[0].x >= elementsHor[1].x && selectionsVer[0].x <= elementsHor[1].z && selectionsVer[0].y <= elementsHor[1].y && selectionsVer[0].z >= elementsHor[1].y){
				return true;
			}
			if (elementsVer[0].x >= selectionsHor[0].x && elementsVer[0].x <= selectionsHor[0].z && elementsVer[0].y <= selectionsHor[0].y && elementsVer[0].z >= selectionsHor[0].y){
				return true;
			}
			if (elementsVer[1].x >= selectionsHor[0].x && elementsVer[1].x <= selectionsHor[0].z && elementsVer[1].y <= selectionsHor[0].y && elementsVer[1].z >= selectionsHor[0].y){
				return true;
			}
			if (elementsVer[1].x >= selectionsHor[1].x && elementsVer[1].x <= selectionsHor[1].z && elementsVer[1].y <= selectionsHor[1].y && elementsVer[1].z >= selectionsHor[1].y){
				return true;
			}
			if (elementsVer[0].x >= selectionsHor[1].x && elementsVer[0].x <= selectionsHor[1].z && elementsVer[0].y <= selectionsHor[1].y && elementsVer[0].z >= selectionsHor[1].y){
				return true;
			}
			if (selections[0].x <= elements[0].x && selections[1].x >= elements[0].x && selections[0].y <= elements[0].y && selections[2].y >= elements[0].y){
				return true;
			}
			return false;
		}
		
		internal void CheckConnections(){
			for (int i = 0; i < elements.Count; ++i){
				for (int j = 0; j < elements[i].joints.Count; ++j){
					if (elements[i].joints[j].connections != null && elements[i].joints[j].type == 0 && elements[i].joints[j].connections.type == 0){
						elements[i].joints[j].connections.connections = null;
						elements[i].joints[j].connections = null;
					}
				}
			}
			List<Vector4> rights = new List<Vector4>();
			for (int i = 0; i < elements.Count; ++i){
				for (int j = 0; j < elements[i].joints.Count; ++j){
					if (elements[i].joints[j].type == 1){
						rights.Add(new Vector4(i, j, 0, 0));
						elements[i].joints[j].connected = elements[i].joints[j].connections == tempJoint && tempJoint != null;
					}
				}
			}
			for (int i = 0; i < elements.Count; ++i){
				for (int j = 0; j < elements[i].joints.Count; ++j){
					if (elements[i].joints[j].type == 0){
						if (elements[i].joints[j].connections != null){
							elements[i].joints[j].connections.connected = true;
							elements[i].joints[j].connected = true;
							for (int k = 0; k < rights.Count; ++k){
								if (elements[i].joints[j].connections.serial.x == rights[k].x && elements[i].joints[j].connections.serial.y == rights[k].y){
									rights[k] = new Vector4(rights[k].x, rights[k].y, rights[k].z + 1, rights[k].w);
									rights[k] = new Vector4(rights[k].x, rights[k].y, rights[k].z, elements[i].joints[j].color);
								}
							}
							for (int k = 0; k < elements.Count; ++k){
								for (int l = 0; l < elements[k].joints.Count; ++l){
									if (elements[i].joints[j].connections == elements[k].joints[l] || elements[i].joints[j].connections == tempJoint){
										elements[i].joints[j].connected = true;
										break;
									} else {
										elements[i].joints[j].connected = false;
									}
								}
								if (elements[i].joints[j].connected){
									break;
								}
							}
							if (!elements[i].joints[j].connected){
								elements[i].joints[j].connections = null;
							}
						} else {
							elements[i].joints[j].connected = false;
						}
					}
				}
			}
		}
		
		internal void DrawLine(Vector2 vec1, Vector2 vec2, int type, int type2, float width){
			vec[0] = new Vector3(vec1.x + 10, vec1.y + 12, 0);
			vec[1] = new Vector3(vec2.x + 10, vec2.y + 12, 0);
			vec[2] = new Vector3(vec1.x + Mathf.Abs(vec1.y - vec2.y) / (0.1f * Mathf.Sqrt(Mathf.Abs(vec1.y - vec2.y + 1))), vec1.y + 12, 0);
			vec[3] = new Vector3(vec2.x - Mathf.Abs(vec1.y - vec2.y) / (0.1f * Mathf.Sqrt(Mathf.Abs(vec1.y - vec2.y + 1))), vec2.y + 12, 0);
			if (flow){
				points = Handles.MakeBezierPoints(vec[0], vec[1], vec[2], vec[3], 32).ToList();
			}
			if (width != 4 || type == type2) {
				lineTexture = width == 4 ? lineTextures[1] : lineTextures[0];
				switch (type){
					case 0:
					Handles.DrawBezier(vec[0], vec[1], vec[2], vec[3], Color.red, lineTexture, width);
					break;
					case 1:
					Handles.DrawBezier(vec[0], vec[1], vec[2], vec[3], Color.green, lineTexture, width);
					break;
					case 2:
					Handles.DrawBezier(vec[0], vec[1], vec[2], vec[3], beziers[type + 3], lineTexture, width);
					break;
					case 3:
					Handles.DrawBezier(vec[0], vec[1], vec[2], vec[3], Color.white, lineTexture, width);
					break;
					case 4:
					Handles.DrawBezier(vec[0], vec[1], vec[2], vec[3], Color.cyan, lineTexture, width);
					break;
					case 5:
					Handles.DrawBezier(vec[0], vec[1], vec[2], vec[3], Color.clear, lineTexture, width);
					break;
				}
			} else {
				lineTexture = lineTextures[1];
				if (!flow){
					points = Handles.MakeBezierPoints(vec[0], vec[1], vec[2], vec[3], 32).ToList();
				}
				for (int i = 0; i < points.Count - 1; ++i){
					Handles.color = Color.Lerp(beziers[type2 + 3], beziers[type + 3], i / 32f);
					twoPoints[0] = points[i];
					twoPoints[1] = points[i + 1];
					Handles.DrawAAPolyLine(lineTexture, width, twoPoints);
				}
				Handles.color = Color.white;
			}
			if (flow){
				pointsDistance = Vector3.Distance(points[0], points[31]);
				flowPoints = (int) (pointsDistance / 200);
				if (flowPoints > 12){
					flowPoints = 16;
				} else if (flowPoints > 6){
					flowPoints = 8;
				} else if (flowPoints > 3){
					flowPoints = 4;
				} else if (flowPoints > 1){
					flowPoints = 2;
				} else {
					flowPoints = 1;
				}
				time = (float) EditorApplication.timeSinceStartup / (Mathf.Sqrt(pointsDistance) / 750);
				time = time % (32 / flowPoints);
				if (time >= 0 && !tempLine){
					switch (flowPoints){
						case 1:
							GUI.DrawTexture(new Rect(points[(int) time].x - 12, points[(int) time].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][(int) (time / 8)]);
						break;
						case 2:
							GUI.DrawTexture(new Rect(points[(int) time].x - 12, points[(int) time].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][(int) (time / 8)]);
							GUI.DrawTexture(new Rect(points[(int) (16 + time)].x - 12, points[(int) (16 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][(int) (2 + time / 8)]);
						break;
						case 4:
							GUI.DrawTexture(new Rect(points[(int) time].x - 12, points[(int) time].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][0]);
							GUI.DrawTexture(new Rect(points[(int) (8 + time)].x - 12, points[(int) (8 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][1]);
							GUI.DrawTexture(new Rect(points[(int) (16 + time)].x - 12, points[(int) (16 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][2]);
							GUI.DrawTexture(new Rect(points[(int) (24 + time)].x - 12, points[(int) (24 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][3]);
						break;
						case 8:
							GUI.DrawTexture(new Rect(points[(int) time].x - 12, points[(int) time].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][0]);
							GUI.DrawTexture(new Rect(points[(int) (4 + time)].x - 12, points[(int) (4 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][0]);
							GUI.DrawTexture(new Rect(points[(int) (8 + time)].x - 12, points[(int) (8 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][1]);
							GUI.DrawTexture(new Rect(points[(int) (12 + time)].x - 12, points[(int) (12 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][1]);
							GUI.DrawTexture(new Rect(points[(int) (16 + time)].x - 12, points[(int) (16 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][2]);
							GUI.DrawTexture(new Rect(points[(int) (20 + time)].x - 12, points[(int) (20 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][2]);
							GUI.DrawTexture(new Rect(points[(int) (24 + time)].x - 12, points[(int) (24 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][3]);
							GUI.DrawTexture(new Rect(points[(int) (28 + time)].x - 12, points[(int) (28 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][3]);
						break;
						case 16:
							GUI.DrawTexture(new Rect(points[(int) time].x - 12, points[(int) time].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][0]);
							GUI.DrawTexture(new Rect(points[(int) (2 + time)].x - 12, points[(int) (2 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][0]);
							GUI.DrawTexture(new Rect(points[(int) (4 + time)].x - 12, points[(int) (4 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][0]);
							GUI.DrawTexture(new Rect(points[(int) (6 + time)].x - 12, points[(int) (6 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][0]);
							GUI.DrawTexture(new Rect(points[(int) (8 + time)].x - 12, points[(int) (8 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][1]);
							GUI.DrawTexture(new Rect(points[(int) (10 + time)].x - 12, points[(int) (10 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][1]);
							GUI.DrawTexture(new Rect(points[(int) (12 + time)].x - 12, points[(int) (12 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][1]);
							GUI.DrawTexture(new Rect(points[(int) (14 + time)].x - 12, points[(int) (14 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][1]);
							GUI.DrawTexture(new Rect(points[(int) (16 + time)].x - 12, points[(int) (16 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][2]);
							GUI.DrawTexture(new Rect(points[(int) (18 + time)].x - 12, points[(int) (18 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][2]);
							GUI.DrawTexture(new Rect(points[(int) (20 + time)].x - 12, points[(int) (20 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][2]);
							GUI.DrawTexture(new Rect(points[(int) (22 + time)].x - 12, points[(int) (22 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][2]);
							GUI.DrawTexture(new Rect(points[(int) (24 + time)].x - 12, points[(int) (24 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][2]);
							GUI.DrawTexture(new Rect(points[(int) (26 + time)].x - 12, points[(int) (26 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][3]);
							GUI.DrawTexture(new Rect(points[(int) (28 + time)].x - 12, points[(int) (28 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][3]);
							GUI.DrawTexture(new Rect(points[(int) (30 + time)].x - 12, points[(int) (30 + time)].y - 12, 24, 24), type == type2 ? flowDots[type][type][0] : flowDots[type2][type][3]);
						break;
					}
				}
			}
		}
		
		internal bool Connect(VisualElement.Joint joint1, VisualElement.Joint joint2){
			joint1.connections = joint2;
			joint2.connections = joint1;
			CheckConnections();
			return true;
		}
		
		internal void Disconnect(VisualElement.Joint joint1, VisualElement.Joint joint2){
			if (joint1.type == 0){
				joint1.connections = null;
			}
		}
		
		internal void Serialize(){
			for (int i = 0; i < allElements.Count; ++i){
				for (int j = 0; j < allElements[i].Count; ++j){
					for (int k = 0; k < allElements[i][j].joints.Count; ++k){
						allElements[i][j].joints[k].serial = new Vector2(j, k);
					}
				}
			}
		}
		
		internal void DrawBackground(){
			if (backgrounds.Count != 0){
				for (int i = 0; i < position.width / 128; ++i){
					for (int j = 0; j < position.height / 128; ++j){
						GUI.DrawTexture(new Rect(i * 128, j * 128, 128, 128), backgrounds[0]);
					}
				}
				GUI.DrawTexture(new Rect(228, 0, position.width - 228, position.height), backgrounds[1]);
			}
		}
		
		internal void DrawBeziers(){
			for (int i = 0; i < elements.Count; ++i){
				for (int j = 0; j < elements[i].joints.Count; ++j){
					if (elements[i].joints[j].type == 0 && elements[i].joints[j].connections != null && elements[i].joints[j].connections != tempJoint){
						DrawLine(elements[i].joints[j].connections.coords, elements[i].joints[j].coords, elements[i].joints[j].color, elements[i].joints[j].connections.color, 4);
					}
				}
			}
			if (tempLine){
				for (int i = 0; i < elements.Count; ++i){
					for (int j = 0; j < elements[i].joints.Count; ++j){
						if (elements[i].joints[j].connections != null && elements[i].joints[j].connections == tempJoint){
							if (elements[i].joints[j].type == 0){
								DrawLine(elements[i].joints[j].connections.coords - (new Vector2(8, 12)), elements[i].joints[j].coords, elements[i].joints[j].color, elements[i].joints[j].color, 4);
								DrawLine(elements[i].joints[j].connections.coords - (new Vector2(8, 12)), elements[i].joints[j].coords, lineColor, lineColor, 8);
								if (lineColor == 1){
									arrowColor = 5;
								} else if (lineColor == 0){
									arrowColor = 6;
								} else {
									arrowColor = elements[i].joints[j].color;
								}
								GUI.DrawTexture(new Rect(tempJoint.coords.x - 8, tempJoint.coords.y - 8, 16, 16), arrows[0][arrowColor]);
							} else {
								DrawLine(elements[i].joints[j].coords, elements[i].joints[j].connections.coords - (new Vector2(12, 12)), elements[i].joints[j].color, elements[i].joints[j].color, 4);
								DrawLine(elements[i].joints[j].coords, elements[i].joints[j].connections.coords - (new Vector2(12, 12)), lineColor, lineColor, 8);
								if (lineColor == 1){
									arrowColor = 5;
								} else if (lineColor == 0){
									arrowColor = 6;
								} else {
									arrowColor = elements[i].joints[j].color;
								}
								GUI.DrawTexture(new Rect(tempJoint.coords.x - 12, tempJoint.coords.y - 65 / 6.84f, 96 / 3.42f, 65 / 3.42f), arrows[1][arrowColor]);
							}
						}
					}
				}
			}
			for (int i = 0; i < loose.Count; ++i){
				if (loose[i] != null && loose[i].connections != null){
					DrawLine(loose[i].connections.coords - (new Vector2(0, 1)), loose[i].coords - (new Vector2(0, 1)), 0, 0, 8);
				}
			}
		}
		
		internal void DrawElements(){
			for (int i = 0; i < elements.Count; ++i){
				elements[i].Show();
			}
		}
		
		internal void DrawLabels(){
			for (int i = 0; i < elements.Count; ++i){
				elements[i].scale = scale;
				elements[i].ShowLabels();
			}
		}
		
		internal void CheckUniques(){
			for (int i = 0; i < properties.Count - 1; ++i){
				if (properties[properties.Count - 1].unique == properties[i].unique){
					properties[properties.Count - 1].unique = UnityEngine.Random.Range(1, 4096);
					CheckUniques();
					break;
				}
			}
		}
		
		internal void OrganizePasses(){
			if (allElements.Count > 0){
				if (pass >= allElements.Count){
					pass = allElements.Count - 1;
				}
				if (pass != currentPass){
					currentPass = pass;
					elements = allElements[pass];
					DisposeProperties();
					if (elements.Count == 0){
						elements.Add(new CameraOutput());
						elements[0].position.x = 272 / scale + 2 * 400;
						elements[0].position.y = 48 / scale;
						elements.Add(new CameraInput());
						elements[1].position.x = 272 / scale + 1 * 400;
						elements[1].position.y = 48 / scale;
						elements.Add(new StereoUV());
						elements[2].position.x = 272 / scale;
						elements[2].position.y = 48 / scale;
						Connect(elements[1].joints[3], elements[0].joints[3]);
						Connect(elements[2].joints[2], elements[1].joints[6]);
						CheckConnections();
					}
				}
				renderQueue.passOptions.Clear();
				for (int i = 0; i < allElements.Count; ++i){
					renderQueue.passOptions.Add(i.ToString());
				}
			}
			for (int i = 0; i < renderQueue.passes.Count; ++i){
				renderQueue.passes[i].PassLabels = renderQueue.passOptions.ToArray();
				renderQueue.passes[i].InputLabels = renderQueue.inputOptions.ToArray();
				renderQueue.passes[i].OutputLabels = renderQueue.outputOptions.ToArray();
				renderQueue.passes[i].VariableLabels = renderQueue.variableOptions.ToArray();
			}
		}
		
		internal void DisposePasses(){
			for (int i = 0; i < renderQueue.passes.Count; ++i){
				renderQueue.passes[i].position = new Rect(0, (i + 1) * 36, 212, 16);
			}
		}
		
		internal void DisposeProperties(){
			for (int i = 0; i < specifiedProperties.Count; ++i){
				specifiedProperties[i].Clear();
			}
			for (int i = 0; i < properties.Count; ++i){
				switch (properties[i].type){
					case "IntField":
						specifiedProperties[0].Add(properties[i]);
					break;
					case "FloatField":
						specifiedProperties[1].Add(properties[i]);
					break;
					case "IntSliderField":
						specifiedProperties[2].Add(properties[i]);
					break;
					case "FloatSliderField":
						specifiedProperties[3].Add(properties[i]);
					break;
					case "ColorField":
						specifiedProperties[4].Add(properties[i]);
					break;
					case "Vector2Field":
						specifiedProperties[5].Add(properties[i]);
					break;
					case "Vector3Field":
						specifiedProperties[6].Add(properties[i]);
					break;
					case "Vector4Field":
						specifiedProperties[7].Add(properties[i]);
					break;
					case "Texture2DField":
						specifiedProperties[8].Add(properties[i]);
					break;
					case "SplineField":
						specifiedProperties[9].Add(properties[i]);
					break;
					case "BoolField":
						specifiedProperties[10].Add(properties[i]);
					break;
				}
				properties[i].serial = i;
			}
			for (int i = 0; i < popupOptions.Count; ++i){
				popupOptions[i].Clear();
				popupOptions[i].Add("ZERO (0)");
				for (int j = 0; j < specifiedProperties[i].Count; ++j){
					popupOptions[i].Add(specifiedProperties[i][j].Values[0]);
				}
			}
			for (int i = 0; i < popupUniques.Count; ++i){
				popupUniques[i].Clear();
				popupUniques[i].Add(0);
				for (int j = 0; j < specifiedProperties[i].Count; ++j){
					popupUniques[i].Add(specifiedProperties[i][j].unique);
				}
			}
			for (int i = 0; i < elements.Count; ++i){
				switch (elements[i].name){
					case "_Int":
						elements[i].options = popupOptions[0];
						elements[i].uniques = popupUniques[0];
						elements[i].uniqueChanged = true;
					break;
					case "_Float":
						elements[i].options = popupOptions[1];
						elements[i].uniques = popupUniques[1];
						elements[i].uniqueChanged = true;
					break;
					case "_IntSlider":
						elements[i].options = popupOptions[2];
						elements[i].uniques = popupUniques[2];
						elements[i].uniqueChanged = true;
					break;
					case "_FloatSlider":
						elements[i].options = popupOptions[3];
						elements[i].uniques = popupUniques[3];
						elements[i].uniqueChanged = true;
					break;
					case "_Color":
						elements[i].options = popupOptions[4];
						elements[i].uniques = popupUniques[4];
						elements[i].uniqueChanged = true;
					break;
					case "_Vector2":
						elements[i].options = popupOptions[5];
						elements[i].uniques = popupUniques[5];
						elements[i].uniqueChanged = true;
					break;
					case "_Vector3":
						elements[i].options = popupOptions[6];
						elements[i].uniques = popupUniques[6];
						elements[i].uniqueChanged = true;
					break;
					case "_Vector4":
						elements[i].options = popupOptions[7];
						elements[i].uniques = popupUniques[7];
						elements[i].uniqueChanged = true;
					break;
					case "_Texture2D":
						elements[i].options = popupOptions[8];
						elements[i].uniques = popupUniques[8];
						elements[i].uniqueChanged = true;
					break;
					case "_Spline":
						elements[i].options = popupOptions[9];
						elements[i].uniques = popupUniques[9];
						elements[i].uniqueChanged = true;
					break;
					case "_Bool":
						elements[i].options = popupOptions[10];
						elements[i].uniques = popupUniques[10];
						elements[i].uniqueChanged = true;
					break;
				}
			}
			renderQueue.variableOptions.Clear();
			renderQueue.variableOptions.Add("None");
			for (int i = 0; i < specifiedProperties[0].Count; ++i){
				renderQueue.variableOptions.Add(specifiedProperties[0][i].Values[0]);
			}
			for (int i = 0; i < specifiedProperties[2].Count; ++i){
				renderQueue.variableOptions.Add(specifiedProperties[2][i].Values[0]);
			}
			DisposeTemps();
		}
		
		internal void DisposeTemps(){
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].name == "TempTex"){
					elements[i].options.Clear();
					elements[i].options.Add("ZERO (0)");
					for (int j = 1; j < renderQueue.outputOptions.Count; ++j){
						elements[i].options.Add(renderQueue.outputOptions[j]);	
					}
				}
			}
		}
		
		internal void OnEnable(){
			++uSets.Starts;
			System.IO.File.WriteAllText("Assets/Post Processing Ultimate/Editor/config.usets", JsonUtility.ToJson(uSets));
			inputs = new ReorderableList(properties, typeof(VisualProperty), true, false, false, false);
			inputs.headerHeight = 0;
			inputs.footerHeight  = 0;
			inputs.showDefaultBackground = false;
			inputs.drawHeaderCallback = (Rect rect) => {
				EditorGUI.LabelField(rect, "Global Variables");
			};
			inputs.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
				if (properties.Count > index){
					properties[index].position.y = rect.y;
					properties[index].Show();
					if (GUI.Button(new Rect(0, properties[index].position.y + properties[index].position.height - 8, 212, 16), new GUIContent("DELETE"), stylesUI[5])){
						properties.RemoveAt(index);
						DisposeProperties();
					}
				}
			};
			inputs.elementHeightCallback = (int index) => {
				return properties.Count > index ? properties[index].position.height + 8 : 0;
			};
		}
		
		internal virtual void DrawToolbar(){
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
			toolbarScroll = GUI.BeginScrollView(new Rect(0, 0, 228, position.height), toolbarScroll, new Rect(0, 0, 228, 4 * 36 + 11 * 18 + 9 + propertiesWindow + passesWindow + 30), false, false, GUIStyle.none, GUIStyle.none);
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
			EditorGUI.LabelField(new Rect(4, 4 * 36 + 1 * 18 + 9, 220, 18), new GUIContent(path == null || path == "" ? "SHADER FILE NOT SELECTED" : path, "Current shader file path"), stylesUI[0]);
			stylesUI[0].alignment = TextAnchor.MiddleLeft;
			GUI.DrawTexture(new Rect(8, 4 * 36 + 2 * 18 + 18, 208, 3), toolbarTextures[11]);
			EditorGUI.LabelField(new Rect(8, 4 * 36 + 3 * 18 + 9, 96, 18), new GUIContent("EVENT:", "Destined Injection Point"), stylesUI[0]);
			settings.Values[0] = EditorGUI.Popup(new Rect (96, 4 * 36 + 3 * 18 + 9, 124, 16), Int32.Parse(settings.Values[0]), targets, toolbar[0]).ToString();
			EditorGUI.LabelField(new Rect(8, 4 * 36 + 4 * 18 + 9, 96, 18), new GUIContent("NAME:", "Name of the shader file"), stylesUI[0]);
			settings.Values[1] = EditorGUI.TextField(new Rect (96, 4 * 36 + 4 * 18 + 9, 124, 16), settings.Values[1], toolbar[1]).Replace(" ", "");
			EditorGUI.LabelField(new Rect(8, 4 * 36 + 5 * 18 + 9, 96, 18), new GUIContent("FLOW:", "Visual representation of data flow"), stylesUI[0]);
			flow = EditorGUI.Toggle(new Rect(96, 4 * 36 + 5 * 18 + 9, 16, 16), flow, toolbar[2]);
			EditorGUI.LabelField(new Rect(8, 4 * 36 + 6 * 18 + 9, 96, 18), new GUIContent("PRECISION:", "Destined shader precision"), stylesUI[0]);
			settings.Values[3] = EditorGUI.Popup(new Rect(96, 4 * 36 + 6 * 18 + 9, 124, 16), Int32.Parse(settings.Values[3]), options, toolbar[0]).ToString();
			EditorGUI.LabelField(new Rect(8, 4 * 36 + 7 * 18 + 9, 96, 18), new GUIContent("PASS:", "Select current shader pass"), stylesUI[0]);
			pass = EditorGUI.Popup(new Rect(96, 4 * 36 + 7 * 18 + 9, 124, 16), pass, renderQueue.passOptions.ToArray(), toolbar[0]);
			if (renderQueue.passOptions.Count == 1){
				if (GUI.Button(new Rect(8, 4 * 36 + 8 * 18 + 9, 212, 16), new GUIContent("ADD"), stylesUI[5])){
					allElements.Add(new List<VisualElement>());
					pass = allElements.Count - 1;
				}
			} else {
				if (GUI.Button(new Rect(8, 4 * 36 + 8 * 18 + 9, 98, 16), new GUIContent("Remove"), stylesUI[5])){
					allElements.RemoveAt(pass);
					pass = 0;
				}
				if (GUI.Button(new Rect(122, 4 * 36 + 8 * 18 + 9, 98, 16), new GUIContent("ADD"), stylesUI[5])){
					allElements.Add(new List<VisualElement>());
					pass = allElements.Count - 1;
				}
			}
			OrganizePasses();
			GUI.DrawTexture(new Rect(8, 4 * 36 + 10 * 18, 208, 3), toolbarTextures[11]);
			GUI.BeginGroup(new Rect(8, 4 * 36 + 10 * 18 + 9, 212, passesWindow));
			passesWindow = 59;
			renderQueue.tempTextures = (int) Mathf.Max(EditorGUI.IntField(new Rect(106, 0, 212, 16), renderQueue.tempTextures, toolbar[1]), 0);
			EditorGUI.LabelField(new Rect(0, 0, 104, 16), "TEMP TEXTURES:", stylesUI[0]);
			EditorGUI.LabelField(new Rect(0, 18, 50, 16), "INPUT", stylesUI[0]);
			EditorGUI.LabelField(new Rect(54, 18, 50, 16), "OUTPUT", stylesUI[0]);
			EditorGUI.LabelField(new Rect(108, 18, 50, 16), "PASS", stylesUI[0]);
			EditorGUI.LabelField(new Rect(162, 18, 50, 16), "LOOPS", stylesUI[0]);
			if (renderQueue.inputOptions.Count != renderQueue.tempTextures + 1){
				renderQueue.inputOptions.Clear();
				renderQueue.outputOptions.Clear();
				renderQueue.inputOptions.Add("Game");
				renderQueue.outputOptions.Add("Screen");
				for (int i = 0; i < renderQueue.tempTextures; ++i){
					renderQueue.inputOptions.Add("tempRT" + i);
					renderQueue.outputOptions.Add("tempRT" + i);
				}
				DisposeTemps();
			}
			if (renderQueue.passes.Count == 1){
				renderQueue.passes[0].Show();
				passesWindow += 36;
			} else {
				for (int i = 0; i < renderQueue.passes.Count; ++i){
					renderQueue.passes[i].Show();
					if (GUI.Button(new Rect(0, renderQueue.passes[i].position.y + 18, 160, 16), new GUIContent("DELETE"), stylesUI[5])){
						renderQueue.passes.RemoveAt(i);
						DisposePasses();
						break;
					}
					passesWindow += 36;
				}
			}
			if (GUI.Button(new Rect(0, passesWindow - 16, 212, 16), new GUIContent("ADD"), stylesUI[5])){
				renderQueue.passes.Add(new VisualPass());
				DisposePasses();
			}
			GUI.EndGroup();
			GUI.DrawTexture(new Rect(8, 4 * 36 + 11 * 18 + passesWindow, 208, 3), toolbarTextures[11]);
			if (inputs.count != properties.Count){
				OnEnable();
			}
			try {
				GUILayout.BeginArea(new Rect(8, 4 * 36 + 11 * 18 + 9 + passesWindow, 212, propertiesWindow));
				inputs.DoLayoutList();
				GUILayout.EndArea();
			} catch {}
			propertiesWindow = 0;
			for (int i = 0; i < properties.Count; ++i){
				propertiesWindow += properties[i].position.height + 8;
			}
			GUI.BeginGroup(new Rect(8, 4 * 36 + 11 * 18 + 9 + passesWindow + propertiesWindow + 4, 212, 30));
			newProp = EditorGUI.Popup(new Rect(0, 0, 98, 16), newProp, propNames, toolbar[0]);
			if (GUI.Button(new Rect(114, 0, 98, 16), new GUIContent("ADD"), stylesUI[5])){
				switch (newProp){
					case 0:
						properties.Add(new IntField());
					break;
					case 1:
						properties.Add(new FloatField());
					break;
					case 2:
						properties.Add(new IntSliderField());
					break;
					case 3:
						properties.Add(new FloatSliderField());
					break;
					case 4:
						properties.Add(new ColorField());
					break;
					case 5:
						properties.Add(new Vector2Field());
					break;
					case 6:
						properties.Add(new Vector3Field());
					break;
					case 7:
						properties.Add(new Vector4Field());
					break;
					case 8:
						properties.Add(new Texture2DField());
					break;
					case 9:
						properties.Add(new SplineField());
					break;
					case 10:
						properties.Add(new BoolField());
					break;
				}
				CheckUniques();
				DisposeProperties();
			}
			GUI.EndGroup();
			GUI.EndScrollView();
		}
		
		internal void DrawSelection(){
			if (selecting && !context){
				EditorGUI.DrawRect(selectingBox, new Color(1, 1, 1, 0.5f));
			}
		}
		
		internal virtual void DrawContext(Vector2 coords){
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
				EditorGUI.DrawRect(new Rect(contextScroll.x, contextScroll.y, 128, visibleHeight), new Color(0.5f, 0.5f, 0.5f, 0.9f));
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
				if (CheckSearch(true, 1, 0)){
					foldouts[1] = EditorGUILayout.Foldout(foldouts[1], "Camera", stylesContext[1]);
				}
				if (foldouts[1] || searching){
					for (int i = 0; i < contextLabels[1].Count; ++i){
						if (CheckSearch(false, 1, i)){
							if (GUILayout.Button(new GUIContent(contextLabels[1][i], Tooltips.List(contextLabels[1][i])), stylesContext[0], GUILayout.Width(128))){
								AddElement(contextLabels[1][i]);
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
				if (CheckSearch(true, 10, 0)){
					foldouts[10] = EditorGUILayout.Foldout(foldouts[10], "Custom", stylesContext[1]);
				}
				if (foldouts[10] || searching){
					for (int i = 0; i < contextLabels[10].Count; ++i){
						if (CheckSearch(false, 10, i)){
							if (GUILayout.Button(new GUIContent(contextLabels[10][i], Tooltips.List(contextLabels[10][i])), stylesContext[0], GUILayout.Width(128))){
								AddElement(contextLabels[10][i]);
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
				if (CheckSearch(true, 0, 0)){
					foldouts[0] = EditorGUILayout.Foldout(foldouts[0], "Input", stylesContext[1]);
				}
				if (foldouts[0] || searching){
					for (int i = 0; i < contextLabels[0].Count; ++i){
						if (CheckSearch(false, 0, i)){
							if (GUILayout.Button(new GUIContent(contextLabels[0][i], Tooltips.List(contextLabels[0][i])), stylesContext[0], GUILayout.Width(128))){
								AddElement(contextLabels[0][i]);
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
		
		internal bool CheckSearch(bool foldout, int x, int y){
			output = true;
			if (!searching){
				output = true;
			} else if (foldout){
				for (int i = 0; i < contextLabels[x].Count; ++i){
					if (contextLabels[x][i].ToLower().Contains(search.ToLower())){
						output = true;
						break;
					} else {
						output = false;
						switch (x){
							case 0:
							output = search.ToLower() == "input";
							break;
							case 1:
							output = search.ToLower() == "camera";
							break;
							case 2:
							output = search.ToLower() == "value";
							break;
							case 3:
							output = search.ToLower() == "hlsl function";
							break;
							case 4:
							output = search.ToLower() == "arithmetic";
							break;
							case 5:
							output = search.ToLower() == "constant";
							break;
							case 6:
							output = search.ToLower() == "data";
							break;
							case 7:
							output = search.ToLower() == "logic";
							break;
							case 8:
							output = search.ToLower() == "variable";
							break;
							case 9:
							output = search.ToLower() == "input";
							break;
							case 10:
							output = search.ToLower() == "custom";
							break;
							case 11:
							output = search.ToLower() == "pps function";
							break;
							case 12:
							output = search.ToLower() == "predefined macro";
							break;
						}
					}
				}
			} else {
				if (contextLabels[x][y].ToLower().Contains(search.ToLower())){
					output = true;
				} else {
					output = false;
					switch (x){
						case 0:
						output = search.ToLower() == "input";
						break;
						case 1:
						output = search.ToLower() == "camera";
						break;
						case 2:
						output = search.ToLower() == "value";
						break;
						case 3:
						output = search.ToLower() == "hlsl function";
						break;
						case 4:
						output = search.ToLower() == "arithmetic";
						break;
						case 5:
						output = search.ToLower() == "constant";
						break;
						case 6:
						output = search.ToLower() == "data";
						break;
						case 7:
						output = search.ToLower() == "logic";
						break;
						case 8:
						output = search.ToLower() == "variable";
						break;
						case 9:
						output = search.ToLower() == "input";
						break;
						case 10:
						output = search.ToLower() == "custom";
						break;
						case 11:
						output = search.ToLower() == "pps function";
						break;
						case 12:
						output = search.ToLower() == "predefined macro";
						break;
					}
				}
			}
			return output;
		}
		
		internal void BeforeScroll(){
			scroll = false;
			distance[0] = new Vector2(scale * mouseCoords.x - elements[0].position.x, scale * mouseCoords.y - elements[0].position.y);
			distance[2] = new Vector2(scale, 0);
		}
		
		internal void AfterScroll(){
			if (scroll){
				scroll = false;
				distance[1] = new Vector2(scale * mouseCoords.x - elements[0].position.x, scale * mouseCoords.y - elements[0].position.y);
				distance[0] = distance[1] - distance[0];
				distance[0] = distance[0] / scale;
				for (int i = 0; i < elements.Count; ++i){
					elements[i].position = new Rect(elements[i].position.x - distance[0].x, elements[i].position.y - distance[0].y, elements[i].position.width, elements[i].position.height);
				}
				contextPosition /= scale / distance[2].x;
			}
		}
		
		internal void Import(){
			toolbar[0].clipping = TextClipping.Clip;
			toolbar[0].padding = new RectOffset(4, 12, 2, 0);
			toolbar[0].normal.background = Resources.Load("Icons/Popup") as Texture2D;
			toolbar[0].normal.textColor = new Color(0.7f, 0.7f, 0.7f);
			toolbar[0].active.background = Resources.Load("Icons/Popup") as Texture2D;
			toolbar[0].active.textColor = new Color(0.7f, 0.7f, 0.7f);
			toolbar[0].focused.background = Resources.Load("Icons/Popup") as Texture2D;
			toolbar[0].focused.textColor = new Color(0.7f, 0.7f, 0.7f);
			toolbar.Add(new GUIStyle(toolbar[0]));
			toolbar[1].padding = new RectOffset(4, 4, 2, 0);
			toolbar[1].normal.background = Resources.Load("Nodes/Field") as Texture2D;
			toolbar[1].active.background = Resources.Load("Nodes/Field") as Texture2D;
			toolbar[1].focused.background = Resources.Load("Nodes/Field") as Texture2D;
			toolbar.Add(new GUIStyle(EditorStyles.toggle));
			toolbar[2].focused.background = Resources.Load("Icons/ToggleOff") as Texture2D;
			toolbar[2].normal.background = Resources.Load("Icons/ToggleOff") as Texture2D;
			toolbar[2].active.background = Resources.Load("Icons/ToggleOff") as Texture2D;
			toolbar[2].onFocused.background = Resources.Load("Icons/ToggleOn") as Texture2D;
			toolbar[2].onNormal.background = Resources.Load("Icons/ToggleOn") as Texture2D;
			toolbar[2].onActive.background = Resources.Load("Icons/ToggleOff") as Texture2D;
			//==========ContextLabels==========
			contextLabels.Clear();
			for (int i = 0; i < 13; ++i){
				contextLabels.Add(new List<string>());
			}
			//=====Input=====
			contextLabels[0].Add("_Int");
			contextLabels[0].Add("_Float");
			contextLabels[0].Add("_IntSlider");
			contextLabels[0].Add("_FloatSlider");
			contextLabels[0].Add("_Color");
			contextLabels[0].Add("_Vector2");
			contextLabels[0].Add("_Vector3");
			contextLabels[0].Add("_Vector4");
			contextLabels[0].Add("_Texture2D");
			contextLabels[0].Add("_Spline");
			contextLabels[0].Add("_Bool");
			//=====Camera=====
			contextLabels[1].Add("CameraInput");
			contextLabels[1].Add("CameraDepth");
			contextLabels[1].Add("DefaultUV");
			contextLabels[1].Add("StereoUV");
			contextLabels[1].Add("SpecialTex");
			contextLabels[1].Add("TempTex");
			contextLabels[1].Add("TexelSize");
			contextLabels[1].Add("WorldPosition");
			//=====Value=====
			contextLabels[2].Add("Value1");
			contextLabels[2].Add("Value2");
			contextLabels[2].Add("Value3");
			contextLabels[2].Add("Value4");
			//=====HLSL Function=====
			contextLabels[3].Add("Abs");
			contextLabels[3].Add("Acos");
			contextLabels[3].Add("All");
			contextLabels[3].Add("Any");
			contextLabels[3].Add("Asin");
			contextLabels[3].Add("Atan");
			contextLabels[3].Add("Atan2");
			contextLabels[3].Add("Ceil");
			contextLabels[3].Add("Clamp");
			contextLabels[3].Add("Cos");
			contextLabels[3].Add("Cosh");
			contextLabels[3].Add("Cross");
			contextLabels[3].Add("Ddx");
			contextLabels[3].Add("Ddy");
			contextLabels[3].Add("Degrees");
			contextLabels[3].Add("Distance");
			contextLabels[3].Add("Dot");
			contextLabels[3].Add("Exp");
			contextLabels[3].Add("Exp2");
			contextLabels[3].Add("Floor");
			contextLabels[3].Add("Fmod");
			contextLabels[3].Add("Frac");
			contextLabels[3].Add("Fwidth");
			contextLabels[3].Add("Isfinite");
			contextLabels[3].Add("Isinf");
			contextLabels[3].Add("Isnan");
			contextLabels[3].Add("Ldexp");
			contextLabels[3].Add("Length");
			contextLabels[3].Add("Lerp");
			contextLabels[3].Add("Log");
			contextLabels[3].Add("Log2");
			contextLabels[3].Add("Log10");
			contextLabels[3].Add("Max");
			contextLabels[3].Add("Min");
			contextLabels[3].Add("Normalize");
			contextLabels[3].Add("Pow");
			contextLabels[3].Add("Radians");
			contextLabels[3].Add("Reflect");
			contextLabels[3].Add("Refract");
			contextLabels[3].Add("Round");
			contextLabels[3].Add("Rsqrt");
			contextLabels[3].Add("Saturate");
			contextLabels[3].Add("Sign");
			contextLabels[3].Add("Sin");
			contextLabels[3].Add("Sinh");
			contextLabels[3].Add("Smoothstep");
			contextLabels[3].Add("Sqrt");
			contextLabels[3].Add("Step");
			contextLabels[3].Add("Tan");
			contextLabels[3].Add("Tanh");
			contextLabels[3].Add("Trunc");
			//=====Arithmetic=====
			contextLabels[4].Add("Add");
			contextLabels[4].Add("Av");
			contextLabels[4].Add("Sub");
			contextLabels[4].Add("Mul");
			contextLabels[4].Add("Div");
			contextLabels[4].Add("Mod");
			//=====Constant=====
			contextLabels[5].Add("HALF_MAX");
			contextLabels[5].Add("EPSILON");
			contextLabels[5].Add("PI");
			contextLabels[5].Add("TWO_PI");
			contextLabels[5].Add("FOUR_PI");
			contextLabels[5].Add("INV_PI");
			contextLabels[5].Add("INV_TWO_PI");
			contextLabels[5].Add("INV_FOUR_PI");
			contextLabels[5].Add("HALF_PI");
			contextLabels[5].Add("INV_HALF_PI");
			contextLabels[5].Add("FLT_EPSILON");
			contextLabels[5].Add("FLT_MIN");
			contextLabels[5].Add("FLT_MAX");
			//=====Data=====
			contextLabels[6].Add("WorldSpace");
			contextLabels[6].Add("Projection");
			contextLabels[6].Add("Luminance");
			contextLabels[6].Add("DeltaTime");
			contextLabels[6].Add("Ortho");
			contextLabels[6].Add("ZBuffer");
			contextLabels[6].Add("Screen");
			contextLabels[6].Add("Time");
			contextLabels[6].Add("SinTime");
			contextLabels[6].Add("CosTime");
			//=====Logic=====
			contextLabels[7].Add("If");
			contextLabels[7].Add("Compare");
			contextLabels[7].Add("And");
			contextLabels[7].Add("Or");
			contextLabels[7].Add("Not");
			//=====Variable=====
			contextLabels[8].Add("Variable1");
			contextLabels[8].Add("Variable2");
			contextLabels[8].Add("Variable3");
			contextLabels[8].Add("Variable4");
			contextLabels[8].Add("Iterator");
			contextLabels[8].Add("VarLoop1");
			contextLabels[8].Add("VarLoop2");
			contextLabels[8].Add("VarLoop3");
			contextLabels[8].Add("VarLoop4");
			//=====Input=====
			contextLabels[9].Add("Input1");
			contextLabels[9].Add("Input2");
			contextLabels[9].Add("Input3");
			contextLabels[9].Add("Input4");
			//=====Custom=====
			contextLabels[10].Add("Custom");
			//=====PPS Function=====
			contextLabels[11].Add("AnyIsNan");
			contextLabels[11].Add("DecodeStereo");
			contextLabels[11].Add("FastSign");
			contextLabels[11].Add("GradientNoise");
			contextLabels[11].Add("IsNan");
			contextLabels[11].Add("Linear01Depth");
			contextLabels[11].Add("LinearEyeDepth");
			contextLabels[11].Add("Max3");
			contextLabels[11].Add("Min3");
			contextLabels[11].Add("PositivePow");
			contextLabels[11].Add("Rcp");
			contextLabels[11].Add("SafeHDR");
			contextLabels[11].Add("TriangleVertToUV");
			//=====Predefined Macro=====
			contextLabels[12].Add("Checker");
			contextLabels[12].Add("NearClipValue");
			contextLabels[12].Add("StartsAtTop");
			contextLabels[12].Add("Target");
			contextLabels[12].Add("Version");
			//====================
			for (int i = 0; i < 11; ++i){
				specifiedProperties.Add(new List<VisualProperty>());
				popupOptions.Add(new List<string>());
				popupUniques.Add(new List<int>());
			}
			arrows.Add(new List<Texture2D>());
			arrows.Add(new List<Texture2D>());
			arrows[0].Add(Resources.Load("Arrows/LeftRed") as Texture2D);
			arrows[0].Add(Resources.Load("Arrows/LeftGreen") as Texture2D);
			arrows[0].Add(Resources.Load("Arrows/LeftBlue") as Texture2D);
			arrows[0].Add(Resources.Load("Arrows/LeftWhite") as Texture2D);
			arrows[0].Add(Resources.Load("Arrows/LeftCyan") as Texture2D);
			arrows[0].Add(Resources.Load("Arrows/LeftSelected") as Texture2D);
			arrows[1].Add(Resources.Load("Arrows/RightRed") as Texture2D);
			arrows[1].Add(Resources.Load("Arrows/RightGreen") as Texture2D);
			arrows[1].Add(Resources.Load("Arrows/RightBlue") as Texture2D);
			arrows[1].Add(Resources.Load("Arrows/RightWhite") as Texture2D);
			arrows[1].Add(Resources.Load("Arrows/RightCyan") as Texture2D);
			arrows[1].Add(Resources.Load("Arrows/RightSelected") as Texture2D);
			arrows[1].Add(Resources.Load("Arrows/RightDeselected") as Texture2D);
			for (int i = 0; i < 5; ++i){
				flowDots.Add(new List<List<Texture2D>>());
			}
			for (int i = 0; i < 5; ++i){
				flowDots[0].Add(new List<Texture2D>());
				flowDots[1].Add(new List<Texture2D>());
				flowDots[2].Add(new List<Texture2D>());
				flowDots[3].Add(new List<Texture2D>());
				flowDots[4].Add(new List<Texture2D>());
			}
			flowDots[0][0].Add(Resources.Load("Dots/R") as Texture2D);
			flowDots[1][1].Add(Resources.Load("Dots/G") as Texture2D);
			flowDots[2][2].Add(Resources.Load("Dots/B") as Texture2D);
			flowDots[3][3].Add(Resources.Load("Dots/W") as Texture2D);
			flowDots[4][4].Add(Resources.Load("Dots/C") as Texture2D);
			AddDots(0, 1, 'R', 'G');
			AddDots(0, 2, 'R', 'B');
			AddDots(0, 3, 'R', 'W');
			AddDots(0, 4, 'R', 'C');
			AddDots(1, 0, 'G', 'R', true);
			AddDots(1, 2, 'G', 'B');
			AddDots(1, 3, 'G', 'W');
			AddDots(1, 4, 'G', 'C');
			AddDots(2, 0, 'B', 'R', true);
			AddDots(2, 1, 'B', 'G', true);
			AddDots(2, 3, 'B', 'W');
			AddDots(2, 4, 'B', 'C');
			AddDots(3, 0, 'W', 'R', true);
			AddDots(3, 1, 'W', 'G', true);
			AddDots(3, 2, 'W', 'B', true);
			AddDots(3, 4, 'W', 'C');
			AddDots(4, 0, 'C', 'R', true);
			AddDots(4, 1, 'C', 'G', true);
			AddDots(4, 2, 'C', 'B', true);
			AddDots(4, 3, 'C', 'W', true);
		}
		
		internal void AddDots(int a, int b, char left, char right, bool rev = false){
			if (rev) {
				flowDots[a][b].Add(Resources.Load("Dots/" + left) as Texture2D);
				flowDots[a][b].Add(Resources.Load("Dots/" + right + left + "1") as Texture2D);
				flowDots[a][b].Add(Resources.Load("Dots/" + right + left + "0") as Texture2D);
				flowDots[a][b].Add(Resources.Load("Dots/" + right) as Texture2D);
			} else {
				flowDots[a][b].Add(Resources.Load("Dots/" + left) as Texture2D);
				flowDots[a][b].Add(Resources.Load("Dots/" + left + right + "0") as Texture2D);
				flowDots[a][b].Add(Resources.Load("Dots/" + left + right + "1") as Texture2D);
				flowDots[a][b].Add(Resources.Load("Dots/" + right) as Texture2D);
			}
		}
		
		public virtual void OnBeforeSerialize(){
			Serialize();
			allElements[pass] = elements;
			serialization = saver.Save(allElements, settings, properties, renderQueue);
		}
		
		public virtual void OnAfterDeserialize(){
			if (serialization != null && !state.Contains("Drag")){
				renderQueue = saver.RenderRead(serialization);
				DisposePasses();
				settings = saver.SettingsRead(serialization);
				properties = saver.PropertiesRead(serialization);
				allElements = saver.ElementsRead(serialization);
				elements = allElements[pass];
				Serialize();
				CheckConnections();
				TexelTextures();
			}
		}
		
		internal void Enquenque(){
			List<string> varOptions = new List<string>();
			List<int> varSort = new List<int>();
			List<int> varInd = new List<int>();
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].name.Length > 2 && elements[i].name.Substring(0, 3) == "Var"){
					varOptions.Add((varOptions.Count + 1).ToString());
				}
			}
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].name.Length > 2 && elements[i].name.Substring(0, 3) == "Var"){
					elements[i].options = varOptions;
				}
			}
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].name.Length > 2 && elements[i].name.Substring(0, 3) == "Var" && elements[i].Values[1] == "x"){
					elements[i].Values[0] = (varOptions.Count - 1).ToString();
					elements[i].Values[1] = elements[i].Values[0];
				}
			}
			for (int i = 0; i < elements.Count; ++i){
				for (int j = 0; j < elements.Count; ++j){
					if (elements[i].name.Length > 2 && elements[i].name.Substring(0, 3) == "Var" && elements[j].name.Length > 2 && elements[j].name.Substring(0, 3) == "Var" && elements[i].Values[0] == elements[j].Values[0] && i != j){
						if (elements[i].Values[0] == elements[i].Values[1]){
							elements[i].Values[0] = elements[j].Values[1];
							elements[i].Values[1] = elements[i].Values[0];
							elements[j].Values[1] = elements[j].Values[0];
						}
					}
				}
			}
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].name.Length > 2 && elements[i].name.Substring(0, 3) == "Var"){
					varInd.Add(i);
					varSort.Add(Int32.Parse(elements[i].Values[0]));
				}
			}
			var sortIt = varSort.Select((x, i) => new KeyValuePair<int, int>(x, i)).OrderBy(x => x.Key).ToList();
			List<int> idx = sortIt.Select(x => x.Value).ToList();
			for (int i = 0; i < varInd.Count; ++i){
				elements[varInd[idx[i]]].Values[0] = i.ToString();
			}
		}
		
		internal virtual void DragUpdated(){
			if (DragAndDrop.paths[0].Contains(".shader")){
				DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
			}
		}
		
		internal virtual void DragExited(){
			if (DragAndDrop.paths[0].Contains(".shader")){
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
					OrganizePasses();
					Serialize();
					CheckConnections();
					DisposeProperties();
				}
			}
		}
		
		internal void Shortcut(){
			if (pressed.Count == 1){
				switch (pressed[0]){
					//=====Arithmetic=====
					case KeyCode.A:
						AddElement("Add");
					break;
					case KeyCode.S:
						AddElement("Sub");
					break;
					case KeyCode.M:
						AddElement("Mul");
					break;
					case KeyCode.D:
						AddElement("Div");
					break;
					//=====HLSL Function=====
					case KeyCode.E:
						AddElement("Exp");
					break;
					case KeyCode.F:
						AddElement("Floor");
					break;
					case KeyCode.L:
						AddElement("Lerp");
					break;
					case KeyCode.N:
						AddElement("Normalize");
					break;
					case KeyCode.P:
						AddElement("Pow");
					break;
					case KeyCode.R:
						AddElement("Round");
					break;
					case KeyCode.T:
						AddElement("Tan");
					break;
					//=====Logic=====
					case KeyCode.I:
						AddElement("If");
					break;
					//=====Value=====
					case KeyCode.Alpha1:
						AddElement("Value1");
					break;
					case KeyCode.Alpha2:
						AddElement("Value2");
					break;
					case KeyCode.Alpha3:
						AddElement("Value3");
					break;
					case KeyCode.Alpha4:
						AddElement("Value4");
					break;
					//=====Custom=====
					case KeyCode.C:
						AddElement("Custom");
					break;
				}
			} else if (pressed.Count == 2){
				KeyCode tempKey = KeyCode.None;
				if (pressed[0] == KeyCode.LeftAlt){
					tempKey = pressed[1];
				} else if (pressed[1] == KeyCode.LeftAlt){
					tempKey = pressed[0];
				}
				if (tempKey != KeyCode.None){
					switch (tempKey){
						//=====Arithmetic=====
						case KeyCode.M:
							AddElement("Mod");
						break;
						//=====HLSL Function=====
						case KeyCode.C:
							AddElement("Clamp");
						break;
						case KeyCode.D:
							AddElement("Dot");
						break;
						case KeyCode.E:
							AddElement("Exp2");
						break;
						case KeyCode.F:
							AddElement("Fwidth");
						break;
						case KeyCode.L:
							AddElement("Length");
						break;
						case KeyCode.R:
							AddElement("Reflect");
						break;
						case KeyCode.S:
							AddElement("Sin");
						break;
						case KeyCode.T:
							AddElement("Trunc");
						break;
						//=====Variable=====
						case KeyCode.Alpha1:
							AddElement("Variable1");
						break;
						case KeyCode.Alpha2:
							AddElement("Variable2");
						break;
						case KeyCode.Alpha3:
							AddElement("Variable3");
						break;
						case KeyCode.Alpha4:
							AddElement("Variable4");
						break;
					}
				}
			}
		}
		
		internal void Refresh(){
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].name.Contains("Custom")){
					elements[i].Refresh();
					elements[i].changed = true;
					elements[i].Show();
					elements[i].ShowLabels();
				}
			}
			CheckConnections();
			Write();
			AssetDatabase.ImportAsset("Assets/Post Processing Ultimate/Shaders", ImportAssetOptions.ImportRecursive);
		}
		
		internal void DrawSettings(){
			if (stylesUI.Count > 5 && GUI.Button(new Rect(position.width - 100, position.height - 20, 96, 16), new GUIContent("SETTINGS"), stylesUI[5])){
				SettingsWindow window = (SettingsWindow) GetWindow(typeof(SettingsWindow));
				window.minSize = new Vector2(300, 70);
				window.position = new Rect(UnityEngine.Screen.currentResolution.width / 2 - 150, UnityEngine.Screen.currentResolution.height / 2 - 25, 300, 50);
				window.wantsMouseMove = true;
				window.wantsMouseEnterLeaveWindow = true;
				window.Show();
				window.titleContent = new GUIContent("Settings");
			}
		}
		
		public void LoadSettings(){
			try {
				uSets = JsonUtility.FromJson<UserSettings>(System.IO.File.ReadAllText("Assets/Post Processing Ultimate/Editor/config.usets"));
			} catch {
				uSets = new UserSettings();
			}
		}
		
		public void DrawReview(){
			if (uSets.Starts == 31){
				ReviewWindow window = (ReviewWindow) GetWindow(typeof(ReviewWindow));
				window.minSize = new Vector2(400, 56);
				window.position = new Rect(UnityEngine.Screen.currentResolution.width / 2 - 150, UnityEngine.Screen.currentResolution.height / 2 - 25, 300, 50);
				window.wantsMouseMove = true;
				window.wantsMouseEnterLeaveWindow = true;
				window.Show();
				window.titleContent = new GUIContent("Review");
				++uSets.Starts;
			}
		}
		
		internal void OnGUI(){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			state = Event.current.type.ToString();
			if (Event.current.type == EventType.MouseEnterWindow || Event.current.type == EventType.MouseLeaveWindow){
				CheckConnections();
			}
			if (!loaded && contextLabels.Count == 0){
				if (Event.current.type != EventType.Layout){
					if (backgrounds.Count != 0){
						for (int i = 0; i < position.width / 128; ++i){
							for (int j = 0; j < position.height / 128; ++j){
								GUI.DrawTexture(new Rect(i * 128, j * 128, 128, 128), backgrounds[0]);
							}
						}
						GUI.DrawTexture(new Rect(0, 0, position.width, position.height), backgrounds[1]);
						if (!loadedS){
							GUI.DrawTexture(new Rect(position.width * 0.25f, position.height * 0.395f - position.width * 0.05f, position.width * 0.5f, position.width * 0.21f), backgrounds[2]);
						}
					}
					loaded = true;
					loadedS = true;
					Repaint();
				}
			} else {
				if (Event.current.type != EventType.Layout){
					if (selecting && selectingBox.x == 0 && selectingBox.y == 0){
						selecting = false;
						for (int i = 0; i < elements.Count; ++i){
							if (elements[i] != focusedElement){
								elements[i].selected = false;
							}
						}
					}
					Undo.RecordObject(this, "Changed Settings");
					if (contextLabels.Count == 0){
						Import();
					}
					if (serialization != null){
						serialization = null;
						DisposeProperties();
					}
					isDockedMethod = typeof(EditorWindow).GetProperty("docked", fullBinding).GetGetMethod(true);
						docked = (bool) isDockedMethod.Invoke(this, null);
					if (!docked){
						offset = 23;
					} else {
						offset = 21;
					}
					defaultGUI = GUI.matrix;
					DrawBackground();
					GUI.EndGroup();
					SelectionHadle();
					BeforeScroll();
					GUI.BeginGroup(new Rect(0, offset * scale, position.width / scale, position.height / scale));
					GUIUtility.ScaleAroundPivot(Vector2.one * scale, new Vector2(0, offset));
					MouseHandle();
					AfterScroll();
					DrawBeziers();
					DrawElements();
					DrawSelection();
					GUI.matrix = defaultGUI;
					GUI.EndGroup();
					GUI.BeginGroup(new Rect(0, offset, position.width, position.height));
					DrawLabels();
					DrawReview();
					GL.Clear(true, false, Color.white, 1);
				}
				Enquenque();
				DrawContext(contextPosition * scale);
				DrawSettings();
				if (toolbar.Count > 1){
					DrawToolbar();
				}
			}
		}
	}
}