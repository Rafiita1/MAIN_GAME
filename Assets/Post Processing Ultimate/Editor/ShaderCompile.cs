using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace NTEC.PPU{
	public class ShaderCompile {
		
		internal bool world;
		internal bool spline;
		internal bool loops;
		internal string name;
		internal List<string> evaluations;
		internal string precision;
		internal ShaderSave saver = new ShaderSave();
	
		internal virtual void Compile(List<List<VisualElement>> allElements, Settings settings, List<VisualProperty> properties, string path, RenderQueue renderQueue){
			if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "en-US"){
				System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
			}
			precision = settings.Values[3] == "0" ? "half" : "float";
			string secondPath = (path.Substring(0, path.Length - 7) + ".cs");
			name = secondPath.Substring(0, secondPath.Length - 3);
			for (int i = name.Length - 1; i >= 0; --i){
				if (name[i] == '/'){
					name = name.Substring(i + 1);
					break;
				}
			}
			secondPath = secondPath.Substring(0, secondPath.IndexOf(name));
			name = name.Replace(" ", "");
			secondPath += name + ".cs";
			List<VisualProperty> props = FilterProps(properties);
			props.Reverse();
			spline = false;
			for (int i = 0; i < renderQueue.passes.Count; ++i){
				if (renderQueue.passes[i].Iterations != 1 || renderQueue.passes[i].Variable != 0){
					loops = true;
					break;
				} else {
					loops = false;
				}
			}
			evaluations = new List<string>();
			List<string> tempRenders = TempRT(renderQueue.tempTextures);
			List<string> parameters = Parameters(props);
			List<string> inputs = Inputs(props);
			List<string> fiel = Fiel(props, allElements);
			List<string> ques = Queue(renderQueue, allElements);
			List<string> rels = Rel(renderQueue);
			List<List<string>> frag = new List<List<string>>();
			List<List<string>> customs = new List<List<string>>();
			for (int i = 0; i < allElements.Count; ++i){
				frag.Add(Frag(allElements[i]));
			}
			for (int i = 0; i < allElements.Count; ++i){
				customs.Add(Customs(allElements[i]));
			}
			for (int i = 0; i < allElements.Count; ++i){
				if (world = World(allElements[i])){
					break;
				}
			}
			string output2 = "using System;\nusing UnityEngine;\nusing UnityEngine.Rendering.PostProcessing;\n\nnamespace NTEC.PPU\n{\n\t[Serializable]";
			output2 += "\n\t[PostProcess(typeof(" + name + "Renderer), PostProcessEvent." + (settings.Values[0] == "0" ? "AfterStack" : settings.Values[0] == "1" ? "BeforeStack" : "BeforeTransparent") + ", \"" + settings.Values[1] + "\")]";
			output2 += "\n\tpublic sealed class " + name + " : PostProcessEffectSettings\n\t{";
			for (int i = 0; i < parameters.Count; ++i){
				output2 += "\n\t\t" + parameters[i];
			}
			for (int i = 0; i < tempRenders.Count; ++i){
				output2 += "\n\t\t" + tempRenders[i];
			}
			if (spline){
				output2 += "\n\n\t\tpublic void CreateEvaluations(){";
				output2 += "\n\t\t\tColor pixel = Color.black;";
				for (int i = 0; i < evaluations.Count; ++i){
					output2 += "\n\t\t\t" + evaluations[i] + " = new Texture2D(128, 1) {wrapMode = TextureWrapMode.Clamp}; ";
					output2 += "\n\t\t\tfor (int i = 0; i < 128; ++i) {";
					output2 += "\n\t\t\t\tpixel.a = " + evaluations[i].Substring(0, evaluations[i].Length - 10) + ".value.cachedData[i];";
					output2 += "\n\t\t\t\t" + evaluations[i] + ".SetPixel(i, 0, pixel);";
					output2 += "\n\t\t\t}";
					output2 += "\n\t\t\t" + evaluations[i] + ".Apply();";
				}
				output2 += "\n\t\t}";
			}
			output2 += "\n\t}\n\n\tpublic sealed class " + name + "Renderer : PostProcessEffectRenderer<" + name + ">\n\t{\n\t\tpublic override void Render(PostProcessRenderContext context)\n\t\t{";
			output2 += "\n\t\t\tvar sheet = context.propertySheets.Get(Shader.Find(\"" + settings.Values[1] + "\"));";
			for (int i = 0; i < rels.Count; ++i){
				output2 += "\n\t\t\t" + rels[i];
			}
			if (spline){
				output2 += "\n\t\t\tsettings.CreateEvaluations();";
			}
			if (world){
				output2 += "\n\t\t\tMatrix4x4 projMat = GL.GetGPUProjectionMatrix(context.camera.projectionMatrix, false);";
				output2 += "\n\t\t\tprojMat[15] = projMat[14] = projMat[11] = 0;";
				output2 += "\n\t\t\t++projMat[15];";
				output2 += "\n\t\t\tsheet.properties.SetMatrix(\"_ProjMat\", Matrix4x4.Inverse(projMat * context.camera.worldToCameraMatrix) * Matrix4x4.TRS(new Vector3(0, 0, -1 * projMat[10]), Quaternion.identity, Vector3.one));";
			}
			for (int i = 0; i < inputs.Count; ++i){
				output2 += "\n\t\t\t" + inputs[i];
			}
			for (int i = 0; i < ques.Count; ++i){
				output2 += "\n\t\t\t" + ques[i];
			}
			output2 += "\n\t\t}\n\t}\n}";
			string output = saver.Save(allElements, settings, properties, renderQueue) + "\n\n";
			output += "Shader \"" + settings.Values[1] + "\" {";
			output += "\n\n\tSubShader {";
			output += "\n\t\tCull Off ZWrite Off ZTest Always";
			output += "\n";
			for (int i = 0; i < allElements.Count; ++i){
				List<string> texFiel = TexFiel(allElements[i]);
				List<string> tempFiel = TempFiel(allElements[i]);
				output += "\n\t\tPass {";
				output += "\n\t\t\tHLSLPROGRAM";
				if (!world){
					output += "\n\t\t\t#pragma vertex VertDefault";
				} else {
					output += "\n\t\t\t#pragma vertex Vert";
				}
				output += "\n\t\t\t#pragma fragment Frag";
				output += "\n";
				if (Application.unityVersion.Contains("2017")){
					output += "\n\t\t\t#include \"../../PostProcessing/Shaders/StdLib.hlsl\"";
				} else if (Application.unityVersion.Contains("2018.1") || Application.unityVersion.Contains("2018.2")){
					output += "\n\t\t\t#include \"PostProcessing/Shaders/StdLib.hlsl\"";
				} else {
					output += "\n\t\t\t#include \"Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl\"";
				}
				for (int j = 0; j < customs[i].Count; ++j){
					output += "\n\t\t\t" + customs[i][j];
				}
				if (world){
					output += "\n\t\t\tuniform float4x4 _ProjMat;";
	                output += "\n\t\t\t";
					output += "\n\t\t\tstruct VaryingsExtended";
					output += "\n\t\t\t{";
					output += "\n\t\t\t\tfloat4 vertex : SV_POSITION;";
					output += "\n\t\t\t\tfloat2 texcoord : TEXCOORD0;";
					output += "\n\t\t\t\tfloat2 texcoordStereo : TEXCOORD1;";
					output += "\n\t\t\t\tfloat3 worldDirection : TEXCOORD2;";
					output += "\n\t\t\t};";
					output += "\n\t\t\t";
					output += "\n\t\t\tVaryingsExtended Vert(AttributesDefault v)";
					output += "\n\t\t\t{";
					output += "\n\t\t\t\tVaryingsExtended o;";
					output += "\n\t\t\t\to.vertex = float4(v.vertex.xy, 0.0, 1.0);";
					output += "\n\t\t\t\to.texcoord = TransformTriangleVertexToUV(v.vertex.xy);";
					output += "\n\t\t\t\t#if UNITY_UV_STARTS_AT_TOP";
					output += "\n\t\t\t\t\to.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);";
					output += "\n\t\t\t\t#endif";
					output += "\n\t\t\t\to.texcoordStereo = TransformStereoScreenSpaceTex(o.texcoord, 1.0);";
					output += "\n\t\t\t\tfloat4 clip = float4(o.texcoord.xy * 2 - 1, 0.0, 1.0);";
					output += "\n\t\t\t\to.worldDirection = mul(_ProjMat, clip) - _WorldSpaceCameraPos;";
					output += "\n\t\t\t\treturn o;";
					output += "\n\t\t\t}";
				}
				output += "\n";
				output += "\n\t\t\tTEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);";
				if (Depth(allElements[i])){
					output += "\n\t\t\tTEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);";
				}
				for (int j = 0; j < fiel.Count; ++j){
					output += "\n\t\t\t" + fiel[j];
				}
				for (int j = 0; j < texFiel.Count; ++j){
					output += "\n\t\t\t" + texFiel[j];
				}
				for (int j = 0; j < tempFiel.Count; ++j){
					output += "\n\t\t\t" + tempFiel[j];
				}
				if (!world){
					output += "\n\n\t\t\t" + precision + "4 Frag (VaryingsDefault i) : SV_Target {";
				} else {
					output += "\n\n\t\t\t" + precision + "4 Frag (VaryingsExtended i) : SV_Target {";
				}
				for (int j = 0; j < frag[i].Count; ++j){
					output += "\n\t\t\t\t" + frag[i][j];
				}
				output += "\n\t\t\t}";
				output += "\n\t\t\tENDHLSL";
				output += "\n\t\t}";
			}
			output += "\n\t}";
			output += "\n}";
			File.WriteAllBytes(path, System.Text.Encoding.UTF8.GetBytes(output));
			File.WriteAllBytes(secondPath, System.Text.Encoding.UTF8.GetBytes(output2));
			AssetDatabase.Refresh();
		}
		
		internal bool World(List<VisualElement> elements){
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].name == "WorldPosition"){
					return true;
				}
			}
			return false;
		}
		
		internal bool Depth(List<VisualElement> elements){
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].name == "CameraDepth" || elements[i].name == "WorldPosition"){
					return true;
				}
			}
			return false;
		}
		
		internal List<VisualProperty> FilterProps(List<VisualProperty> properties){
			List<VisualProperty> output = new List<VisualProperty>();
			for (int i = 0; i < properties.Count; ++i){
				if (properties[i].Values[0] == name){
					Debug.Log("Property name same as class name detected: " + properties[i].Values[0] + " is now " + properties[i].Values[0] + "Variable");
					properties[i].Values[0] += "Variable";
				}
			}
			bool check;
			for (int i = properties.Count - 1; i >= 0; --i){
				if (i > 0){
					check = true;
					for (int j = i - 1; j >= 0; --j){
						if (properties[i].Values[0] == properties[j].Values[0]){
							check = false;
							Debug.Log("Same property names found: " + properties[i].Values[0]);
							break;
						}
					}
					if (check){
						output.Add(properties[i]);
					}
				} else {
					output.Add(properties[0]);
				}
			}
			return output;
		}
		
		internal List<string> Parameters(List<VisualProperty> properties){
			List<string> output = new List<string>();
			for (int i = 0; i < properties.Count; ++i){
				switch (properties[i].name){
					case "Float":
					output.Add("[Tooltip(\""+ properties[i].Values[1] +"\")]");
					output.Add("public FloatParameter " + properties[i].Values[0] + " = new FloatParameter {value = " + properties[i].Values[2] + "f};");
					break;
					case "Int":
					output.Add("[Tooltip(\""+ properties[i].Values[1] +"\")]");
					output.Add("public IntParameter " + properties[i].Values[0] + " = new IntParameter {value = " + properties[i].Values[2] + "};");
					break;
					case "IntSlider":
					output.Add("[Range(" + properties[i].Values[3] + ", " + properties[i].Values[4] + "), Tooltip(\""+ properties[i].Values[1] +"\")]");
					output.Add("public IntParameter " + properties[i].Values[0] + " = new IntParameter {value = " + properties[i].Values[2] + "};");
					break;
					case "FloatSlider":
					output.Add("[Range(" + properties[i].Values[3] + "f, " + properties[i].Values[4] + "f), Tooltip(\""+ properties[i].Values[1] +"\")]");
					output.Add("public FloatParameter " + properties[i].Values[0] + " = new FloatParameter {value = " + properties[i].Values[2] + "f};");
					break;
					case "Color":
					output.Add("[Tooltip(\""+ properties[i].Values[1] +"\")]");
					output.Add("public ColorParameter " + properties[i].Values[0] + " = new ColorParameter {value = new Color(" + properties[i].Values[2] + "f, " + properties[i].Values[3] + "f, " + properties[i].Values[4] + "f, " + properties[i].Values[5] + "f)};");
					break;
					case "Vector2":
					output.Add("[Tooltip(\""+ properties[i].Values[1] +"\")]");
					output.Add("public Vector2Parameter " + properties[i].Values[0] + " = new Vector2Parameter {value = new Vector2(" + properties[i].Values[2] + "f, " + properties[i].Values[3] + "f)};");
					break;
					case "Vector3":
					output.Add("[Tooltip(\""+ properties[i].Values[1] +"\")]");
					output.Add("public Vector3Parameter " + properties[i].Values[0] + " = new Vector3Parameter {value = new Vector3(" + properties[i].Values[2] + "f, " + properties[i].Values[3] + "f, " + properties[i].Values[4] + "f)};");
					break;
					case "Vector4":
					output.Add("[Tooltip(\""+ properties[i].Values[1] +"\")]");
					output.Add("public Vector4Parameter " + properties[i].Values[0] + " = new Vector4Parameter {value = new Vector4(" + properties[i].Values[2] + "f, " + properties[i].Values[3] + "f, " + properties[i].Values[4] + "f, " + properties[i].Values[5] + "f)};");
					break;
					case "Texture2D":
					output.Add("[Tooltip(\""+ properties[i].Values[1] +"\")]");
					output.Add("public TextureParameter " + properties[i].Values[0] + " = new TextureParameter {value = null};");
					break;
					case "Spline":
					spline = true;
					evaluations.Add(properties[i].Values[0] + "EVALUATION");
					output.Add("[SerializeField]");
					output.Add("internal Texture2D " + properties[i].Values[0] + "EVALUATION;");
					output.Add("[Tooltip(\""+ properties[i].Values[1] +"\")]");
					output.Add("public SplineParameter " + properties[i].Values[0] + " = new SplineParameter {value = new Spline(new AnimationCurve(new Keyframe(0f, 0f, 1f, 1f), new Keyframe(1f, 1f, 1f, 1f)), 0f, false, new Vector2(0f, 1f))};");
					break;
					case "Bool":
					output.Add("[Tooltip(\""+ properties[i].Values[1] +"\")]");
					output.Add("public BoolParameter " + properties[i].Values[0] + " = new BoolParameter {value = " + properties[i].Values[2].ToLower() + "};");
					break;
				}
			}
			return output;
		}
		
		internal List<string> Inputs(List<VisualProperty> properties){
			List<string> output = new List<string>();
			for (int i = 0; i < properties.Count; ++i){
				switch (properties[i].name){
					case "Float":
					output.Add("sheet.properties.SetFloat(\"_" + properties[i].Values[0] + "\", settings." + properties[i].Values[0] + ");");
					break;
					case "Int":
					output.Add("sheet.properties.SetFloat(\"_" + properties[i].Values[0] + "\", settings." + properties[i].Values[0] + ");");
					break;
					case "IntSlider":
					output.Add("sheet.properties.SetFloat(\"_" + properties[i].Values[0] + "\", settings." + properties[i].Values[0] + ");");
					break;
					case "FloatSlider":
					output.Add("sheet.properties.SetFloat(\"_" + properties[i].Values[0] + "\", settings." + properties[i].Values[0] + ");");
					break;
					case "Color":
					output.Add("sheet.properties.SetColor(\"_" + properties[i].Values[0] + "\", settings." + properties[i].Values[0] + ");");
					break;
					case "Vector2":
					output.Add("sheet.properties.SetVector(\"_" + properties[i].Values[0] + "\", settings." + properties[i].Values[0] + ".value);");
					break;
					case "Vector3":
					output.Add("sheet.properties.SetVector(\"_" + properties[i].Values[0] + "\", settings." + properties[i].Values[0] + ".value);");
					break;
					case "Vector4":
					output.Add("sheet.properties.SetVector(\"_" + properties[i].Values[0] + "\", settings." + properties[i].Values[0] + ");");
					break;
					case "Texture2D":
					output.Add("if (settings." + properties[i].Values[0] + ".value != null) { sheet.properties.SetTexture(\"_" + properties[i].Values[0] + "\", settings." + properties[i].Values[0] + "); }");
					break;
					case "Spline":
					output.Add("sheet.properties.SetTexture(\"_" + properties[i].Values[0] + "\", settings." + properties[i].Values[0] + "EVALUATION);");
					break;
					case "Bool":
					output.Add("sheet.properties.SetFloat(\"_" + properties[i].Values[0] + "\", settings." + properties[i].Values[0] + " ? 1f : 0f);");
					break;
				}
			}
			return output;
		}
		
		internal List<string> Queue(RenderQueue renderQueue, List<List<VisualElement>> allElements){
			List<string> render = new List<string>();
			string source = "";
			string destination = "";
			string pass = "";
			string iterator = "";
			for (int i = 0; i < renderQueue.tempTextures; ++i){
				render.Add("settings.tempRT" + i + " = RenderTexture.GetTemporary(Screen.width, Screen.height);");
			}
			for (int i = 0; i < renderQueue.passes.Count; ++i){
				pass = renderQueue.passes[i].Pass.ToString();
				for (int j = 0; j < allElements[Int32.Parse(pass)].Count; ++j){
					if (allElements[Int32.Parse(pass)][j].name == "TempTex"){
						if (!allElements[Int32.Parse(pass)][j].Values[0].Contains("(0)")){
							render.Add("sheet.properties.SetTexture(\"_" + allElements[Int32.Parse(pass)][j].Values[0] + "\", settings." + allElements[Int32.Parse(pass)][j].Values[0] + ");");
						}
					}
				}
				if (renderQueue.passes[i].InputLabels[renderQueue.passes[i].Input] == "Game"){
					source = "context.source";
				} else {
					source = "settings." + renderQueue.passes[i].InputLabels[renderQueue.passes[i].Input];
				}
				if (renderQueue.passes[i].OutputLabels[renderQueue.passes[i].Output] == "Screen"){
					destination = "context.destination";
				} else {
					destination = "settings." + renderQueue.passes[i].OutputLabels[renderQueue.passes[i].Output];
				}
				if (renderQueue.passes[i].Variable == 0){
					iterator = renderQueue.passes[i].Iterations.ToString();
				} else {
					iterator = "settings." + renderQueue.passes[i].VariableLabels[renderQueue.passes[i].Variable];
				}
				if (renderQueue.passes[i].Iterations == 1 && renderQueue.passes[i].Variable == 0){
					render.Add("context.command.BlitFullscreenTriangle(" + source + ", " + destination + ", sheet, " + pass + ");");
				} else {
					render.Add("settings.loopRT0 = RenderTexture.GetTemporary(Screen.width, Screen.height);");
					render.Add("context.command.BlitFullscreenTriangle(" + source + ", settings.loopRT0);");
					render.Add("for (int i = 0; i < " + iterator + "; ++i)");
					render.Add("{");
					render.Add("\tsettings.loopRT1 = RenderTexture.GetTemporary(Screen.width, Screen.height);");
					render.Add("\tcontext.command.BlitFullscreenTriangle(settings.loopRT0, settings.loopRT1, sheet, " + pass + ");");
					render.Add("\tRenderTexture.ReleaseTemporary(settings.loopRT0);");
					render.Add("\tsettings.loopRT0 = settings.loopRT1;");
					render.Add("}");
					render.Add("context.command.BlitFullscreenTriangle(settings.loopRT0, " + destination + ");");
				}
			}
			return render;
		}
		
		internal List<string> Rel(RenderQueue renderQueue){
			List<string> render = new List<string>();
			for (int i = 0; i < renderQueue.tempTextures; ++i){
				render.Add("RenderTexture.ReleaseTemporary(settings.tempRT" + i + ");");
			}
			for (int i = 0; i < renderQueue.passes.Count; ++i){
				if (renderQueue.passes[i].Iterations != 1 || renderQueue.passes[i].Variable != 0){
					render.Add("RenderTexture.ReleaseTemporary(settings.loopRT0);");
					break;
				}
			}
			return render;
		}
		
		internal List<string> TempRT(int tempTextures){
			List<string> textures = new List<string>();
			for (int i = 0; i < tempTextures; ++i){
				textures.Add("internal RenderTexture tempRT" + i + ";");
			}
			if (loops){
				textures.Add("internal RenderTexture loopRT0;");
				textures.Add("internal RenderTexture loopRT1;");
			}
			return textures;
		}
		
		internal List<string> TexFiel(List<VisualElement> elements){
			List<string> fiel = new List<string>();
			string tempData = "";
			bool check = false;
			for (int i = 0; i < elements.Count; ++i){
				check = true;
				if (elements[i].name == "TexelSize"){
					tempData = precision + "4 " + elements[i].Values[0] + "_TexelSize;";
				}
				for (int j = 0; j < fiel.Count; ++j){
					if (tempData == fiel[j]){
						check = false;
						break;
					} else {
						check = true;
					}
				}
				if (check && tempData != ""){
					fiel.Add(tempData);
				}
			}
			return fiel;
		}
		
		internal List<string> TempFiel(List<VisualElement> elements){
			List<string> fiel = new List<string>();
			string tempData = "";
			bool check = false;
			for (int i = 0; i < elements.Count; ++i){
				check = true;
				if (elements[i].name == "TempTex" && !elements[i].Values[0].Contains("(0)")){
					tempData = "TEXTURE2D_SAMPLER2D(_" + elements[i].Values[0] + ", sampler_" + elements[i].Values[0] + ");";
				}
				for (int j = 0; j < fiel.Count; ++j){
					if (tempData == fiel[j]){
						check = false;
						break;
					} else {
						check = true;
					}
				}
				if (check && tempData != ""){
					fiel.Add(tempData);
				}
			}
			return fiel;
		}
		
		internal List<string> Fiel(List<VisualProperty> properties, List<List<VisualElement>> allElements){
			List<string> fiel = new List<string>();
			for (int i = 0; i < properties.Count; ++i){
				switch (properties[i].name){
					case "Float":
						fiel.Add("uniform " + precision + " _" + properties[i].Values[0] + ";");
					break;
					case "Int":
						fiel.Add("uniform " + precision + " _" + properties[i].Values[0] + ";");
					break;
					case "IntSlider":
						fiel.Add("uniform " + precision + " _" + properties[i].Values[0] + ";");
					break;
					case "FloatSlider":
						fiel.Add("uniform " + precision + " _" + properties[i].Values[0] + ";");
					break;
					case "Color":
						fiel.Add("uniform " + precision + "4 _" + properties[i].Values[0] + ";");
					break;
					case "Vector2":
						fiel.Add("uniform " + precision + "2 _" + properties[i].Values[0] + ";");
					break;
					case "Vector3":
						fiel.Add("uniform " + precision + "3 _" + properties[i].Values[0] + ";");
					break;
					case "Vector4":
						fiel.Add("uniform " + precision + "4 _" + properties[i].Values[0] + ";");
					break;
					case "Texture2D":
						fiel.Add("TEXTURE2D_SAMPLER2D(_" + properties[i].Values[0] + ", sampler_" + properties[i].Values[0] + ");");
					break;
					case "Spline":
						fiel.Add("TEXTURE2D_SAMPLER2D(_" + properties[i].Values[0] + ", sampler_" + properties[i].Values[0] + ");");
					break;
					case "Bool":
						fiel.Add("uniform bool _" + properties[i].Values[0] + ";");
					break;
				}
			}
			string tempData = "";
			bool check = false;
			for (int i = 0; i < allElements.Count; ++i){
				for (int j = 0; j < allElements[i].Count; ++j){
					check = true;
					if (allElements[i][j].name == "SpecialTex"){
						tempData = "TEXTURE2D_SAMPLER2D(" + allElements[i][j].Values[0] + ", sampler" + allElements[i][j].Values[0] + ");";
					}
					for (int k = 0; k < fiel.Count; ++k){
						if (tempData == fiel[k]){
							check = false;
							break;
						} else {
							check = true;
						}
					}
					if (check && tempData != ""){
						fiel.Add(tempData);
					}
				}
			}
			return fiel;
		}
		
		internal List<string> Texel(List<VisualProperty> properties, List<List<VisualElement>> allElements, List<VisualElement> elements){
			List<string> fiel = Fiel(properties, allElements);
			List<string> output = new List<string>();
			output.Add("_MainTex");
			if (Depth(elements)){
				output.Add("_CameraDepthTexture");
			}
			for (int i = 0; i < fiel.Count; ++i){
				if (fiel[i][0] == 'T'){
					output.Add(fiel[i].Substring(20, fiel[i].IndexOf(", ") - 20));
				}
			}
			return output;
		}
		
		internal List<int> Variables(List<VisualElement> elements){
			List<int> output = new List<int>();
			List<int> temp = new List<int>();
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].name.Length > 2 && elements[i].name.Substring(0, 3) == "Var"){
					temp.Add(i);
				}
			}
			for (int i = 0; i < temp.Count; ++i){
				for (int j = 0; j < temp.Count; ++j){
					if (elements[temp[j]].Values[0] == i.ToString()){
						output.Add(temp[j]);
					}
				}
			}
			return output;
		}
		
		internal List<int> Pres(List<VisualElement> elements){
			List<int> output = new List<int>();
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].name == "Checker"){
					elements[i].Values[1] = "pre" + output.Count;
					output.Add(i);
				}
			}
			return output;
		}
		
		internal virtual List<string> Frag(List<VisualElement> elements){
			List<string> frag = new List<string>();
			List<int> variables = Variables(elements);
			List<int> pres = Pres(elements);
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].name.Contains("Iterator") || elements[i].name.Contains("Loop")){
					frag.Add(precision + " IteratorVariable = 0.0;");
					break;
				}
			}
			for (int i = 0; i < variables.Count; ++i){
				frag.Add(precision + elements[variables[i]].name.Substring(elements[variables[i]].name.Length - 1) + " var" + elements[variables[i]].Values[0] + " = 0.0;");
			}
			frag.Add(precision + "4 CameraOutput = 0.0;");
			for (int i = 0; i < pres.Count; ++i){
				frag.AddRange(MacroCom2(elements, pres[i]));
			}
			for (int i = 0; i < variables.Count; ++i){
				frag.AddRange(VariableCom2(elements, variables[i]));
			}
			frag.AddRange(CameraOutputCom(elements, 0));
			frag.Add("return CameraOutput;");
			return frag;
		}
		
		internal virtual List<string> Customs(List<VisualElement> elements){
			List<string> customs = new List<string>();
			string tempData;
			bool unique = true;
			for (int i = 0; i < elements.Count; ++i){
				if (elements[i].name.Contains("Custom") && elements[i].Values[0].Contains("Assets/")){
					tempData = "#include \"../../" + elements[i].Values[0].Substring(elements[i].Values[0].IndexOf("Assets/") + 7) + "\"";
					unique = true;
					for (int j = 0; j < customs.Count; ++j){
						if (customs[j] == tempData){
							unique = false;
							break;
						} else {
							unique = true;
						}
					}
					if (unique){
						customs.Add(tempData);
					}
				}
			}
			return customs;
		}
		
		internal string Demand(List<VisualElement> elements, int element, int joint, string boss, int destination, int mode){
			string output = "";
			string data = "";
			int master;
			int slave;
			if (element < elements.Count){
				switch (elements[element].name){
					//=====Input=====
					case "_Float":
					data = SinglePropertyCom(elements, element)[joint];
					break;
					case "_Int":
					data = SinglePropertyCom(elements, element)[joint];
					break;
					case "_IntSlider":
					data = SinglePropertyCom(elements, element)[joint];
					break;
					case "_FloatSlider":
					data = SinglePropertyCom(elements, element)[joint];
					break;
					case "_Color":
					data = ColorPropertyCom(elements, element)[joint];
					break;
					case "_Vector2":
					data = VectorPropertyCom(elements, element)[joint];
					break;
					case "_Vector3":
					data = VectorPropertyCom(elements, element)[joint];
					break;
					case "_Vector4":
					data = VectorPropertyCom(elements, element)[joint];
					break;
					case "_Texture2D":
					data = TexturePropertyCom(elements, element)[joint];
					break;
					case "_Spline":
					data = SplinePropertyCom(elements, element)[joint];
					break;
					case "_Bool":
					data = SinglePropertyCom(elements, element)[joint];
					break;
					//=====Camera=====
					case "CameraInput":
					data = CameraInputCom(elements, element)[joint];
					break;
					case "CameraDepth":
					data = CameraDepthCom(elements, element)[joint];
					break;
					case "DefaultUV":
					data = DefaultUVCom(elements, element)[joint];
					break;
					case "StereoUV":
					data = StereoUVCom(elements, element)[joint];
					break;
					case "SpecialTex":
					data = SpecialTexCom(elements, element)[joint];
					break;
					case "TempTex":
					data = SpecialTexCom(elements, element)[joint];
					break;
					case "TexelSize":
					data = TexelSizeCom(elements, element)[joint];
					break;
					case "WorldPosition":
					data = WorldPositionCom(elements, element)[joint];
					break;
					//=====FE=====
					case "Input1":
					data = InputCom(elements, element)[joint];
					break;
					case "Input2":
					data = InputCom(elements, element)[joint];
					break;
					case "Input3":
					data = InputCom(elements, element)[joint];
					break;
					case "Input4":
					data = InputCom(elements, element)[joint];
					break;
					//=====Value=====
					case "Value1":
					data = ValueCom(elements, element)[joint];
					break;
					case "Value2":
					data = ValueCom(elements, element)[joint];
					break;
					case "Value3":
					data = ValueCom(elements, element)[joint];
					break;
					case "Value4":
					data = ValueCom(elements, element)[joint];
					break;
					//=====HLSL Function=====
					case "Abs":
					data = AbsCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Acos":
					data = AcosCom(elements, element, boss, destination, mode)[joint];
					break;
					case "All":
					data = AllCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Any":
					data = AnyCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Asin":
					data = AsinCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Atan":
					data = AtanCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Atan2":
					data = Atan2Com(elements, element, boss, destination, mode)[joint];
					break;
					case "Ceil":
					data = CeilCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Clamp":
					data = ClampCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Cos":
					data = CosCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Cosh":
					data = CoshCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Cross":
					data = CrossCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Ddx":
					data = DdxCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Ddy":
					data = DdyCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Degrees":
					data = DegreesCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Distance":
					data = DistanceCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Dot":
					data = DotCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Exp":
					data = ExpCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Exp2":
					data = Exp2Com(elements, element, boss, destination, mode)[joint];
					break;
					case "Floor":
					data = FloorCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Fmod":
					data = FmodCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Frac":
					data = FracCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Fwidth":
					data = FwidthCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Isfinite":
					data = IsfiniteCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Isinf":
					data = IsinfCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Isnan":
					data = IsnanCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Ldexp":
					data = LdexpCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Length":
					data = LengthCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Lerp":
					data = LerpCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Log":
					data = LogCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Log2":
					data = Log2Com(elements, element, boss, destination, mode)[joint];
					break;
					case "Log10":
					data = Log10Com(elements, element, boss, destination, mode)[joint];
					break;
					case "Max":
					data = MaxCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Min":
					data = MinCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Normalize":
					data = NormalizeCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Pow":
					data = PowCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Radians":
					data = RadiansCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Reflect":
					data = ReflectCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Refract":
					data = RefractCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Round":
					data = RoundCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Rsqrt":
					data = RsqrtCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Saturate":
					data = SaturateCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Sign":
					data = SignCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Sin":
					data = SinCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Sinh":
					data = SinhCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Smoothstep":
					data = SmoothstepCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Sqrt":
					data = SqrtCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Step":
					data = StepCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Tan":
					data = TanCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Tanh":
					data = TanhCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Trunc":
					data = TruncCom(elements, element, boss, destination, mode)[joint];
					break;
					//=====Arithmetic=====
					case "Add":
					data = AddCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Av":
					data = AvCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Sub":
					data = SubCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Mul":
					data = MulCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Div":
					data = DivCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Mod":
					data = ModCom(elements, element, boss, destination, mode)[joint];
					break;
					//=====Constant=====
					case "HALF_MAX":
					data = ConstantCom(elements, element)[joint];
					break;
					case "EPSILON":
					data = ConstantCom(elements, element)[joint];
					break;
					case "PI":
					data = ConstantCom(elements, element)[joint];
					break;
					case "TWO_PI":
					data = ConstantCom(elements, element)[joint];
					break;
					case "FOUR_PI":
					data = ConstantCom(elements, element)[joint];
					break;
					case "INV_PI":
					data = ConstantCom(elements, element)[joint];
					break;
					case "INV_TWO_PI":
					data = ConstantCom(elements, element)[joint];
					break;
					case "INV_FOUR_PI":
					data = ConstantCom(elements, element)[joint];
					break;
					case "HALF_PI":
					data = ConstantCom(elements, element)[joint];
					break;
					case "INV_HALF_PI":
					data = ConstantCom(elements, element)[joint];
					break;
					case "FLT_EPSILON":
					data = ConstantCom(elements, element)[joint];
					break;
					case "FLT_MIN":
					data = ConstantCom(elements, element)[joint];
					break;
					case "FLT_MAX":
					data = ConstantCom(elements, element)[joint];
					break;
					//=====Data=====
					case "WorldSpace":
					data = DataCom(elements, element)[joint];
					break;
					case "Projection":
					data = DataCom(elements, element)[joint];
					break;
					case "Luminance":
					data = DataCom(elements, element)[joint];
					break;
					case "DeltaTime":
					data = DataCom(elements, element)[joint];
					break;
					case "Ortho":
					data = DataCom(elements, element)[joint];
					break;
					case "ZBuffer":
					data = DataCom(elements, element)[joint];
					break;
					case "Screen":
					data = DataCom(elements, element)[joint];
					break;
					case "Time":
					data = DataCom(elements, element)[joint];
					break;
					case "SinTime":
					data = DataCom(elements, element)[joint];
					break;
					case "CosTime":
					data = DataCom(elements, element)[joint];
					break;
					//=====Logic=====
					case "If":
					data = IfCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Compare":
					data = CompareCom(elements, element, boss, destination, mode)[joint];
					break;
					case "And":
					data = AndCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Or":
					data = OrCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Not":
					data = NotCom(elements, element, boss, destination, mode)[joint];
					break;
					//=====Variable=====
					case "Variable1":
					data = VariableCom(elements, element)[joint];
					break;
					case "Variable2":
					data = VariableCom(elements, element)[joint];
					break;
					case "Variable3":
					data = VariableCom(elements, element)[joint];
					break;
					case "Variable4":
					data = VariableCom(elements, element)[joint];
					break;
					case "Iterator":
					data = "IteratorVariable";
					break;
					case "VarLoop1":
					data = VariableCom(elements, element)[joint];
					break;
					case "VarLoop2":
					data = VariableCom(elements, element)[joint];
					break;
					case "VarLoop3":
					data = VariableCom(elements, element)[joint];
					break;
					case "VarLoop4":
					data = VariableCom(elements, element)[joint];
					break;
					//=====Custom=====
					case "Custom1":
					data = CustomCom(elements, element)[joint];
					break;
					case "Custom2":
					data = CustomCom(elements, element)[joint];
					break;
					case "Custom3":
					data = CustomCom(elements, element)[joint];
					break;
					case "Custom4":
					data = CustomCom(elements, element)[joint];
					break;
					//=====PPS Function=====
					case "AnyIsNan":
					data = AnyIsNanCom(elements, element, boss, destination, mode)[joint];
					break;
					case "DecodeStereo":
					data = DecodeStereoCom(elements, element)[joint];
					break;
					case "FastSign":
					data = FastSignCom(elements, element, boss, destination, mode)[joint];
					break;
					case "GradientNoise":
					data = GradientNoiseCom(elements, element)[joint];
					break;
					case "IsNan":
					data = IsNanCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Linear01Depth":
					data = Linear01DepthCom(elements, element, boss, destination, mode)[joint];
					break;
					case "LinearEyeDepth":
					data = LinearEyeDepthCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Max3":
					data = Max3Com(elements, element, boss, destination, mode)[joint];
					break;
					case "Min3":
					data = Min3Com(elements, element, boss, destination, mode)[joint];
					break;
					case "PositivePow":
					data = PositivePowCom(elements, element, boss, destination, mode)[joint];
					break;
					case "Rcp":
					data = RcpCom(elements, element, boss, destination, mode)[joint];
					break;
					case "SafeHDR":
					data = SafeHDRCom(elements, element)[joint];
					break;
					case "TriangleVertToUV":
					data = TriangleVertToUVCom(elements, element)[joint];
					break;
					//=====Predefined Macro=====
					case "Checker":
					data = MacroCom(elements, element)[joint];
					break;
					case "NearClipValue":
					data = MacroCom(elements, element)[joint];
					break;
					case "StartsAtTop":
					data = MacroCom(elements, element)[joint];
					break;
					case "Target":
					data = MacroCom(elements, element)[joint];
					break;
					case "Version":
					data = MacroCom(elements, element)[joint];
					break;
				}
			} else {
				data = "0.0";
			}
			if (element < elements.Count){
				master = GetSize(boss, destination);
				slave = GetSize(elements[element].name, joint);
				if (master > slave && slave != 1 && mode > 0){
					output = precision + master + "(" + data;
					for (int i = 0; i < master - slave; ++i){
						output += ",0.0";
					}
					output += ")";
				} else {
					if (mode == -1 && master != slave){
						output += data + ".x";
					} else {
						output = data;
					}
				}
			} else {
				output = data;
			}
			return output;
		}
		
		internal string Demand(List<VisualElement> elements, VisualElement me, int joint, string boss, int mode){
			return me.joints[joint].connections != null ? Demand(elements, (int) me.joints[joint].connections.serial.x, (int) me.joints[joint].connections.serial.y, boss, joint, mode) : "0.0";
		}
		
		internal string Demand(List<VisualElement> elements, VisualElement me, int joint, string boss, int destination, int mode){
			return me.joints[joint].connections != null ? Demand(elements, (int) me.joints[joint].connections.serial.x, (int) me.joints[joint].connections.serial.y, boss, destination, mode) : "0.0";
		}
		
		//=====Input=====
		internal List<string> SinglePropertyCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			if (me.Values[1] != "0"){
				output.Add("_" + me.Values[0]);
			} else {
				output.Add("0.0");
			}
			return output;
		}
		
		internal List<string> ColorPropertyCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			if (me.Values[1] != "0"){
				output.Add("_" + me.Values[0]);
				output.Add("_" + me.Values[0] + ".r");
				output.Add("_" + me.Values[0] + ".g");
				output.Add("_" + me.Values[0] + ".b");
				output.Add("_" + me.Values[0] + ".a");
			} else {
				output.Add("0.0");
			}
			return output;
		}
		
		internal List<string> VectorPropertyCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			if (me.Values[1] != "0"){
				switch (me.name){
					case "_Vector2":
					output.Add("_" + me.Values[0]);
					output.Add("_" + me.Values[0] + ".x");
					output.Add("_" + me.Values[0] + ".y");
					break;
					case "_Vector3":
					output.Add("_" + me.Values[0]);
					output.Add("_" + me.Values[0] + ".x");
					output.Add("_" + me.Values[0] + ".y");
					output.Add("_" + me.Values[0] + ".z");
					break;
					case "_Vector4":
					output.Add("_" + me.Values[0]);
					output.Add("_" + me.Values[0] + ".x");
					output.Add("_" + me.Values[0] + ".y");
					output.Add("_" + me.Values[0] + ".z");
					output.Add("_" + me.Values[0] + ".w");
					break;
				}
			} else {
				output.Add("0.0");
			}
			return output;
		}
		
		internal List<string> TexturePropertyCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			if (me.Values[1] != "0"){
				List<string> demands = new List<string>();
				demands.Add(me.joints[5].connections != null ? Demand(elements, (int) me.joints[5].connections.serial.x, (int) me.joints[5].connections.serial.y, "TextureProperty", 5, -1) : "0.0");
				demands.Add(me.joints[6].connections != null ? Demand(elements, (int) me.joints[6].connections.serial.x, (int) me.joints[6].connections.serial.y, "TextureProperty", 6, -1) : "0.0");
				for (int i = 0; i < 5; ++i){
					if (me.joints[7].connections != null){
						output.Add("SAMPLE_TEXTURE2D(_" + me.Values[0] + ", " + "sampler_" + me.Values[0] + ", " + Demand(elements, (int) me.joints[7].connections.serial.x, (int) me.joints[7].connections.serial.y, "TextureProperty", 7, 1) + GetData("TextureProperty", i));
					} else {
						output.Add("SAMPLE_TEXTURE2D(_" + me.Values[0] + ", " + "sampler_" + me.Values[0] + ", " + precision + "2(" + demands[0] + "," + demands[1] + ")" + GetData("TextureProperty", i));
					}
				}
			} else {
				output.Add("0.0");
			}
			return output;
		}
		
		internal List<string> SplinePropertyCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			if (me.Values[1] != "0"){
				List<string> demands = new List<string>();
				demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, "TextureProperty", 5, -1) : "0.0");
				output.Add("SAMPLE_TEXTURE2D(_" + me.Values[0] + ", " + "sampler_" + me.Values[0] + ", " + precision + "2(" + demands[0] + ", 0.5)).a");
			} else {
				output.Add("0.0");
			}
			return output;
		}
		
		//=====Camera=====
		internal List<string> CameraOutputCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			for (int i = 0; i < me.joints.Count; ++i){
				if (me.joints[i].connections != null){
					output.Add(GetData("CameraOutput", i) + Demand(elements, (int) me.joints[i].connections.serial.x, (int) me.joints[i].connections.serial.y, "CameraOutput", i, 1) + ";");
				}
			}
			return output;
		}
		
		internal List<string> CameraInputCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[4].connections != null ? Demand(elements, (int) me.joints[4].connections.serial.x, (int) me.joints[4].connections.serial.y, "CameraInput", 4, -1) : "0.0");
			demands.Add(me.joints[5].connections != null ? Demand(elements, (int) me.joints[5].connections.serial.x, (int) me.joints[5].connections.serial.y, "CameraInput", 5, -1) : "0.0");
			for (int i = 0; i < 4; ++i){
				if (me.joints[6].connections != null){
					output.Add("SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, " + Demand(elements, (int) me.joints[6].connections.serial.x, (int) me.joints[6].connections.serial.y, "CameraInput", 6, 1) + GetData("CameraInput", i));
				} else {
					output.Add("SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, " + precision + "2(" + demands[0] + "," + demands[1] + ")" + GetData("CameraInput", i));
				}
			}
			return output;
		}
		
		internal List<string> CameraDepthCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, "CameraDepth", 4, -1) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, "CameraDepth", 5, -1) : "0.0");
			for (int i = 0; i < 4; ++i){
				if (me.joints[3].connections != null){
					output.Add("SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, " + Demand(elements, (int) me.joints[3].connections.serial.x, (int) me.joints[3].connections.serial.y, "CameraDepth", 3, 1) + ").x");
				} else {
					output.Add("SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, " + precision + "2(" + demands[0] + "," + demands[1] + ")).x");
				}
			}
			return output;
		}
		
		internal List<string> DefaultUVCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			for (int i = 0; i < me.joints.Count; ++i){
				output.Add(GetData("DefaultUV", i));
			}
			return output;
		}
		
		internal List<string> StereoUVCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			for (int i = 0; i < me.joints.Count; ++i){
				output.Add(GetData("StereoUV", i));
			}
			return output;
		}
		
		internal List<string> SpecialTexCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			string under = me.name == "TempTex" ? "_" : "";
			if (under == ""){
				demands.Add(me.joints[5].connections != null ? Demand(elements, (int) me.joints[5].connections.serial.x, (int) me.joints[5].connections.serial.y, "CameraInput", 5, -1) : "0.0");
				demands.Add(me.joints[6].connections != null ? Demand(elements, (int) me.joints[6].connections.serial.x, (int) me.joints[6].connections.serial.y, "CameraInput", 6, -1) : "0.0");
				for (int i = 0; i < 5; ++i){
					if (me.joints[7].connections != null){
						output.Add("SAMPLE_TEXTURE2D(" + under + me.Values[0] + ", sampler" + under + me.Values[0] + ", " + Demand(elements, (int) me.joints[7].connections.serial.x, (int) me.joints[7].connections.serial.y, "CameraInput", 6, 1) + GetData("SafeHDR", i));
					} else {
						output.Add("SAMPLE_TEXTURE2D(" + under + me.Values[0] + ", sampler" + under + me.Values[0] + ", " + precision + "2(" + demands[0] + "," + demands[1] + ")" + GetData("SafeHDR", i));
					}
				}
			} else {
				demands.Add(me.joints[4].connections != null ? Demand(elements, (int) me.joints[4].connections.serial.x, (int) me.joints[4].connections.serial.y, "CameraInput", 4, -1) : "0.0");
				demands.Add(me.joints[5].connections != null ? Demand(elements, (int) me.joints[5].connections.serial.x, (int) me.joints[5].connections.serial.y, "CameraInput", 5, -1) : "0.0");
				for (int i = 0; i < 4; ++i){
					if (!me.Values[0].Contains("(0)")){
						if (me.joints[6].connections != null){
							output.Add("SAMPLE_TEXTURE2D(" + under + me.Values[0] + ", sampler" + under + me.Values[0] + ", " + Demand(elements, (int) me.joints[6].connections.serial.x, (int) me.joints[6].connections.serial.y, "CameraInput", 6, 1) + GetData("CameraInput", i));
						} else {
							output.Add("SAMPLE_TEXTURE2D(" + under + me.Values[0] + ", sampler" + under + me.Values[0] + ", " + precision + "2(" + demands[0] + "," + demands[1] + ")" + GetData("SafeHDR", i));
						}
					} else {
						output.Add("0.0");
					}
				}
			}
			return output;
		}
		
		internal List<string> TexelSizeCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			output.Add(me.Values[0] + "_TexelSize.x");
			output.Add(me.Values[0] + "_TexelSize.y");
			output.Add(me.Values[0] + "_TexelSize.z");
			output.Add(me.Values[0] + "_TexelSize.w");
			output.Add(me.Values[0] + "_TexelSize");
			return output;
		}
		
		internal List<string> WorldPositionCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			string ret = "(i.worldDirection * LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoordStereo)) + _WorldSpaceCameraPos)";
			output.Add(ret + ".x");
			output.Add(ret + ".y");
			output.Add(ret + ".z");
			output.Add(ret);
			return output;
		}
		
		//=====FE=====
		internal List<string> InputCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			switch (me.name.Substring(5)){
				case "1":
					output.Add(me.Values[0]);
				break;
				case "2":
					output.Add(me.Values[0] + ".x");
					output.Add(me.Values[0] + ".y");
					output.Add(me.Values[0]);
				break;
				case "3":
					output.Add(me.Values[0] + ".x");
					output.Add(me.Values[0] + ".y");
					output.Add(me.Values[0] + ".z");
					output.Add(me.Values[0]);
				break;
				case "4":
					output.Add(me.Values[0] + ".x");
					output.Add(me.Values[0] + ".y");
					output.Add(me.Values[0] + ".z");
					output.Add(me.Values[0] + ".w");
					output.Add(me.Values[0]);
				break;
			}
			return output;
		}
		
		//=====Value=====
		internal List<string> ValueCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			for (int i = 0; i < me.Values.Count; ++i){
				if (!me.Values[i].ToLower().Contains("e") && !me.Values[i].ToLower().Contains(".")){
					me.Values[i] += ".0";
				}
			}
			switch (me.name){
				case "Value1":
				output.Add(me.Values[0]);
				break;
				case "Value2":
				output.Add(me.Values[0]);
				output.Add(me.Values[1]);
				output.Add(precision + "2(" + me.Values[0] + "," + me.Values[1] + ")");
				break;
				case "Value3":
				output.Add(me.Values[0]);
				output.Add(me.Values[1]);
				output.Add(me.Values[2]);
				output.Add(precision + "3(" + me.Values[0] + "," + me.Values[1] + "," + me.Values[2] + ")");
				break;
				case "Value4":
				output.Add(me.Values[0]);
				output.Add(me.Values[1]);
				output.Add(me.Values[2]);
				output.Add(me.Values[3]);
				output.Add(precision + "4(" + me.Values[0] + "," + me.Values[1] + "," + me.Values[2] + "," + me.Values[3] + ")");
				break;
			}
			return output;
		}
		
		//=====HLSL Function=====
		internal List<string> AbsCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("abs(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> AcosCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("acos(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> AllCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("all(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> AnyCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("any(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> AsinCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("asin(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> AtanCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("atan(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> Atan2Com(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("atan2(" + demands[0] + "," + demands[1] + ")");
			return output;
		}
		
		internal List<string> CeilCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("ceil(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> ClampCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[3].connections != null ? Demand(elements, (int) me.joints[3].connections.serial.x, (int) me.joints[3].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("clamp(" + demands[0] + "," + demands[1] + "," + demands[2] + ")");
			return output;
		}
		
		internal List<string> CosCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("cos(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> CoshCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("cosh(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> CrossCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, "Cross", 0, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, "Cross", 0, mode) : "0.0");
			output.Add("cross(" + demands[0] + "," + demands[1] + ")");
			return output;
		}
		
		internal List<string> DdxCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("ddx(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> DdyCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("ddy(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> DegreesCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("degrees(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> DistanceCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("distance(" + demands[0] + "," + demands[1] + ")");
			return output;
		}
		
		internal List<string> DotCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("dot(" + demands[0] + "," + demands[1] + ")");
			return output;
		}
		
		internal List<string> ExpCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("exp(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> Exp2Com(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("exp2(" + demands[0] + ")");
			return output;
		}
		
		
		internal List<string> FloorCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("floor(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> FmodCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("fmod(" + demands[0] + "," + demands[1] + ")");
			return output;
		}
		
		internal List<string> FracCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("frac(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> FwidthCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("fwidth(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> IsfiniteCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("isfinite(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> IsinfCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("isinf(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> IsnanCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("isnan(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> LdexpCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("ldexp(" + demands[0] + "," + demands[1] + ")");
			return output;
		}
		
		internal List<string> LengthCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("length(" + demands[0] + ")");
			return output;
		}
		
		
		internal List<string> LerpCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[3].connections != null ? Demand(elements, (int) me.joints[3].connections.serial.x, (int) me.joints[3].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("lerp(" + demands[0] + "," + demands[1] + "," + demands[2] + ")");
			return output;
		}
		
		internal List<string> LogCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("log(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> Log2Com(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("log2(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> Log10Com(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("log10(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> MaxCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("max(" + demands[0] + "," + demands[1] + ")");
			return output;
		}
		
		internal List<string> MinCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("min(" + demands[0] + "," + demands[1] + ")");
			return output;
		}
		
		internal List<string> NormalizeCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("normalize(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> PowCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("pow(" + demands[0] + "," + demands[1] + ")");
			return output;
		}
		
		internal List<string> RadiansCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("radians(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> ReflectCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("reflect(" + demands[0] + "," + demands[1] + ")");
			return output;
		}
		
		internal List<string> RefractCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[3].connections != null ? Demand(elements, (int) me.joints[3].connections.serial.x, (int) me.joints[3].connections.serial.y, "Refract", destination, -1) : "0.0");
			output.Add("refract(" + demands[0] + "," + demands[1] + "," + demands[2] + ")");
			return output;
		}
		
		internal List<string> RoundCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("round(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> RsqrtCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("rsqrt(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> SaturateCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("saturate(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> SignCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("sign(" + demands[0] + ")");
			return output;
		}
		
		
		internal List<string> SinCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("sin(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> SinhCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("sinh(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> SmoothstepCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[3].connections != null ? Demand(elements, (int) me.joints[3].connections.serial.x, (int) me.joints[3].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("smoothstep(" + demands[0] + "," + demands[1] + "," + demands[2] + ")");
			return output;
		}
		
		internal List<string> SqrtCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("sqrt(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> StepCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("step(" + demands[0] + "," + demands[1] + ")");
			return output;
		}
		
		internal List<string> TanCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("tan(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> TanhCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("tanh(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> TruncCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("trunc(" + demands[0] + ")");
			return output;
		}
		
		//=====Arithmetic=====
		internal List<string> AddCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			for (int i = 1; i < me.joints.Count; ++i){
				try {
					demands.Add(me.joints[i].connections != null ? Demand(elements, (int) me.joints[i].connections.serial.x, (int) me.joints[i].connections.serial.y, boss, destination, mode) : "NULL");
				} catch {
					demands.Add("NULL");
				}
			}
			output.Add("(");
			for (int i = 0; i < demands.Count; ++i){
				if (demands[i] != "NULL"){
					if (output[0] == "("){
						output[0] += demands[i];
					} else {
						output[0] += " + " + demands[i];
					}
				}
			}
			output[0] += ")";
			if (output[0] == "()"){
				output[0] = "0.0";
			}
			return output;
		}
		
		internal List<string> AvCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			int count = 0;
			for (int i = 1; i < me.joints.Count; ++i){
				demands.Add(me.joints[i].connections != null ? Demand(elements, (int) me.joints[i].connections.serial.x, (int) me.joints[i].connections.serial.y, boss, destination, mode) : "NULL");
			}
			output.Add("");
			for (int i = 0; i < demands.Count; ++i){
				if (demands[i] != "NULL"){
					++count;
					if (output[0] == ""){
						output[0] += demands[i];
					} else {
						output[0] += " + " + demands[i];
					}
				}
			}
			if (count == 0){
				output[0] = "0.0";
			} else if (count != 1){
				output[0] = "((" + output[0] + ") / " + count + ".0)";
			}
			return output;
		}
		
		internal List<string> SubCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			for (int i = 1; i < me.joints.Count; ++i){
				try {
					demands.Add(me.joints[i].connections != null ? Demand(elements, (int) me.joints[i].connections.serial.x, (int) me.joints[i].connections.serial.y, boss, destination, mode) : "NULL");
				} catch {
					demands.Add("NULL");
				}
			}
			output.Add("(");
			for (int i = 0; i < demands.Count; ++i){
				if (demands[i] != "NULL"){
					if (output[0] == "("){
						output[0] += demands[i];
					} else {
						output[0] += " - " + demands[i];
					}
				}
			}
			output[0] += ")";
			if (output[0] == "()"){
				output[0] = "0.0";
			}
			return output;
		}
		
		internal List<string> MulCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			for (int i = 1; i < me.joints.Count; ++i){
				try {
					demands.Add(me.joints[i].connections != null ? Demand(elements, (int) me.joints[i].connections.serial.x, (int) me.joints[i].connections.serial.y, boss, destination, mode) : "NULL");
				} catch {
					demands.Add("NULL");
				}
			}
			output.Add("(");
			for (int i = 0; i < demands.Count; ++i){
				if (demands[i] != "NULL"){
					if (output[0] == "("){
						output[0] += demands[i];
					} else {
						output[0] += " * " + demands[i];
					}
				}
			}
			output[0] += ")";
			if (output[0] == "()"){
				output[0] = "0.0";
			}
			return output;
		}
		
		internal List<string> DivCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			for (int i = 1; i < me.joints.Count; ++i){
				try {
					demands.Add(me.joints[i].connections != null ? Demand(elements, (int) me.joints[i].connections.serial.x, (int) me.joints[i].connections.serial.y, boss, destination, mode) : "NULL");
				} catch {
					demands.Add("NULL");
				}
			}
			output.Add("(");
			for (int i = 0; i < demands.Count; ++i){
				if (demands[i] != "NULL"){
					if (output[0] == "("){
						output[0] += demands[i];
					} else {
						output[0] += " / " + demands[i];
					}
				}
			}
			output[0] += ")";
			if (output[0] == "()"){
				output[0] = "0.0";
			}
			return output;
		}
		
		internal List<string> ModCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			for (int i = 1; i < me.joints.Count; ++i){
				try {
					demands.Add(me.joints[i].connections != null ? Demand(elements, (int) me.joints[i].connections.serial.x, (int) me.joints[i].connections.serial.y, boss, destination, mode) : "NULL");
				} catch {
					demands.Add("NULL");
				}
			}
			output.Add("(");
			for (int i = 0; i < demands.Count; ++i){
				if (demands[i] != "NULL"){
					if (output[0] == "("){
						output[0] += demands[i];
					} else {
						output[0] += " % " + demands[i];
					}
				}
			}
			output[0] += ")";
			if (output[0] == "()"){
				output[0] = "0.0";
			}
			return output;
		}
		
		//=====Constant=====
		internal List<string> ConstantCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			output.Add(me.name);
			return output;
		}
		
		//=====Data=====
		internal List<string> DataCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			switch (me.name){
				case "WorldSpace":
				output.Add("_WorldSpaceCameraPos.x");
				output.Add("_WorldSpaceCameraPos.y");
				output.Add("_WorldSpaceCameraPos.z");
				output.Add("_WorldSpaceCameraPos");
				break;
				case "Projection":
				output.Add("_ProjectionParams.x");
				output.Add("_ProjectionParams.y");
				output.Add("_ProjectionParams.z");
				output.Add("_ProjectionParams.w");
				output.Add("_ProjectionParams");
				break;
				case "Luminance":
				output.Add("unity_ColorSpaceLuminance.x");
				output.Add("unity_ColorSpaceLuminance.y");
				output.Add("unity_ColorSpaceLuminance.z");
				output.Add("unity_ColorSpaceLuminance.w");
				output.Add("unity_ColorSpaceLuminance");
				break;
				case "DeltaTime":
				output.Add("unity_DeltaTime.x");
				output.Add("unity_DeltaTime.y");
				output.Add("unity_DeltaTime.z");
				output.Add("unity_DeltaTime.w");
				output.Add("unity_DeltaTime");
				break;
				case "Ortho":
				output.Add("unity_OrthoParams.x");
				output.Add("unity_OrthoParams.y");
				output.Add("unity_OrthoParams.z");
				output.Add("unity_OrthoParams.w");
				output.Add("unity_OrthoParams");
				break;
				case "ZBuffer":
				output.Add("_ZBufferParams.x");
				output.Add("_ZBufferParams.y");
				output.Add("_ZBufferParams.z");
				output.Add("_ZBufferParams.w");
				output.Add("_ZBufferParams");
				break;
				case "Screen":
				output.Add("_ScreenParams.x");
				output.Add("_ScreenParams.y");
				output.Add("_ScreenParams.z");
				output.Add("_ScreenParams.w");
				output.Add("_ScreenParams");
				break;
				case "Time":
				output.Add("_Time.x");
				output.Add("_Time.y");
				output.Add("_Time.z");
				output.Add("_Time.w");
				output.Add("_Time");
				break;
				case "SinTime":
				output.Add("_SinTime.x");
				output.Add("_SinTime.y");
				output.Add("_SinTime.z");
				output.Add("_SinTime.w");
				output.Add("_SinTime");
				break;
				case "CosTime":
				output.Add("_CosTime.x");
				output.Add("_CosTime.y");
				output.Add("_CosTime.z");
				output.Add("_CosTime.w");
				output.Add("_CosTime");
				break;
			}
			return output;
		}
		
		//=====Logic=====
		internal List<string> IfCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, 1) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[3].connections != null ? Demand(elements, (int) me.joints[3].connections.serial.x, (int) me.joints[3].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("(" + demands[0] + " ? " + demands[1] + " : " + demands[2] + ")");
			return output;
		}
		
		internal List<string> CompareCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[6].connections != null ? Demand(elements, (int) me.joints[6].connections.serial.x, (int) me.joints[6].connections.serial.y, boss, destination, 1) : "0.0");
			demands.Add(me.joints[7].connections != null ? Demand(elements, (int) me.joints[7].connections.serial.x, (int) me.joints[7].connections.serial.y, boss, destination, 1) : "0.0");
			demands.Add(me.joints[8].connections != null ? Demand(elements, (int) me.joints[8].connections.serial.x, (int) me.joints[8].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[9].connections != null ? Demand(elements, (int) me.joints[9].connections.serial.x, (int) me.joints[9].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("(" + demands[0] + " > " + demands[1] + " ? " + demands[2] + " : " + demands[3] + ")");
			output.Add("(" + demands[0] + " >= " + demands[1] + " ? " + demands[2] + " : " + demands[3] + ")");
			output.Add("(" + demands[0] + " < " + demands[1] + " ? " + demands[2] + " : " + demands[3] + ")");
			output.Add("(" + demands[0] + " <= " + demands[1] + " ? " + demands[2] + " : " + demands[3] + ")");
			output.Add("(" + demands[0] + " == " + demands[1] + " ? " + demands[2] + " : " + demands[3] + ")");
			output.Add("(" + demands[0] + " != " + demands[1] + " ? " + demands[2] + " : " + demands[3] + ")");
			return output;
		}
		
		internal List<string> AndCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, 1) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, 1) : "0.0");
			output.Add("(" + demands[0] + " && " + demands[1] + ")");
			return output;
		}
		
		internal List<string> OrCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, 1) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, 1) : "0.0");
			output.Add("(" + demands[0] + " || " + demands[1] + ")");
			return output;
		}
		
		internal List<string> NotCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, 1) : "0.0");
			output.Add("!" + demands[0]);
			return output;
		}
		
		//=====Variable=====
		internal List<string> VariableCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			switch (me.name[me.name.Length - 1].ToString()){
				case "1":
					output.Add("var" + me.Values[0]);
				break;
				case "2":
					output.Add("var" + me.Values[0] + ".x");
					output.Add("var" + me.Values[0] + ".y");
					output.Add("var" + me.Values[0]);
				break;
				case "3":
					output.Add("var" + me.Values[0] + ".x");
					output.Add("var" + me.Values[0] + ".y");
					output.Add("var" + me.Values[0] + ".z");
					output.Add("var" + me.Values[0]);
				break;
				case "4":
					output.Add("var" + me.Values[0] + ".x");
					output.Add("var" + me.Values[0] + ".y");
					output.Add("var" + me.Values[0] + ".z");
					output.Add("var" + me.Values[0] + ".w");
					output.Add("var" + me.Values[0]);
				break;
			}
			return output;
		}
		
		internal List<string> VariableCom2(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			for (int i = 0; i < me.joints.Count; ++i){
				if (me.joints[i].connections != null && me.joints[i].type == 0 && !me.name.Contains("Loop")){
					output.Add("var" + me.Values[0].ToString() + GetData(me.name, i) + " = " + Demand(elements, (int) me.joints[i].connections.serial.x, (int) me.joints[i].connections.serial.y, me.name, i, 1) + ";");
				}
			}
			if (me.name.Contains("Loop")){
				string sign = "";
				int size = 0;
				switch (me.name[me.name.Length - 1].ToString()){
					case "1":
						size = 2;
					break;
					case "2":
						size = 4;
					break;
					case "3":
						size = 5;
					break;
					case "4":
						size = 6;
					break;
				}
				switch (me.Values[2]){
					case "0":
						sign = "+";
					break;
					case "1":
						sign = "-";
					break;
					case "2":
						sign = "*";
					break;
					case "3":
						sign = "/";
					break;
					case "4":
						sign = "%";
					break;
				}
				string startValue = Demand(elements, me, size + 1, me.name, -1);
				string iterationsCount = "(" + startValue + " + " + Demand(elements, me, size, me.name, -1) + ")";
				output.Add("for (IteratorVariable = " + startValue + "; IteratorVariable < " + iterationsCount + "; ++IteratorVariable){ var" +  me.Values[0].ToString() + " " + sign + "= " + Demand(elements, me, size - 1, me.name, 1) + "; }");
			}
			return output;
		}
		
		//=====Custom=====
		internal List<string> CustomCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			List<string> jointsDemands = new List<string>();
			List<List<string>> groupedJoints = new List<List<string>>();
			VisualElement me = elements[element];
			List<VisualElement.Joint> leftJoints = new List<VisualElement.Joint>();
			for (int i = 0; i < me.joints.Count; ++i){
				if (me.joints[i].type == 0){
					leftJoints.Add(me.joints[i]);
				}
			}
			for (int i = 0; i < leftJoints.Count; ++i){
				if (leftJoints[i].connections != null){
					jointsDemands.Add(Demand(elements, (int) leftJoints[i].connections.serial.x, (int) leftJoints[i].connections.serial.y, "Demand", leftJoints[i].size, leftJoints[i].size == 1 ? -1 : 1));
				} else {
					jointsDemands.Add("0.0");
				}
			}
			List<string> ins = me.Values[3].Split(',').ToList();
			List<string> finalInputs = new List<string>();
			List<int> lastInputs = new List<int>();
			for (int i = 0; i < ins.Count; ++i){
				finalInputs.Add("");
				lastInputs.Add(0);
			}
			int amount = 0;
			int x = 0;
			for (int i = 0; i < ins.Count; ++i){
				groupedJoints.Add(new List<string>());
				amount = Int32.Parse(ins[i]) + (ins[i] == "1" ? 0 : 1);
				for (int j = 0; j < amount; ++j){
					groupedJoints[groupedJoints.Count - 1].Add(jointsDemands[x]);
					if (j == amount - 1){
						lastInputs[i] = x;
					}
					++x;
				}
			}
			for (int i = 0; i < ins.Count; ++i){
				if (i == 0){
					switch (ins[i]){
						case "1":
							finalInputs[i] = groupedJoints[i][0];
						break;
						case "2":
							if (leftJoints[lastInputs[i]].connections != null){
								finalInputs[i] = groupedJoints[i][2];
							} else {
								finalInputs[i] = "half2(" + groupedJoints[i][0] + ", " +  groupedJoints[i][1] + ")";
							}
						break;
						case "3":
							if (leftJoints[lastInputs[i]].connections != null){
								finalInputs[i] = groupedJoints[i][3];
							} else {
								finalInputs[i] = "half3(" + groupedJoints[i][0] + ", " +  groupedJoints[i][1] + ", " +  groupedJoints[i][2] + ")";
							}
						break;
						case "4":
							if (leftJoints[lastInputs[i]].connections != null){
								finalInputs[i] = groupedJoints[i][4];
							} else {
								finalInputs[i] = "half4(" + groupedJoints[i][0] + ", " +  groupedJoints[i][1] + ", " +  groupedJoints[i][2] + ", " +  groupedJoints[i][3] + ")";
							}
						break;
					}
				} else {
					switch (ins[i]){
						case "1":
							finalInputs[i] = ", " + groupedJoints[i][0];
						break;
						case "2":
							if (leftJoints[lastInputs[i]].connections != null){
								finalInputs[i] = ", " + groupedJoints[i][2];
							} else {
								finalInputs[i] = ", " + "half2(" + groupedJoints[i][0] + ", " +  groupedJoints[i][1] + ")";
							}
						break;
						case "3":
							if (leftJoints[lastInputs[i]].connections != null){
								finalInputs[i] = ", " + groupedJoints[i][3];
							} else {
								finalInputs[i] = ", " + "half3(" + groupedJoints[i][0] + ", " +  groupedJoints[i][1] + ", " +  groupedJoints[i][2] + ")";
							}
						break;
						case "4":
							if (leftJoints[lastInputs[i]].connections != null){
								finalInputs[i] = ", " + groupedJoints[i][4];
							} else {
								finalInputs[i] = ", " + "half4(" + groupedJoints[i][0] + ", " +  groupedJoints[i][1] + ", " +  groupedJoints[i][2] + ", " +  groupedJoints[i][3] + ")";
							}
						break;
					}
				}
			}
			string function = me.Values[1] + "(";
			for (int i = 0; i < ins.Count; ++i){
				function += finalInputs[i];
			}
			function += ")";
			switch (me.name.Substring(6)){
				case "1":
					output.Add(function);
				break;
				case "2":
					output.Add(function + ".x");
					output.Add(function + ".y");
					output.Add(function);
				break;
				case "3":
					output.Add(function + ".x");
					output.Add(function + ".y");
					output.Add(function + ".z");
					output.Add(function);
				break;
				case "4":
					output.Add(function + ".x");
					output.Add(function + ".y");
					output.Add(function + ".z");
					output.Add(function + ".w");
					output.Add(function);
				break;
			}
			return output;
		}
		
		//=====PPS Function=====
		internal List<string> AnyIsNanCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, "Demand", 4, 1) : "0.0");
			output.Add("AnyIsNan(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> DecodeStereoCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[4].connections != null ? Demand(elements, (int) me.joints[4].connections.serial.x, (int) me.joints[4].connections.serial.y, "DecodeStereo", 4, -1) : "0.0");
			demands.Add(me.joints[5].connections != null ? Demand(elements, (int) me.joints[5].connections.serial.x, (int) me.joints[5].connections.serial.y, "DecodeStereo", 5, -1) : "0.0");
			demands.Add(me.joints[6].connections != null ? Demand(elements, (int) me.joints[6].connections.serial.x, (int) me.joints[6].connections.serial.y, "DecodeStereo", 6, -1) : "0.0");
			demands.Add(me.joints[7].connections != null ? Demand(elements, (int) me.joints[7].connections.serial.x, (int) me.joints[7].connections.serial.y, "DecodeStereo", 7, -1) : "0.0");
			for (int i = 0; i < 4; ++i){
				if (me.joints[8].connections != null){
					output.Add("DecodeViewNormalStereo(" + Demand(elements, (int) me.joints[8].connections.serial.x, (int) me.joints[8].connections.serial.y, "DecodeStereo", 8, 1) + GetData("DecodeStereo", i));
				} else {
					output.Add("DecodeViewNormalStereo(" + precision + "4(" + demands[0] + "," + demands[1] + "," + demands[2] + "," + demands[3] + ")" + GetData("DecodeStereo", i));
				}
			}
			return output;
		}
		
		internal List<string> FastSignCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("FastSign(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> GradientNoiseCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, "GradientNoise", 4, -1) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, "GradientNoise", 5, -1) : "0.0");
			if (me.joints[3].connections != null){
				output.Add("GradientNoise(" + Demand(elements, (int) me.joints[3].connections.serial.x, (int) me.joints[3].connections.serial.y, "GradientNoise", 3, 1) + ")");
			} else {
				output.Add("GradientNoise(" + precision + "2(" + demands[0] + "," + demands[1] + "))");
			}
			return output;
		}
		
		internal List<string> IsNanCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("IsNan(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> Linear01DepthCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("Linear01Depth(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> LinearEyeDepthCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("LinearEyeDepth(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> Max3Com(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[3].connections != null ? Demand(elements, (int) me.joints[3].connections.serial.x, (int) me.joints[3].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("Max3(" + demands[0] + "," + demands[1] + "," + demands[2] + ")");
			return output;
		}
		
		internal List<string> Min3Com(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[3].connections != null ? Demand(elements, (int) me.joints[3].connections.serial.x, (int) me.joints[3].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("Min3(" + demands[0] + "," + demands[1] + "," + demands[2] + ")");
			return output;
		}
		
		internal List<string> PositivePowCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			demands.Add(me.joints[2].connections != null ? Demand(elements, (int) me.joints[2].connections.serial.x, (int) me.joints[2].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("PositivePow(" + demands[0] + "," + demands[1] + ")");
			return output;
		}
		
		internal List<string> RcpCom(List<VisualElement> elements, int element, string boss, int destination, int mode){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[1].connections != null ? Demand(elements, (int) me.joints[1].connections.serial.x, (int) me.joints[1].connections.serial.y, boss, destination, mode) : "0.0");
			output.Add("rcp(" + demands[0] + ")");
			return output;
		}
		
		internal List<string> SafeHDRCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[5].connections != null ? Demand(elements, (int) me.joints[5].connections.serial.x, (int) me.joints[5].connections.serial.y, "SafeHDR", 5, -1) : "0.0");
			demands.Add(me.joints[6].connections != null ? Demand(elements, (int) me.joints[6].connections.serial.x, (int) me.joints[6].connections.serial.y, "SafeHDR", 6, -1) : "0.0");
			demands.Add(me.joints[7].connections != null ? Demand(elements, (int) me.joints[7].connections.serial.x, (int) me.joints[7].connections.serial.y, "SafeHDR", 7, -1) : "0.0");
			demands.Add(me.joints[8].connections != null ? Demand(elements, (int) me.joints[8].connections.serial.x, (int) me.joints[8].connections.serial.y, "SafeHDR", 8, -1) : "0.0");
			for (int i = 0; i < 5; ++i){
				if (me.joints[9].connections != null){
					output.Add("SafeHDR(" + Demand(elements, (int) me.joints[6].connections.serial.x, (int) me.joints[6].connections.serial.y, "SafeHDR", 6, 1) + GetData("SafeHDR", i));
				} else {
					output.Add("SafeHDR(" + precision + "4(" + demands[0] + "," + demands[1] + "," + demands[2] + "," + demands[3] + ")" + GetData("SafeHDR", i));
				}
			}
			return output;
		}
		
		internal List<string> TriangleVertToUVCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			List<string> demands = new List<string>();
			demands.Add(me.joints[3].connections != null ? Demand(elements, (int) me.joints[3].connections.serial.x, (int) me.joints[3].connections.serial.y, "TriangleVertToUV", 3, -1) : "0.0");
			demands.Add(me.joints[4].connections != null ? Demand(elements, (int) me.joints[4].connections.serial.x, (int) me.joints[4].connections.serial.y, "TriangleVertToUV", 4, -1) : "0.0");
			for (int i = 0; i < 3; ++i){
				if (me.joints[5].connections != null){
					output.Add("TransformTriangleVertexToUV(" + Demand(elements, (int) me.joints[5].connections.serial.x, (int) me.joints[5].connections.serial.y, "TriangleVertToUV", 5, 1) + GetData("TriangleVertToUV", i));
				} else {
					output.Add("TransformTriangleVertexToUV(" + precision + "2(" + demands[0] + "," + demands[1] + ")" + GetData("TriangleVertToUV", i));
				}
			}
			return output;
		}
		
		//=====Predefined Macro=====
		internal List<string> MacroCom(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			switch (me.name){
				case "Checker":
					output.Add(me.Values[1]);
				break;
				case "NearClipValue":
					output.Add("UNITY_NEAR_CLIP_VALUE");
				break;
				case "StartsAtTop":
					output.Add("UNITY_UV_STARTS_AT_TOP");
				break;
				case "Target":
					output.Add("SHADER_TARGET");
				break;
				case "Version":
					output.Add("UNITY_VERSION");
				break;
			}
			return output;
		}
		
		internal List<string> MacroCom2(List<VisualElement> elements, int element){
			List<string> output = new List<string>();
			VisualElement me = elements[element];
			output.Add("#if (" + me.options[Int32.Parse(me.Values[0])] + ")");
			output.Add("\tbool " + me.Values[1] + " = true;");
			output.Add("#else");
			output.Add("\tbool " + me.Values[1] + " = false;");
			output.Add("#endif");
			return output;
		}
		
		//==========
		
		internal string GetData(string nodeName, int jointNumber){
			switch (nodeName){
				case "CameraOutput":
				switch (jointNumber){
					case 0:
					return "CameraOutput.r = ";
					case 1:
					return "CameraOutput.g = ";
					case 2:
					return "CameraOutput.b = ";
					case 3:
					return "CameraOutput.rgb = ";
					default:
					return "0.0";
				}
				case "CameraInput":
				switch (jointNumber){
					case 0:
					return ").r";
					case 1:
					return ").g";
					case 2:
					return ").b";
					case 3:
					return ").rgb";
					default:
					return "0.0";
				}
				case "DecodeStereo":
				switch (jointNumber){
					case 0:
					return ").x";
					case 1:
					return ").y";
					case 2:
					return ").z";
					case 3:
					return ")";
					default:
					return "0.0";
				}
				case "TriangleVertToUV":
				switch (jointNumber){
					case 0:
					return ").x";
					case 1:
					return ").y";
					case 2:
					return ")";
					default:
					return "0.0";
				}
				case "SafeHDR":
				switch (jointNumber){
					case 0:
					return ").r";
					case 1:
					return ").g";
					case 2:
					return ").b";
					case 3:
					return ").a";
					case 4:
					return ")";
					default:
					return "0.0";
				}
				case "TextureProperty":
				switch (jointNumber){
					case 0:
					return ")";
					case 1:
					return ").r";
					case 2:
					return ").g";
					case 3:
					return ").b";
					case 4:
					return ").a";
					default:
					return "0.0";
				}
				case "DefaultUV":
				switch (jointNumber){
					case 0:
					return "i.texcoord.x";
					case 1:
					return "i.texcoord.y";
					case 2:
					return "i.texcoord";
					default:
					return "0.0";
				}
				case "StereoUV":
				switch (jointNumber){
					case 0:
					return "i.texcoordStereo.x";
					case 1:
					return "i.texcoordStereo.y";
					case 2:
					return "i.texcoordStereo";
					default:
					return "0.0";
				}
				case "Variable1":
				switch (jointNumber){
					case 1:
					return "";
					default:
					return "0.0";
				}
				case "Variable2":
				switch (jointNumber){
					case 3:
					return ".x";
					case 4:
					return ".y";
					case 5:
					return "";
					default:
					return "0.0";
				}
				case "Variable3":
				switch (jointNumber){
					case 4:
					return ".x";
					case 5:
					return ".y";
					case 6:
					return ".z";
					case 7:
					return "";
					default:
					return "0.0";
				}
				case "Variable4":
				switch (jointNumber){
					case 5:
					return ".x";
					case 6:
					return ".y";
					case 7:
					return ".z";
					case 8:
					return ".w";
					case 9:
					return "";
					default:
					return "0.0";
				}
				default:
				return "0.0";
			}
		}
		
		internal int GetSize(string nodeName, int jointNumber){
			switch (nodeName){
				case "Demand":
				switch (jointNumber){
					case 2:
					return 2;
					case 3:
					return 3;
					case 4:
					return 4;
					default:
					return 1;
				}
				case "TriangleVertToUV":
				switch (jointNumber){
					case 2:
					return 2;
					case 5:
					return 2;
					default:
					return 1;
				}
				case "DecodeStereo":
				switch (jointNumber){
					case 3:
					return 3;
					case 8:
					return 4;
					default:
					return 1;
				}
				case "GradientNoise":
				switch (jointNumber){
					case 3:
					return 2;
					default:
					return 1;
				}
				case "SafeHDR":
				switch (jointNumber){
					case 4:
					return 4;
					case 9:
					return 4;
					default:
					return 1;
				}
				case "Input2":
				switch (jointNumber){
					case 2:
					return 2;
					default:
					return 1;
				}
				case "Input3":
				switch (jointNumber){
					case 3:
					return 3;
					default:
					return 1;
				}
				case "Input4":
				switch (jointNumber){
					case 4:
					return 4;
					default:
					return 1;
				}
				case "Output2":
				return 2;
				case "Output3":
				return 3;
				case "Output4":
				return 4;
				case "CameraOutput":
				switch (jointNumber){
					case 3:
					return 3;
					default:
					return 1;
				}
				case "CameraInput":
				switch (jointNumber){
					case 3:
					return 3;
					case 6:
					return 2;
					default:
					return 1;
				}
				case "SpecialTex":
				switch (jointNumber){
					case 4:
					return 4;
					case 7:
					return 2;
					default:
					return 1;
				}
				case "TexelSize":
				switch (jointNumber){
					case 4:
					return 4;
					default:
					return 1;
				}
				case "WorldPosition":
				switch (jointNumber){
					case 3:
					return 3;
					default:
					return 1;
				}
				case "CameraDepth":
				switch (jointNumber){
					case 3:
					return 2;
					default:
					return 1;
				}
				case "_Color":
				switch (jointNumber){
					case 0:
					return 4;
					default:
					return 1;
				}
				case "_Vector2":
				switch (jointNumber){
					case 0:
					return 2;
					default:
					return 1;
				}
				case "_Vector3":
				switch (jointNumber){
					case 0:
					return 3;
					default:
					return 1;
				}
				case "_Vector4":
				switch (jointNumber){
					case 0:
					return 4;
					default:
					return 1;
				}
				case "_Texture":
				switch (jointNumber){
					case 0:
					return 4;
					case 7:
					return 2;
					default:
					return 1;
				}
				case "DefaultUV":
				switch (jointNumber){
					case 2:
					return 2;
					default:
					return 1;
				}
				case "StereoUV":
				switch (jointNumber){
					case 2:
					return 2;
					default:
					return 1;
				}
				case "WorldSpace":
				switch (jointNumber){
					case 3:
					return 3;
					default:
					return 1;
				}
				case "Projection":
				switch (jointNumber){
					case 4:
					return 4;
					default:
					return 1;
				}
				case "Luminance":
				switch (jointNumber){
					case 4:
					return 4;
					default:
					return 1;
				}
				case "DeltaTime":
				switch (jointNumber){
					case 4:
					return 4;
					default:
					return 1;
				}
				case "Ortho":
				switch (jointNumber){
					case 4:
					return 4;
					default:
					return 1;
				}
				case "ZBuffer":
				switch (jointNumber){
					case 4:
					return 4;
					default:
					return 1;
				}
				case "Screen":
				switch (jointNumber){
					case 4:
					return 4;
					default:
					return 1;
				}
				case "Time":
				switch (jointNumber){
					case 4:
					return 4;
					default:
					return 1;
				}
				case "SinTime":
				switch (jointNumber){
					case 4:
					return 4;
					default:
					return 1;
				}
				case "CosTime":
				switch (jointNumber){
					case 4:
					return 4;
					default:
					return 1;
				}
				case "Cross":
				return 3;
				case "Value2":
				switch (jointNumber){
					case 2:
					return 2;
					default:
					return 1;
				}
				case "Value3":
				switch (jointNumber){
					case 3:
					return 3;
					default:
					return 1;
				}
				case "Value4":
				switch (jointNumber){
					case 4:
					return 4;
					default:
					return 1;
				}
				case "Variable2":
				switch (jointNumber){
					case 2:
					return 2;
					case 5:
					return 2;
					default:
					return 1;
				}
				case "Variable3":
				switch (jointNumber){
					case 3:
					return 3;
					case 7:
					return 3;
					default:
					return 1;
				}
				case "Variable4":
				switch (jointNumber){
					case 4:
					return 4;
					case 9:
					return 4;
					default:
					return 1;
				}
				case "VarLoop2":
				switch (jointNumber){
					case 2:
					return 2;
					case 3:
					return 2;
					default:
					return 1;
				}
				case "VarLoop3":
				switch (jointNumber){
					case 3:
					return 3;
					case 4:
					return 3;
					default:
					return 1;
				}
				case "VarLoop4":
				switch (jointNumber){
					case 4:
					return 4;
					case 5:
					return 4;
					default:
					return 1;
				}
				case "Custom2":
				switch (jointNumber){
					case 2:
					return 2;
					default:
					return 1;
				}
				case "Custom3":
				switch (jointNumber){
					case 3:
					return 3;
					default:
					return 1;
				}
				case "Custom4":
				switch (jointNumber){
					case 4:
					return 4;
					default:
					return 1;
				}
				default:
				return 1;
			}
		}
	}
}
