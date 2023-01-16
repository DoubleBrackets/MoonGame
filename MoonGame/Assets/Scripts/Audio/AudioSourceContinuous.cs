using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceContinuous : MonoBehaviour
{
    [ColorHeader("Listening - Play/Stop Audio Ask Channel", ColorHeaderColor.ListeningEvents)]
    [SerializeField] private VoidEventChannelSO askPlayAudio;
    [SerializeField] private VoidEventChannelSO askStopAudio;

    [ColorHeader("Config")]
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;
    [SerializeField] private float stopDropoffDuration = 0.1f;
    [SerializeField] private bool playOnAwake;

    private Coroutine dropoffCorout;

    private void OnEnable()
    {
        askPlayAudio.OnRaised += PlayAudio;
        askStopAudio.OnRaised += StopAudio;

        if (playOnAwake)
            PlayAudio();
    }

    private void OnDisable()
    {
        askPlayAudio.OnRaised -= PlayAudio;
        askStopAudio.OnRaised -= StopAudio;
    }

    
    private void PlayAudio()
    {
        StopDropoffCorout();
        if (source.isPlaying) return;
        source.loop = true;
        source.PlayOneShot(clip);
    }
    
    private void RestartAudio()
    {
        source.PlayOneShot(clip);
    }
    
    private void StopAudio()
    {
        StopDropoffCorout();
        dropoffCorout = StartCoroutine(CoroutStopAudio());
    }

    private void StopDropoffCorout()
    {
        if (dropoffCorout != null)
        {
            StopCoroutine(dropoffCorout);
            source.volume = 1f;
            dropoffCorout = null;
        }
    }

    private IEnumerator CoroutStopAudio()
    {
        float timer = 0f;
        while (timer < stopDropoffDuration)
        {
            timer += Time.deltaTime;
            source.volume = (1 - timer / stopDropoffDuration);
            yield return new WaitForEndOfFrame();
        }
        source.Stop();
        source.volume = 1f;
    }

}
