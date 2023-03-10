using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Channels/Func/IntFuncChannel", fileName = "NewIntFuncChannel")]
public class IntFuncChannelSO : DescriptionBaseSO
{
    public Func<int> OnCalled;

    public int Call()
    {
        if (OnCalled != null)
        {
            return OnCalled.Invoke();
        }
        else
        {
            Debug.LogWarning($"Func Channel {name} was called but had no receivers");
            return 0;
        }
    }
}
