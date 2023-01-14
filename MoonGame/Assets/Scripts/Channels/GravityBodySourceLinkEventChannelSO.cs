using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Channels/Event/AddGravityBodySourceLinkEventChannel", fileName = "NewAddGravityBodyEventChannel")]
public class GravityBodySourceLinkEventChannelSO : DescriptionBaseSO
{
    public Action<GravityBody, GravitySource> OnRaised;

    public void Raise(GravityBody body, GravitySource source)
    {
        if (OnRaised != null)
        {
            OnRaised.Invoke(body, source);
        }
        else
        {
            Debug.LogWarning($"Event Channel {name} was raised but had no listeners.");
        }
    }
}
