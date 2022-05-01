using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;

namespace NTEC.PPU {
	public class Screenshot : EditorWindow {
		
		private static int width; 
		private static int height;
		private static Camera camera;
		private static RenderTexture rt;
		private static Texture2D screenShot;
		private static byte[] bytes;
		private static string filename = "screen";
		private static Vector2 dim = new Vector2(1920, 1080);
		private static int index;
		private static Camera[] cameras;
		private static List<string> camNames = new List<string>();
		private static string path;
		private static int files = 1;
		
		[MenuItem("Window/Post Processing Ultimate/Screenshot")]
		static void Init(){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			Screenshot window = (Screenshot) EditorWindow.GetWindow(typeof(Screenshot));
			window.Show();
			path = Application.dataPath + "/Post Processing Ultimate/Screenshots/";
			if (!Directory.Exists(path)){
				path = Application.dataPath + "/Post Processing Ultimate/";
			}
			window.minSize = new Vector2(300, 76);
			window.titleContent = new GUIContent("Screenshot");
			window.position = new Rect(UnityEngine.Screen.currentResolution.width * 0.375f, UnityEngine.Screen.currentResolution.height * 0.375f, UnityEngine.Screen.currentResolution.width * 0.25f, 79);
		}
		
		void OnGUI(){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			cameras = FindObjectsOfType<Camera>();
			foreach (Camera i in cameras){
				camNames.Add(i.ToString());
			}
			index = EditorGUILayout.Popup(index, camNames.ToArray());
			camera = cameras[index];
			dim = EditorGUILayout.Vector2Field("Screenshot dimensions", dim);
			dim.x = (int) dim.x;
			dim.y = (int) dim.y;
			width = (int) dim.x;
			height = (int) dim.y;
			if (GUILayout.Button("Take screenshot")){
				if (path != null && path != ""){
					if (path.Contains(".")){
						int last = 0;
						for (int i = path.Length - 1; i >= 0; --i){
							if (path[i] == '/'){
								last = i + 1;
								break;
							}
						}
						path = path.Substring(0, last);
					}
					files = (new System.IO.DirectoryInfo(path)).GetFiles().Length / 2 + 1;
				}
				path = EditorUtility.SaveFilePanel("Save screenshot", path, filename + files, "png");
				Shot();
				AssetDatabase.Refresh();
			}
		}
		
		static void Shot(){
			if (camera != null){
				rt = RenderTexture.GetTemporary(width, height, 16);
				camera.targetTexture = rt;
				screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
				camera.Render();
				RenderTexture.active = rt;
				screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
				camera.targetTexture = null;
				RenderTexture.active = null;
				RenderTexture.ReleaseTemporary(rt);
				bytes = screenShot.EncodeToPNG();
				try {
					#if !UNITY_WEBPLAYER
					System.IO.File.WriteAllBytes(path, bytes);
					#else
					Debug.Log("Doesn't work in Webplayer");
					#endif
				} catch {}
			}
		}
	}
}