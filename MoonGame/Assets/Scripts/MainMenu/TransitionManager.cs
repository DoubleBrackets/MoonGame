using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    [ColorHeader("Dependencies")] 
    [SerializeField] private Image image;

    [ColorHeader("Config", ColorHeaderColor.Config)] 
    [SerializeField] private float transitionTime;

    public Coroutine TransitionOut()
    {
        return StartCoroutine(CoroutTransitionOut());
    }
    
    private IEnumerator CoroutTransitionOut()
    {
        var endOfFrame = new WaitForEndOfFrame();
        float t = 0f;
        while (t < transitionTime)
        {
            t += Time.deltaTime;
            SetAlpha(t / transitionTime);
            yield return endOfFrame;
        }
        SetAlpha(1);
    }
    
    public Coroutine TransitionIn()
    {
        return StartCoroutine(CoroutTransitionIn());
    }
    
    private IEnumerator CoroutTransitionIn()
    {
        var endOfFrame = new WaitForEndOfFrame();
        float t = 0f;
        while (t < transitionTime)
        {
            t += Time.deltaTime;
            SetAlpha(1 - t / transitionTime);
            yield return endOfFrame;
        }
        SetAlpha(0);
    }

    private void SetAlpha(float a)
    {
        Color c = image.color;
        c.a = a;
        image.color = c;
    }
}
