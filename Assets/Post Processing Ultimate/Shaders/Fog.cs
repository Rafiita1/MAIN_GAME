using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(FogRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/Fog")]
	public sealed class Fog : PostProcessEffectSettings
	{
		[Tooltip("Fog color")]
		public ColorParameter Color = new ColorParameter {value = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.7843137f)};
		[Range(0f, 1f), Tooltip("Fog visibility")]
		public FloatParameter Intensity = new FloatParameter {value = 0.5f};
		[Range(0f, 1f), Tooltip("Distance at which fog should appear")]
		public FloatParameter Distance = new FloatParameter {value = 1f};
		[Tooltip("Fog height")]
		public FloatParameter Height = new FloatParameter {value = 3f};
	}

	public sealed class FogRenderer : PostProcessEffectRenderer<Fog>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/Fog"));
			Matrix4x4 projMat = GL.GetGPUProjectionMatrix(context.camera.projectionMatrix, false);
			projMat[15] = projMat[14] = projMat[11] = 0;
			++projMat[15];
			sheet.properties.SetMatrix("_ProjMat", Matrix4x4.Inverse(projMat * context.camera.worldToCameraMatrix) * Matrix4x4.TRS(new Vector3(0, 0, -1 * projMat[10]), Quaternion.identity, Vector3.one));
			sheet.properties.SetColor("_Color", settings.Color);
			sheet.properties.SetFloat("_Intensity", settings.Intensity);
			sheet.properties.SetFloat("_Distance", settings.Distance);
			sheet.properties.SetFloat("_Height", settings.Height);
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		}
	}
}