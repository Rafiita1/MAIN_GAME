using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace NTEC.PPU
{
	[Serializable]
	[PostProcess(typeof(EdgeDetectionRenderer), PostProcessEvent.AfterStack, "NTEC/Screen/EdgeDetection")]
	public sealed class EdgeDetection : PostProcessEffectSettings
	{
		[Range(0f, 1f), Tooltip("Edge Offset")]
		public FloatParameter Edge = new FloatParameter {value = 0.5f};
	}

	public sealed class EdgeDetectionRenderer : PostProcessEffectRenderer<EdgeDetection>
	{
		public override void Render(PostProcessRenderContext context)
		{
			var sheet = context.propertySheets.Get(Shader.Find("NTEC/Screen/EdgeDetection"));
			sheet.properties.SetFloat("_Edge", settings.Edge);
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
		}
	}
}