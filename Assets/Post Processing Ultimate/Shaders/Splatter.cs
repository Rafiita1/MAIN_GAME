using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(SplatterRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/Splatter")]
	public sealed class Splatter : PostProcessEffectSettings
	{
		[Range(0f, 1f), Tooltip("Effect intensity")]
		public FloatParameter Intensity = new FloatParameter {value = 0f};
		[Tooltip("Situation texture")]
		public TextureParameter Image = new TextureParameter {value = null};
	}

	public sealed class SplatterRenderer : PostProcessEffectRenderer<Splatter>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/Splatter"));
			sheet.properties.SetFloat("_Intensity", settings.Intensity);
			if (settings.Image.value != null) { sheet.properties.SetTexture("_Image", settings.Image); }
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		}
	}
}