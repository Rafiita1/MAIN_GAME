using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(PixelizeRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/Pixelize")]
	public sealed class Pixelize : PostProcessEffectSettings
	{
		[Tooltip("Horizontal resolution")]
		public IntParameter Horizontal = new IntParameter {value = 16};
		[Tooltip("Vertical resolution")]
		public IntParameter Vertical = new IntParameter {value = 16};
	}

	public sealed class PixelizeRenderer : PostProcessEffectRenderer<Pixelize>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/Pixelize"));
			sheet.properties.SetFloat("_Horizontal", settings.Horizontal);
			sheet.properties.SetFloat("_Vertical", settings.Vertical);
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		}
	}
}