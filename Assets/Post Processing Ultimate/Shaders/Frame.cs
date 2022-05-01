using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(FrameRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/Frame")]
	public sealed class Frame : PostProcessEffectSettings
	{
		[Range(0f, 1f), Tooltip("Horizontal frame")]
		public FloatParameter Horizontal = new FloatParameter {value = 0.5f};
		[Range(0f, 1f), Tooltip("Vertical frame")]
		public FloatParameter Vertical = new FloatParameter {value = 0.5f};
	}

	public sealed class FrameRenderer : PostProcessEffectRenderer<Frame>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/Frame"));
			sheet.properties.SetFloat("_Horizontal", settings.Horizontal);
			sheet.properties.SetFloat("_Vertical", settings.Vertical);
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		}
	}
}