using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAudio : MonoBehaviour
{
    [SerializeField] float fadeTime = 1;
    [SerializeField] float volume = 0.4f;
    [SerializeField] AudioClip m_ClipToPlay;

    private DoubleAudioSource d_Source;

    // Start is called before the first frame update
    void Start()
    {
        d_Source = GetComponent<DoubleAudioSource>();
        d_Source.CrossFade(m_ClipToPlay, volume, fadeTime);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
