using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A group of HierarchyIconOverlays, used to organize configs
/// </summary>
[CreateAssetMenu(menuName = "MushiTools/Hierarchy Overlays/Icons/Overlay Group")]
public class HierarchyIconOverlayGroupSO : ScriptableObject
{
    public List<HierarchyIconStringTargetSO> stringTargetIcons;
    public List<HierarchyIconMonoScriptTargetSO> monoScriptTargetIcons;
    
    public Dictionary<string, HierarchyIconStringTargetSO> stringTargetDict;
    
    private void OnValidate()
    {
        stringTargetDict = new Dictionary<string, HierarchyIconStringTargetSO>();
        foreach (var stringTarget in stringTargetIcons)
        {
            if(!stringTarget) continue;
            stringTargetDict.TryAdd(stringTarget.targetClassString.ToLower(), stringTarget);
        }
    }
}
