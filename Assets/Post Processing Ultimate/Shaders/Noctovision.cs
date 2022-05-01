using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(NoctovisionRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/Noctovision")]
	public sealed class Noctovision : PostProcessEffectSettings
	{
		[Range(0f, 1f), Tooltip("Green intensity")]
		public FloatParameter Green = new FloatParameter {value = 0.75f};
		[Range(0f, 1f), Tooltip("Noise intenisty")]
		public FloatParameter Noise = new FloatParameter {value = 0.5f};
		internal RenderTexture tempRT0;
	}

	public sealed class NoctovisionRenderer : PostProcessEffectRenderer<Noctovision>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/Noctovision"));
			RenderTexture.ReleaseTemporary(settings.tempRT0);
			sheet.properties.SetFloat("_Green", settings.Green);
			sheet.properties.SetFloat("_Noise", settings.Noise);
			settings.tempRT0 = RenderTexture.GetTemporary(Screen.width, Screen.height);
			context.command.BlitFullscreenTriangle(context.source, settings.tempRT0, sheet, 0);
			context.command.BlitFullscreenTriangle(settings.tempRT0, context.destination, sheet, 1);
		}
	}
}