using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace NTEC.PPU {
	public class LUT_Picker : EditorWindow {
		
		internal List<Camera> cameras = new List<Camera>();
		internal List<int> scripts = new List<int>();
		internal List<string> camerasNames = new List<string>();
		internal int selection = 0;
		internal int tempSelection = 0;
		internal Vector2 scrollPos;
		internal static LUT_Picker window;
		internal float width;
		internal float height;
		internal Object[] tex;
		internal Camera cam;
		internal UnityEngine.Rendering.PostProcessing.PostProcessVolume ufs;
		internal UnityEngine.Rendering.PostProcessing.PostProcessProfile ufs2;
		internal List<UnityEngine.Rendering.PostProcessing.PostProcessEffectSettings> ufs3;
		internal UnityEngine.Rendering.PostProcessing.ColorGrading ufs4;
		internal List<Texture2D> tex2D = new List<Texture2D>();
		internal Texture2D image;
		internal Texture2D ldr;
		internal RenderTexture currentRT;
		internal RenderTexture currentRTII;
		internal RenderTexture currentRTIII;
		internal bool stopped;
	
		[MenuItem("Window/Post Processing Ultimate/LUT Picker")]
		private static void Init(){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			window = (LUT_Picker) GetWindow(typeof(LUT_Picker));
			window.minSize = new Vector2(320, 160);
			window.titleContent = new GUIContent("LUT Picker");
			window.Show();
		}
		
		internal void Awake(){
			stopped = false;
			List<Camera> tempCameras = (UnityEngine.Object.FindObjectsOfType<Camera>()).ToList();
			cameras.Clear();
			for (int i = 0; i < tempCameras.Count; ++i){
				if (tempCameras[i] != null && tempCameras[i].enabled){
					ufs = tempCameras[i].GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessVolume>();
					if (ufs == null) {
						return;
					}
					ufs2 = ufs.sharedProfile;
					ufs3 = ufs2.settings;
					for (int j = 0; j < ufs3.Count; ++j){
						if (ufs3[j].ToString().Contains("ColorGrading")){
							scripts.Add(j);
							cameras.Add(tempCameras[i]);
							break;
						}
					}
				}
			}
			camerasNames.Clear();
			for (int i = 0; i < cameras.Count; ++i){
				camerasNames.Add(cameras[i].name);
			}
			if (cameras.Count > 0){
				cam = cameras[selection];
			}
			tex = Resources.LoadAll("PPU_LUTs");
			if (scripts.Count == 0){
				return;
			}
			ufs4 = (UnityEngine.Rendering.PostProcessing.ColorGrading) ufs3[scripts[selection]];
			if (ufs4.ldrLut.value != null){
				ldr = (Texture2D) ufs4.ldrLut.value;
			}
			tex2D.Clear();
			for (int i = 0; i < tex.Length; ++i){
				ufs4.ldrLut.value = (Texture2D) tex[i];
				tex2D.Add(CurrentScreen(cam));
			}
			ufs4.ldrLut.value = ldr;
			Clear(false);
		}
		
		internal Texture2D CurrentScreen(Camera cam){
			currentRT = RenderTexture.GetTemporary(854, 480);
			currentRTII = RenderTexture.active;
			currentRTIII = cam.targetTexture;
			cam.targetTexture = currentRT;
			RenderTexture.active = cam.targetTexture;
			cam.Render();
			image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.RGB24, true, true);
			image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
			image.Apply();
			RenderTexture.active = currentRTII;
			cam.targetTexture = currentRTIII;
			RenderTexture.ReleaseTemporary(currentRT);
			return image;
		}
		
		
		internal void Click(int i){
			EditorUtility.SetDirty(cam);
			EditorUtility.SetDirty(ufs2);
			ufs4.ldrLut.value = (Texture2D) tex[i];
			AssetDatabase.SaveAssets();
		}
		
		internal void Clear(bool full){
			if (full){
				for (int i = 0; i < tex2D.Count; ++i){
					DestroyImmediate(tex2D[i]);
				}
			}
			EditorUtility.UnloadUnusedAssetsImmediate(true);
			Resources.UnloadUnusedAssets();
			System.GC.Collect();
		}
		
		internal void OnDestroy(){
			Clear(true);
		}
		
		internal void OnGUI(){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			width = (position.width / 2) - 16f;
			height = 0.5625f * ((position.width / 2) - 4);
			if (cam != null && cam.enabled && tex.Length == tex2D.Count && !stopped) {
				if (tex2D[0] == null){
					Awake();
				}
				scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
				selection = EditorGUILayout.Popup("Camera:", selection, camerasNames.ToArray());
				if (selection != tempSelection){
					tempSelection = selection;
					Awake();
				}
				int y = 0;
				for (int i = 0; i < (tex.Length + 1) / 2; ++i) {
					EditorGUILayout.BeginHorizontal();
					for (int j = 0; j < 2; ++j) {
						if (y < tex2D.Count){
							EditorGUILayout.BeginVertical();
							if (GUILayout.Button(tex2D[y] as Texture2D, GUILayout.Width(width), GUILayout.Height(height))){
								Click(y);
							}
							GUILayout.Label(tex[y].name);
							++y;
							EditorGUILayout.EndVertical();
						}
					}
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.EndScrollView();
			} else {
				stopped = true;
				if (GUILayout.Button("Click me when Color Grading is attached to an active camera.", GUILayout.Width(position.width - 4), GUILayout.Height(position.height - 4))){
					Awake();
				}
			}
		}
	}
}