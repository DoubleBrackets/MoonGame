using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Channels/Event/StringEventChannel", fileName = "NewStringEventChannel")]
public class StringEventChannelSO : DescriptionBaseSO
{
    public Action<String> OnRaised;

    public void Raise(string str)
    {
        if (OnRaised != null)
        {
            OnRaised.Invoke(str);
        }
        else
        {
            Debug.LogWarning($"Event Channel {name} was raised but had no listeners.");
        }
    }
}
