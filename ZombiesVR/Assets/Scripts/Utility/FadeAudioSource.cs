﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FadeAudioSource
{
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration * Time.deltaTime);
            yield return null;
        }
        yield break;
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float start = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= start * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = start;
    }
}