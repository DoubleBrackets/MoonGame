using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
    [ColorHeader("Invoking - Ask Change Scene", ColorHeaderColor.TriggeringEvents)] 
    [SerializeField] private SceneEventChannelSO askChangeScene;
    
    [ColorHeader("Config", ColorHeaderColor.Config)] 
    [SerializeField] private SceneAsset scene;
    
    public void TriggerSceneChange()
    {
        askChangeScene.Raise(scene);
    }
}
