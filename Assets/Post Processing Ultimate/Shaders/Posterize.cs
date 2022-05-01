using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(PosterizeRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/Posterize")]
	public sealed class Posterize : PostProcessEffectSettings
	{
		[Tooltip("Number of colors in each channel")]
		public IntParameter Colors = new IntParameter {value = 8};
	}

	public sealed class PosterizeRenderer : PostProcessEffectRenderer<Posterize>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/Posterize"));
			sheet.properties.SetFloat("_Colors", settings.Colors);
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		}
	}
}