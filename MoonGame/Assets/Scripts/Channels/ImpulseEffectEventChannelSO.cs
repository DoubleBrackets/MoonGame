using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Channels/Event/ImpulseEffectEventChannel", fileName = "NewImpulseEffectEventChannel")]
public class ImpulseEffectEventChannelSO : DescriptionBaseSO
{
    public Action<CameraImpulseManager.ImpulseEffect, float> OnRaised;

    public void Raise(CameraImpulseManager.ImpulseEffect effect, float force = 1)
    {
        if (OnRaised != null)
        {
            OnRaised.Invoke(effect, force);
        }
        else
        {
            Debug.LogWarning($"Event Channel {name} was raised but had no listeners.");
        }
    }
}
