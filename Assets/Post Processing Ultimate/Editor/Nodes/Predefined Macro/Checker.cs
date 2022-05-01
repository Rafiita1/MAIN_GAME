using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NTEC.PPU{
	public class Checker : VisualElement{
		
		private Color background = new Color(0.6f, 0.6f, 1, 0.25f);
		private Texture2D nodeBack;
		private string label;
		private string tooltip;
 		
		public Checker(){
			position = new Rect(32, 32, 192, 135);
			name = "Checker";
			joints.Add(new Joint(1, 4));
			options.Add("SHADER_API_D3D9");
			options.Add("SHADER_API_D3D11");
			options.Add("SHADER_API_GLCORE");
			options.Add("SHADER_API_GLES");
			options.Add("SHADER_API_GLES3");
			options.Add("SHADER_API_METAL");
			options.Add("SHADER_API_VULKAN");
			options.Add("SHADER_API_D3D11_9X");
			options.Add("SHADER_API_PS4");
			options.Add("SHADER_API_PSSL");
			options.Add("SHADER_API_XBOXONE");
			options.Add("SHADER_API_PSP2");
			options.Add("SHADER_API_WIIU");
			options.Add("SHADER_API_MOBILE");
			options.Add("SHADER_TARGET_GLSL");
			options.Add("UNITY_NO_SCREENSPACE_SHADOWS");
			options.Add("UNITY_NO_LINEAR_COLORSPACE");
			options.Add("UNITY_NO_RGBM");
			options.Add("UNITY_NO_DXT5nm");
			options.Add("UNITY_FRAMEBUFFER_FETCH_AVAILABLE");
			options.Add("UNITY_USE_RGBA_FOR_POINT_SHADOWS");
			options.Add("UNITY_ATTEN_CHANNEL");
			options.Add("UNITY_HALF_TEXEL_OFFSET");
			options.Add("UNITY_MIGHT_NOT_HAVE_DEPTH_Texture");
			options.Add("UNITY_CAN_COMPILE_TESSELLATION");
			options.Add("UNITY_REVERSED_Z");
			Values.Add("0");
			Values.Add("0");
		}
		
		public override void Show(){
			base.Show();
			if (nodeBack == null){
				nodeBack = CombineTextures(Resources.Load("Nodes/Top") as Texture2D, position.height, background);
			}
			GUI.DrawTexture(position, nodeBack);
			for (int i = 0; i < joints.Count; ++i){
				if (joints[i].type == 0){
					joints[i].coords = new Vector2(position.x - 48, 8 + position.y + (i + 2) * (40));
					joints[i].Show();
				} else {
					joints[i].coords = new Vector2(position.x + position.width + 24, 8 + position.y + (i + 2) * (40));
					joints[i].Show();
				}
			}
		}
		
		public override void ShowLabels(){
			base.ShowLabels();
			EditorGUI.LabelField(new Rect(position.x * scale, position.y * scale, position.width * scale, 40 * scale), new GUIContent(name, Tooltips.List(name)), style[0]);
			Values[0] = EditorGUI.Popup(new Rect(position.x * scale, (position.y + 1 * (40) + 4) * scale, position.width * scale, 32 * scale), Int32.Parse(Values[0]), options.ToArray(), style[4]).ToString();                                                                              
			EditorGUI.LabelField(new Rect((position.x + 4) * scale, (position.y + 2 * (40) + 8) * scale, (position.width - 8) * scale, 32 * scale), new GUIContent("Output", "True or false"), style[1]);
		}
	}
}