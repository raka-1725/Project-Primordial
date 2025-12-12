using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering.RendererUtils;

public class SeeThroughPass : CustomPass
{
    public LayerMask xrayLayer;
    public Material xrayMaterial;

    protected override void Execute(CustomPassContext context)
    {
        var rld = new RendererListDesc(
            new ShaderTagId[] {
                new ShaderTagId("ForwardOnly"),
                new ShaderTagId("SRPDefaultUnlit")
            },
            context.cullingResults,
            context.hdCamera.camera)
        {
            layerMask = xrayLayer,
            renderQueueRange = RenderQueueRange.all,
            sortingCriteria = SortingCriteria.CommonOpaque,

            overrideMaterial = xrayMaterial,
            overrideMaterialPassIndex = 0,

            stateBlock = new RenderStateBlock(RenderStateMask.Depth)
            {
                depthState = new DepthState(false, CompareFunction.Greater)
            }
        };
        RendererList rendererList = context.renderContext.CreateRendererList(rld);

        context.cmd.DrawRendererList(rendererList);
    }
}
