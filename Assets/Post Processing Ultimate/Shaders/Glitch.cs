using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(GlitchRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/Glitch")]
	public sealed class Glitch : PostProcessEffectSettings
	{
		[Range(0f, 1f), Tooltip("Animation speed")]
		public FloatParameter Speed = new FloatParameter {value = 0.5f};
		[Range(0f, 1f), Tooltip("Bug size")]
		public FloatParameter Size = new FloatParameter {value = 0.3f};
	}

	public sealed class GlitchRenderer : PostProcessEffectRenderer<Glitch>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/Glitch"));
			sheet.properties.SetFloat("_Speed", settings.Speed);
			sheet.properties.SetFloat("_Size", settings.Size);
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		}
	}
}