using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(SelectColorRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/SelectColor")]
	public sealed class SelectColor : PostProcessEffectSettings
	{
		[Range(0f, 1f), Tooltip("Effect intensity")]
		public FloatParameter Intensity = new FloatParameter {value = 0.7f};
		[Tooltip("Selected color")]
		public ColorParameter Selected = new ColorParameter {value = new Color(1f, 0f, 0f, 0f)};
	}

	public sealed class SelectColorRenderer : PostProcessEffectRenderer<SelectColor>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/SelectColor"));
			sheet.properties.SetFloat("_Intensity", settings.Intensity);
			sheet.properties.SetColor("_Selected", settings.Selected);
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		}
	}
}