using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Channels/Event/GravityBodyEventChannel", fileName = "NewGravityBodyEventChannel")]
public class GravityBodyEventChannelSO : DescriptionBaseSO
{
    public Action<GravityBody> OnRaised;

    public void Raise(GravityBody val)
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
