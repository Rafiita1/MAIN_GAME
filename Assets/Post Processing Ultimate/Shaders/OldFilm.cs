using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(OldFilmRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/OldFilm")]
	public sealed class OldFilm : PostProcessEffectSettings
	{
		[Range(0f, 1f), Tooltip("Noise intensity")]
		public FloatParameter Noise = new FloatParameter {value = 0.5f};
		[Range(0f, 1f), Tooltip("Oscillation amplitude")]
		public FloatParameter Oscillation = new FloatParameter {value = 0.5f};
		internal RenderTexture tempRT0;
		internal RenderTexture tempRT1;
		internal RenderTexture tempRT2;
	}

	public sealed class OldFilmRenderer : PostProcessEffectRenderer<OldFilm>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/OldFilm"));
			RenderTexture.ReleaseTemporary(settings.tempRT0);
			RenderTexture.ReleaseTemporary(settings.tempRT1);
			RenderTexture.ReleaseTemporary(settings.tempRT2);
			sheet.properties.SetFloat("_Noise", settings.Noise);
			sheet.properties.SetFloat("_Oscillation", settings.Oscillation);
			settings.tempRT0 = RenderTexture.GetTemporary(Screen.width, Screen.height);
			settings.tempRT1 = RenderTexture.GetTemporary(Screen.width, Screen.height);
			settings.tempRT2 = RenderTexture.GetTemporary(Screen.width, Screen.height);
			context.command.BlitFullscreenTriangle(context.source, settings.tempRT0, sheet, 0);
			context.command.BlitFullscreenTriangle(settings.tempRT0, settings.tempRT1, sheet, 1);
			context.command.BlitFullscreenTriangle(settings.tempRT1, settings.tempRT2, sheet, 2);
			context.command.BlitFullscreenTriangle(settings.tempRT2, context.destination, sheet, 3);
		}
	}
}