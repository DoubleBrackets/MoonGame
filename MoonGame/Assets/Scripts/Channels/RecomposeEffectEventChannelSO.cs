using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Channels/Event/RecomposeEffectEventChannel", fileName = "NewRecomposeEffectEventChannel")]
public class RecomposeEffectEventChannelSO : DescriptionBaseSO
{
    public Action<CameraRecomposeManager.RecomposeEffect, float> OnRaised;

    public void Raise(CameraRecomposeManager.RecomposeEffect effect, float multiplier = 1)
    {
        if (OnRaised != null)
        {
            OnRaised.Invoke(effect, multiplier);
        }
        else
        {
            Debug.LogWarning($"Event Channel {name} was raised but had no listeners.");
        }
    }
}
