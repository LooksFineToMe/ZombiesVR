using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] WaveManager m_WaveManager;
    [Header("Combos")]
    [Tooltip("The Players current combo, updated per zombie elimination.")]
    [SerializeField] public int m_CurrentCombo;
    [Tooltip("How long the player can maintain their combo. Current Time is reset to this value per elimination.")]
    [SerializeField] float m_TimeReset = 2;
    private float m_CurrentTime; //current value decaying over time
    private bool m_HasCombo = false;
    private bool m_ResetAudio = false;

    [Header("Audio")]
    [SerializeField] AudioSource m_MainAudioSource;
    [SerializeField] AudioSource m_AudioSFX;
    [SerializeField] AudioClip[] m_TrackListOne;
    private bool m_Fade = true;

    private DoubleAudioSource d_AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentCombo = 0;

        m_MainAudioSource.volume = 0;
        d_AudioSource = GetComponent<DoubleAudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //for first wave start up
        if (m_Fade)
        {
            StartCoroutine(FadeAudioSource.StartFade(m_MainAudioSource, .1f, 1));
            if (m_MainAudioSource.volume >= .5f)
                m_Fade = false;
        }

        if (m_CurrentCombo > 0)
        {
            if (m_CurrentTime > 0)
            {
                m_CurrentTime -= Time.deltaTime;
                if (m_CurrentTime <= 0)
                {
                    m_CurrentTime = 0;
                    m_CurrentCombo = 0;
                    if (m_HasCombo)
                    {
                        CrossFadeAudioSource(m_TrackListOne[1], 5f);
                        m_HasCombo = false;
                    }
                }
            }
        }
    }

    //need to add break music
    //don't forget about choosing different songs for different waves
    public void AddCombo()
    {
        m_CurrentCombo += 1;
        m_CurrentTime = m_TimeReset;

        if (m_CurrentCombo == 2)
        {
            m_HasCombo = true;
            CrossFadeAudioSource(m_TrackListOne[2], .5f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
        }
        else if (m_CurrentCombo == 15)
        {
            m_TimeReset += 2f;
            CrossFadeAudioSource(m_TrackListOne[7], 1f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
        }
        else if (m_CurrentCombo == 25)
        {
            CrossFadeAudioSource(m_TrackListOne[3], 1f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
        }
    }
    
    //plays music when the player has higher or decreased combo
    private void CrossFadeAudioSource(AudioClip comboClip, float fadeTime)
    {
        d_AudioSource.CrossFade(comboClip, 1, fadeTime);
    }
}
