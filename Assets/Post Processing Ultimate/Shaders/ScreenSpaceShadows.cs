using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(ScreenSpaceShadowsRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/ScreenSpaceShadows")]
	public sealed class ScreenSpaceShadows : PostProcessEffectSettings
	{
		[Range(0f, 1f), Tooltip("Effect intenisty")]
		public FloatParameter Intensity = new FloatParameter {value = 0f};
		[Range(2, 16), Tooltip("Effect power")]
		public IntParameter Power = new IntParameter {value = 2};
		internal RenderTexture tempRT0;
		internal RenderTexture tempRT1;
	}

	public sealed class ScreenSpaceShadowsRenderer : PostProcessEffectRenderer<ScreenSpaceShadows>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/ScreenSpaceShadows"));
			RenderTexture.ReleaseTemporary(settings.tempRT0);
			RenderTexture.ReleaseTemporary(settings.tempRT1);
			sheet.properties.SetFloat("_Intensity", settings.Intensity);
			sheet.properties.SetFloat("_Power", settings.Power);
			settings.tempRT0 = RenderTexture.GetTemporary(Screen.width, Screen.height);
			settings.tempRT1 = RenderTexture.GetTemporary(Screen.width, Screen.height);
			context.command.BlitFullscreenTriangle(context.source, settings.tempRT0, sheet, 0);
			context.command.BlitFullscreenTriangle(settings.tempRT0, settings.tempRT1, sheet, 1);
			sheet.properties.SetTexture("_tempRT1", settings.tempRT1);
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 2);
		}
	}
}