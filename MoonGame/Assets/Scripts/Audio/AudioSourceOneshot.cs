using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioSourceOneshot : MonoBehaviour
{
    [ColorHeader("Listening - Play Audio Ask Channel", ColorHeaderColor.ListeningEvents)]
    [SerializeField] private VoidEventChannelSO askPlayAudio;

    [ColorHeader("Config")]
    [SerializeField] private List<AudioClip> clip;
    [SerializeField] private AudioSource source;
    [SerializeField] private bool avoidRepeat;

    private List<AudioClip> runtimeClips;
    
    private void OnEnable()
    {
        runtimeClips = new List<AudioClip>(clip);
        askPlayAudio.OnRaised += PlayAudio;
    }

    private void OnDisable()
    {
        askPlayAudio.OnRaised -= PlayAudio;
    }

    private void PlayAudio()
    {
        if (runtimeClips.Count == 0) return;
        source.loop = false;
        int random = Random.Range(0, runtimeClips.Count - (avoidRepeat ? 1 : 0));
        source.PlayOneShot(runtimeClips[random]);
        if (avoidRepeat)
        {
            var moveToBack = runtimeClips[random];
            runtimeClips.RemoveAt(random);
            runtimeClips.Add(moveToBack);
        }
    }
}
