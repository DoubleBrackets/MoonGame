using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Channels/Event/GravitySourceEventChannel", fileName = "NewGravitySourceEventChannel")]
public class GravitySourceEventChannelSO : DescriptionBaseSO
{
    public Action OnRaised;

    public void Raise()
    {
        if (OnRaised != null)
        {
            OnRaised.Invoke();
        }
        else
        {
            Debug.LogWarning($"Event Channel {name} was raised but had no listeners.");
        }
    }
}
