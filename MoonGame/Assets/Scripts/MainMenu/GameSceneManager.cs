using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [ColorHeader("Listening - Load Scene Ask Channel", ColorHeaderColor.ListeningEvents)]
    [SerializeField] private SceneEventChannelSO askLoadScene;
    
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

    private void LoadNewScene(SceneAsset scene)
    {
        StartCoroutine(CoroutLoadScene(scene));
    }

    private IEnumerator CoroutLoadScene(SceneAsset scene)
    {
        yield return transitionManager.TransitionOut();
        
        yield return SceneManager.LoadSceneAsync(scene.name, LoadSceneMode.Single);
        
        yield return transitionManager.TransitionIn();
    }
}
