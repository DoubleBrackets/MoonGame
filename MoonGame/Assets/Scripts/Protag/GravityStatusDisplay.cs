using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using TMPro;
using UnityEngine;

public class GravityStatusDisplay : MonoBehaviour
{
    [ColorHeader("Listening - Ask Display Gravity Status Channel", ColorHeaderColor.ListeningEvents)]
    [SerializeField]private StringEventChannelSO askDisplayStatus;
    
    [ColorHeader("Dependencies")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextAnimator animator;

    [ColorHeader("Config", ColorHeaderColor.Config)] 
    [SerializeField] private Color noGravColor;
    [SerializeField] private Color lowGravColor;
    [SerializeField] private Color highGravColor;

    private void OnEnable()
    {
        askDisplayStatus.OnRaised += DisplayText;
    }
    
    private void OnDisable()
    {
        askDisplayStatus.OnRaised -= DisplayText;
    }

    private void DisplayText(string str)
    {
        switch (str[0])
        {
            case 'n':
                text.color = noGravColor;
                break;
            case 'l':
                text.color = lowGravColor;
                break;
            case 'h':
                text.color = highGravColor;
                break;
            default:
                str = " " + str;
                break;
        }
        str = str.Substring(1);
        text.text = str;
        
        animator.UpdateEffects();
    }
}
