using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(OverlayRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/Overlay")]
	public sealed class Overlay : PostProcessEffectSettings
	{
		[Range(0f, 1f), Tooltip("Effect intensity")]
		public FloatParameter Intensity = new FloatParameter {value = 0.5f};
		[Tooltip("Overlay texture")]
		public TextureParameter Image = new TextureParameter {value = null};
	}

	public sealed class OverlayRenderer : PostProcessEffectRenderer<Overlay>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/Overlay"));
			sheet.properties.SetFloat("_Intensity", settings.Intensity);
			if (settings.Image.value != null) { sheet.properties.SetTexture("_Image", settings.Image); }
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		}
	}
}