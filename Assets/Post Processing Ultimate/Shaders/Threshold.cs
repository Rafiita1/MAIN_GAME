using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(ThresholdRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/Threshold")]
	public sealed class Threshold : PostProcessEffectSettings
	{
		[Range(0f, 1f), Tooltip("Effect visibility")]
		public FloatParameter Intensity = new FloatParameter {value = 0.5f};
		[Range(0f, 1f), Tooltip("Threshold level")]
		public FloatParameter Level = new FloatParameter {value = 0.5f};
	}

	public sealed class ThresholdRenderer : PostProcessEffectRenderer<Threshold>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/Threshold"));
			sheet.properties.SetFloat("_Intensity", settings.Intensity);
			sheet.properties.SetFloat("_Level", settings.Level);
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		}
	}
}