using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(DrunkRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/Drunk")]
	public sealed class Drunk : PostProcessEffectSettings
	{
		[Tooltip("Horizontal wobble")]
		public FloatParameter Horizontal = new FloatParameter {value = 1f};
		[Tooltip("Vertical wobble")]
		public FloatParameter Vertical = new FloatParameter {value = 1f};
		[Tooltip("Animation speed")]
		public FloatParameter Speed = new FloatParameter {value = 1f};
	}

	public sealed class DrunkRenderer : PostProcessEffectRenderer<Drunk>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/Drunk"));
			sheet.properties.SetFloat("_Horizontal", settings.Horizontal);
			sheet.properties.SetFloat("_Vertical", settings.Vertical);
			sheet.properties.SetFloat("_Speed", settings.Speed);
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		}
	}
}