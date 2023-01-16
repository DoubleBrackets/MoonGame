using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Channels/Event/GravitySourceEventChannel", fileName = "NewGravitySourceEventChannel")]
public class GravitySourceEventChannelSO : DescriptionBaseSO
{
    public Action<GravitySource>  OnRaised;

    public void Raise(GravitySource val)
    {
        if (OnRaised != null)
        {
            OnRaised.Invoke(val);
        }
        else
        {
            Debug.LogWarning($"Event Channel {name} was raised but had no listeners.");
        }
    }
}
