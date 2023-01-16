using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Channels/Event/Vector3EventChannel", fileName = "NewVector3EventChannel")]
public class Vector3EventChannelSO : DescriptionBaseSO
{
    public Action<Vector3> OnRaised;

    public void Raise(Vector3 val)
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
