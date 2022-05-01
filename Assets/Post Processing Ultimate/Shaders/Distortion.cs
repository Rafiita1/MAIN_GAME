using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(DistortionRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/Distortion")]
	public sealed class Distortion : PostProcessEffectSettings
	{
		[Range(0f, 1f), Tooltip("Effect intensity")]
		public FloatParameter Intensity = new FloatParameter {value = 0.5f};
		[Tooltip("New coords")]
		public TextureParameter Coords = new TextureParameter {value = null};
	}

	public sealed class DistortionRenderer : PostProcessEffectRenderer<Distortion>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/Distortion"));
			sheet.properties.SetFloat("_Intensity", settings.Intensity);
			if (settings.Coords.value != null) { sheet.properties.SetTexture("_Coords", settings.Coords); }
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		}
	}
}