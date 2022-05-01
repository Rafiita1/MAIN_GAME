using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(InfraredRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/Infrared")]
	public sealed class Infrared : PostProcessEffectSettings
	{
		[Range(0f, 1f), Tooltip("Effect intensity")]
		public FloatParameter Intensity = new FloatParameter {value = 0.3f};
	}

	public sealed class InfraredRenderer : PostProcessEffectRenderer<Infrared>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/Infrared"));
			sheet.properties.SetFloat("_Intensity", settings.Intensity);
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		}
	}
}