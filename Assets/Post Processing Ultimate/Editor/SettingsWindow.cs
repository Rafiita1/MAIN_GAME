using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace NTEC.PPU {
	public class SettingsWindow : EditorWindow {
		
		internal UserSettings uSets;
		internal int curHeight;
		internal List<GUIStyle> stylesUI = new List<GUIStyle>();
		
		internal void Awake(){
			try {
				uSets = JsonUtility.FromJson<UserSettings>(System.IO.File.ReadAllText("Assets/Post Processing Ultimate/Editor/config.usets"));
			} catch {
				uSets = new UserSettings();
			}
			stylesUI.Add(new GUIStyle(GUI.skin.button));
			stylesUI[0].focused.textColor = new Color(0.7f, 0.7f, 0.7f);
			stylesUI[0].normal.textColor = new Color(0.7f, 0.7f, 0.7f);
			stylesUI[0].active.textColor = new Color(0.7f, 0.7f, 0.7f);
			stylesUI[0].hover.textColor = new Color(0.7f, 0.7f, 0.7f);
			stylesUI[0].alignment = TextAnchor.MiddleCenter;
			stylesUI[0].focused.background = Resources.Load("Buttons/GeneralOff") as Texture2D;
			stylesUI[0].normal.background = Resources.Load("Buttons/GeneralOff") as Texture2D;
			stylesUI[0].active.background = Resources.Load("Buttons/GeneralExe") as Texture2D;
			stylesUI[0].hover.background = Resources.Load("Buttons/GeneralOn") as Texture2D;
			stylesUI.Add(new GUIStyle(EditorStyles.toggle));
			stylesUI[1].focused.background = Resources.Load("Icons/ToggleOff") as Texture2D;
			stylesUI[1].normal.background = Resources.Load("Icons/ToggleOff") as Texture2D;
			stylesUI[1].active.background = Resources.Load("Icons/ToggleOff") as Texture2D;
			stylesUI[1].onFocused.background = Resources.Load("Icons/ToggleOn") as Texture2D;
			stylesUI[1].onNormal.background = Resources.Load("Icons/ToggleOn") as Texture2D;
			stylesUI[1].onActive.background = Resources.Load("Icons/ToggleOff") as Texture2D;
		}
		
		internal void OnGUI(){
			curHeight = 4;
			uSets.Flow = EditorGUI.Toggle(new Rect(position.width - 20, curHeight, position.width, 16), uSets.Flow, stylesUI[1]);
			EditorGUI.LabelField(new Rect(4, curHeight, position.width, 16), "Flowing dots enabled by default");
			uSets.SwitchMMB = EditorGUI.Toggle(new Rect(position.width - 20, curHeight += 16, position.width, 16), uSets.SwitchMMB, stylesUI[1]);
			EditorGUI.LabelField(new Rect(4, curHeight, position.width, 16), "Switch MMB with LMB during group selection");
			uSets.AutoSave = EditorGUI.Toggle(new Rect(position.width - 20, curHeight += 16, position.width, 16), uSets.AutoSave, stylesUI[1]);
			EditorGUI.LabelField(new Rect(4, curHeight, position.width, 16), "Autosave when mouse button up");
			if (GUI.Button(new Rect(4, curHeight += 16, position.width - 8, 16), new GUIContent("SAVE"), stylesUI[0])){
				System.IO.File.WriteAllText("Assets/Post Processing Ultimate/Editor/config.usets", JsonUtility.ToJson(uSets));
				this.Close();
				PPU.ShaderEditor window1 = (PPU.ShaderEditor) GetWindow(typeof(PPU.ShaderEditor));
				PPU.FunctionEditor window2 = (PPU.FunctionEditor) GetWindow(typeof(PPU.FunctionEditor));
				if (window1.contextLabels.Count > 1){
					window1.LoadSettings();
				} else {
					window1.Close();
				}
				if (window2.contextLabels.Count > 1){
					window2.LoadSettings();
				} else {
					window2.Close();
				}
			}
			Repaint();
		}
	}
}