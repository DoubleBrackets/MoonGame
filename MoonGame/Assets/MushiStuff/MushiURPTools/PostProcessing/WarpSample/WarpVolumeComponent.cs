using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/WarpVolumeComponent", typeof(UniversalRenderPipeline))]
public class WarpVolumeComponent : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enabled = new BoolParameter(false, true);
    public FloatParameter noiseStrength = new FloatParameter(0f, true);
    public FloatParameter noiseScale = new FloatParameter(10f);

    // Is this post process effect active?
    public bool IsActive() => enabled.value && noiseStrength.value > 0;

    public bool IsTileCompatible() => true;
}