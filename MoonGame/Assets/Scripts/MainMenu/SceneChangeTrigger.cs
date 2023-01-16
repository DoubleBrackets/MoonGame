using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
    [ColorHeader("Invoking - Ask Change Scene", ColorHeaderColor.TriggeringEvents)] 
    [SerializeField] private StringEventChannelSO askChangeScene;

    [ColorHeader("Config", ColorHeaderColor.Config)]
    [SerializeField] private string sceneName;
    
    public void TriggerSceneChange()
    {
        askChangeScene.Raise(sceneName);
    }
}
