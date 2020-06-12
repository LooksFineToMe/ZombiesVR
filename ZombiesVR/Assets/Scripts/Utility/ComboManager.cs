using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    #region General
    [SerializeField] WaveManager m_WaveManager;
    [Header("Combos")]
    [Tooltip("The Players current combo, updated per zombie elimination.")]
    [SerializeField] public int m_CurrentCombo;
    [Tooltip("Low Music Intensity, change music when the player eliminates zombies consecutively")]
    [SerializeField] int m_LowIntensityCombo = 10;
    [Tooltip("Medium Music Intensity, change music when the player eliminates zombies consecutively")]
    [SerializeField] int m_MediumIntensityCombo = 20;
    [Tooltip("High Music Intensity, change music when the player eliminates zombies consecutively")]
    [SerializeField] int m_HighIntensityCombo = 30;
    [Tooltip("How long the player can maintain their combo. Current Time is reset to this value per elimination.")]
    [SerializeField] float m_TimeReset = 2;
    private float m_CurrentTime; //current value decaying over time
    private bool m_HasCombo = false;
    private bool m_ResetAudio = false;
    #endregion

    #region Audio
    [Header("Audio")]
    [Tooltip("The bottom Audio Source Component. Holds the SFX for when the player hits a combo milestone")]
    [SerializeField] AudioSource m_AudioSFX;
    [SerializeField] AudioClip[] m_TrackListZero;
    [SerializeField] AudioClip[] m_TrackListOne;
    [SerializeField] AudioClip[] m_TrackListTwo;
    #endregion

    #region Song Booleans 
    private bool m_CallWaveTrack;
    private bool m_CallBreakSong;
    private bool m_TrackZero;
    private bool m_TrackOne;
    private bool m_TrackTwo;

    private bool m_CalledZero;
    private bool m_CalledOne;
    private bool m_CalledTwo;
    private bool m_CalledThree;
    private bool m_CalledFour;
    #endregion

    private DoubleAudioSource d_AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentCombo = 0;
        d_AudioSource = GetComponent<DoubleAudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CurrentCombo > 2)
        {
            if (m_CurrentTime > 0)
            {
                m_CurrentTime -= Time.deltaTime;
                if (m_CurrentTime <= 0)
                {
                    m_CurrentTime = 0;
                    m_CurrentCombo = 0;
                        //we can either have this reset to the lowest intensity of the track or slowly climb down the ladder of intensity
                        //or we can have this turned off and never reset the music
                    //if (m_HasCombo)
                    //{
                    //    CrossFadeAudioSource(m_TrackListOne[1], 5f);
                    //    m_HasCombo = false;
                    //}
                }
            }
        }
    }
    
    public void AddCombo()
    {
        m_CurrentCombo += 1;
        m_CurrentTime = m_TimeReset;

        //there has to be a better method than this
        if (m_TrackZero && !m_WaveManager.m_Break)
            TrackListZero();
        else if (m_TrackOne && !m_WaveManager.m_Break)
            TrackListOne();
        else if (m_TrackTwo && !m_WaveManager.m_Break)
            TrackListTwo();
    }

    //this needs to be changed for effiency
    public void SetupTrack()
    {
        if (m_WaveManager.m_ChosenTrack == 0)
        {
            CrossFadeAudioSource(m_TrackListZero[0], 5f);
            m_TrackZero = true;
        }
        else if (m_WaveManager.m_ChosenTrack == 1)
        {
            CrossFadeAudioSource(m_TrackListOne[0], 5f);
            m_TrackOne = true;
        }
        else if (m_WaveManager.m_ChosenTrack == 2)
        {
            CrossFadeAudioSource(m_TrackListTwo[0], 5f);
            m_TrackTwo = true;
        }
    }

    //really shouldn't keep doing this method, it will get messy
    public void PlayWaveTrack()
    {
        if (m_TrackZero)
        {
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip); //place holder
            CrossFadeAudioSource(m_TrackListZero[1], .8f);
        }
        else if (m_TrackOne)
        {
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip); //place holder
            CrossFadeAudioSource(m_TrackListOne[1], .8f);
        }
        else if (m_TrackTwo)
        {
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip); //place holder
            CrossFadeAudioSource(m_TrackListTwo[1], .8f);
        }
    }

    private void TrackListZero()
    {
        //ignore the first track as it will play when the wave begins
        if (m_CurrentCombo == 10 && !m_CalledZero) //medium
        {
            CrossFadeAudioSource(m_TrackListZero[2], .5f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
            m_CalledZero = true;
        }
        else if (m_CurrentCombo == 20 && !m_CalledOne) //high
        {
            CrossFadeAudioSource(m_TrackListZero[3], .5f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
            m_CalledOne = true;
        }
        else if (m_CurrentCombo == 30 && !m_CalledTwo)
        {
            CrossFadeAudioSource(m_TrackListZero[4], .5f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
            m_CalledTwo = true;
        }
    }

    private void TrackListOne()
    {
        //ignore the first track as it will play when the wave begins
        if (m_CurrentCombo == 10 && !m_CalledZero)
        {
            CrossFadeAudioSource(m_TrackListOne[2], .5f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
            m_CalledZero = true;
        }
        else if (m_CurrentCombo == 20 && !m_CalledOne)
        {
            CrossFadeAudioSource(m_TrackListOne[3], .5f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
            m_CalledOne = true;
        }
        else if (m_CurrentCombo == 30 && !m_CalledTwo)
        {
            CrossFadeAudioSource(m_TrackListOne[4], .5f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
            m_CalledTwo = true;
        }
    }

    private void TrackListTwo()
    {
        //ignore the first track as it will play when the wave begins
        if (m_CurrentCombo == m_LowIntensityCombo && !m_CalledZero)
        {
            CrossFadeAudioSource(m_TrackListTwo[2], .5f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
            m_CalledZero = true;
        }
        else if (m_CurrentCombo == m_MediumIntensityCombo && !m_CalledOne)
        {
            CrossFadeAudioSource(m_TrackListTwo[3], .5f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
            m_CalledOne = true;
        }
        else if (m_CurrentCombo == m_HighIntensityCombo && !m_CalledTwo)
        {
            CrossFadeAudioSource(m_TrackListTwo[4], .5f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
            m_CalledTwo = true;
        }
    }

    public void ResetCalls()
    {
        //track bools
        m_TrackZero = false;
        m_TrackOne = false;
        m_TrackTwo = false;

        //Section bools
        m_CalledZero = false;
        m_CalledOne = false;
        m_CalledTwo = false;
        m_CalledThree = false;
        m_CalledFour = false;
    }
    
    //crossfade between music when the player has higher or decreased combo
    private void CrossFadeAudioSource(AudioClip comboClip, float fadeTime)
    {
        d_AudioSource.CrossFade(comboClip, 1, fadeTime);
    }
}
