using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace NTEC.PPU {
	public class LUT_Blender : EditorWindow {
		
		internal List<Camera> cameras = new List<Camera>();
		internal List<int> scripts = new List<int>();
		internal List<string> camerasNames = new List<string>();
		internal int selection = 0;
		internal int tempSelection = 0;
		internal Vector2 scrollPos;
		internal static LUT_Blender window;
		internal float width;
		internal float height;
		internal float lerp = 0.5f;
		internal float lerp1 = 0.5f;
		internal Texture2D[] tex = new Texture2D[2];
		internal Texture2D[] tex1 = new Texture2D[2];
		internal Camera cam;
		internal UnityEngine.Rendering.PostProcessing.PostProcessVolume ufs;
		internal UnityEngine.Rendering.PostProcessing.PostProcessProfile ufs2;
		internal List<UnityEngine.Rendering.PostProcessing.PostProcessEffectSettings> ufs3;
		internal UnityEngine.Rendering.PostProcessing.ColorGrading ufs4;
		internal Texture2D tex2D;
		internal Texture2D image;
		internal Texture2D ldr;
		internal Texture2D current;
		internal RenderTexture currentRT;
		internal RenderTexture currentRTII;
		internal RenderTexture currentRTIII;
		internal string path;
		internal bool stopped;
	
		[MenuItem("Window/Post Processing Ultimate/LUT Blender")]
		private static void Init(){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			window = (LUT_Blender) GetWindow(typeof(LUT_Blender));
			window.minSize = new Vector2(320, 160);
			window.titleContent = new GUIContent("LUT Blender");
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
			if (scripts.Count == 0){
				return;
			}
			ufs4 = (UnityEngine.Rendering.PostProcessing.ColorGrading) ufs3[scripts[selection]];
			if (ufs4.ldrLut.value != null){
				ldr = (Texture2D) ufs4.ldrLut.value;
			}
			ufs4.ldrLut.value = null;
			tex2D = CurrentScreen(cam);
			ufs4.ldrLut.value = ldr;
			tex[0] = Resources.Load("NeutralLdrLut") as Texture2D;
			tex[1] = tex[0];
			tex1[0] = tex[0];
			tex1[1] = tex[0];
			Clear();
		}
		
		internal void Blend(){
			current = new Texture2D(1024, 32, TextureFormat.RGBAFloat, true);
			for (int y = 0; y < current.height; ++y){
				for (int x = 0; x < current.width; ++x){
					try {
						Color color = Color.Lerp(tex[0].GetPixel(x, y), tex[1].GetPixel(x, y), lerp);
						current.SetPixel(x, y, color);
					} catch {
						Debug.Log("Wrong textures");
					}
				}
			}
			current.Apply();
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
		
		internal void Clear(){
			EditorUtility.UnloadUnusedAssetsImmediate(true);
			Resources.UnloadUnusedAssets();
			System.GC.Collect();
		}
		
		internal void OnDestroy(){
			Clear();
		}
		
		internal void OnGUI(){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			width = position.width - 22;
			height = 0.5625f * width;
			if (!Application.isPlaying && cam != null && cam.enabled && !stopped) {
				scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
				selection = EditorGUILayout.Popup("Camera:", selection, camerasNames.ToArray());
				if (selection != tempSelection){
					tempSelection = selection;
					Awake();
				}
				GUILayout.Box(tex2D, GUILayout.Width(width), GUILayout.Height(Mathf.Min(height, tex2D.height)));
				tex[0] = (Texture2D) EditorGUILayout.ObjectField(tex[0], typeof(Texture2D), false);
				tex[1] = (Texture2D) EditorGUILayout.ObjectField(tex[1], typeof(Texture2D), false);
				lerp = EditorGUILayout.Slider(lerp, 0f, 1f);
				if (GUILayout.Button("Save")){
					path = EditorUtility.SaveFilePanel("Save LUT Texture", "Assets/Post Processing Ultimate/Resources/PPU_LUTs", "MyLUT", "png");
					Blend();
					if (path != null && path != ""){
						File.WriteAllBytes(path, current.EncodeToPNG());
						Debug.Log("Success");
					}
				}
				if (lerp1 != lerp || tex1[0] != tex[0] || tex1[1] != tex[1]){
					lerp1 = lerp;
					tex1[0] = tex[0];
					tex1[1] = tex[1];
					Blend();
					ufs4.ldrLut.value = current;
					tex2D = CurrentScreen(cam);
					ufs4.ldrLut.value = null;
				}
				EditorGUILayout.EndScrollView();
			} else {
				stopped = true;
				if (GUILayout.Button((Application.isPlaying) ? "Click me when Application is not playing." : "Click me when Color Grading is attached to an active camera.", GUILayout.Width(position.width - 4), GUILayout.Height(position.height - 4))){
					Awake();
				}
			}
		}
	}
}