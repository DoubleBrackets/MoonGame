using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Channels/Event/SceneEventChannel", fileName = "NewSceneEventChannel")]
public class SceneEventChannelSO : DescriptionBaseSO
{
    public Action<SceneAsset> OnRaised;

    public void Raise(SceneAsset scene)
    {
        if (OnRaised != null)
        {
            OnRaised.Invoke(scene);
        }
        else
        {
            Debug.LogWarning($"Event Channel {name} was raised but had no listeners.");
        }
    }
}
