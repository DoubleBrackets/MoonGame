using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class WarpRenderFeature : ScriptableRendererFeature
{
    [SerializeField] private RenderPassEvent renderPassEvent;
    [SerializeField] private bool applyInEditor;
    [SerializeField] private WarpRenderPass.WarpRenderPassSettings settings;

    private ScriptableRenderPass pass;

    public override void Create()
    {
        // Create an instance of your custom pass here
        pass = new WarpRenderPass(settings, applyInEditor, renderPassEvent);
    }
    
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }
}