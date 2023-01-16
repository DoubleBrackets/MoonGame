using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [ColorHeader("Listening - Load Scene Ask Channel", ColorHeaderColor.ListeningEvents)]
    [SerializeField] private StringEventChannelSO askLoadScene;
    
    [ColorHeader("Dependencies")]
    [SerializeField] private TransitionManager transitionManager;

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        askLoadScene.OnRaised += LoadNewScene;
    }

    private void OnDisable()
    {
        askLoadScene.OnRaised -= LoadNewScene;
    }

    private void LoadNewScene(String scene)
    {
        StartCoroutine(CoroutLoadScene(scene));
    }

    private IEnumerator CoroutLoadScene(String scene)
    {
        yield return transitionManager.TransitionOut();
        
        yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        
        yield return transitionManager.TransitionIn();
    }
}
