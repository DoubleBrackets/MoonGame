using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Channels/Event/IntEventChannel", fileName = "NewIntEventChannel")]
public class IntEventChannelSO : DescriptionBaseSO
{
    public Action<int> OnRaised;

    public void Raise(int val)
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
