using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GG.Infrastructure.Utils;
    
public class ComboManager : MonoBehaviour
{
    [SerializeField] WaveManager m_WaveManager;

    #region Combo
    [Header("Combos")]
    [Tooltip("The Players current combo, updated per zombie elimination.")]
    [SerializeField] public int m_CurrentCombo;
    [Tooltip("Low Music Intensity, change music when the player eliminates zombies consecutively")]
    [SerializeField] int m_LowIntensityCombo = 10;
    [Tooltip("Medium Music Intensity, change music when the player eliminates zombies consecutively")]
    [SerializeField] int m_MediumIntensityCombo = 20;
    [Tooltip("High Music Intensity, change music when the player eliminates zombies consecutively")]
    [SerializeField] int m_HighIntensityCombo = 30;
    [Tooltip("Higher Music Intensity, change music when the player eliminates zombies consecutively")]
    [SerializeField] int m_HigherIntensityCombo = 40;
    [Tooltip("How long the player can maintain their combo. Current Time is reset to this value per elimination.")]
    [SerializeField] float m_TimeReset = 2;
    private float m_CurrentTime; //current value decaying over time
    private bool m_HasCombo = false;
    private bool m_ResetAudio = false;
    #endregion

    #region Audio
    [Header("Audio")]
    [SerializeField] float m_Volume = 1;
    [Tooltip("[THIS EXCLUDES THE MAXIMUM VALUE] The number of tracks we have to randomly pick from.")]
    [SerializeField] int m_AmountOfTracks = 4;
    [Tooltip("The bottom Audio Source Component. Holds the SFX for when the player hits a combo milestone")]
    [SerializeField] AudioSource m_AudioSFX;
    [SerializeField] AudioClip[] m_TrackListZero;
    [SerializeField] AudioClip[] m_TrackListOne;
    [SerializeField] AudioClip[] m_TrackListTwo;
    [SerializeField] AudioClip[] m_TrackListThree;
    private int m_ChosenTrack;

    private int m_ComboIncrement;

    private Randomizer m_Randomizer;

    [SerializeField] RandomTrackPicker[] m_TrackList;
    #endregion

    #region Song Booleans 
    private bool m_CallWaveTrack;
    private bool m_CallBreakSong;
    private bool m_TrackZero;
    private bool m_TrackOne;
    private bool m_TrackTwo;
    private bool m_TrackThree;

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

        m_Randomizer = new Randomizer(m_TrackList.Length);
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
    
    //combo points are added when the player eliminates a zombie
    public void AddCombo()
    {
        m_CurrentCombo += 1;
        BeatCombo();
        m_CurrentTime = m_TimeReset;



        //there has to be a better method than this
        //if (m_TrackZero && !m_WaveManager.m_Break)
        //    TrackListZero();
        //else if (m_TrackOne && !m_WaveManager.m_Break)
        //    TrackListOne();
        //else if (m_TrackTwo && !m_WaveManager.m_Break)
        //    TrackListTwo();
        //else if (m_TrackThree && !m_WaveManager.m_Break)
        //    TrackListThree();
    }

    public void BeatCombo()
    {
        int albumRange = m_TrackList[m_ChosenTrack].Music.Length;
        //m_CurrentCombo++; Adds 2 combo per elimination, returns index out of range error
        print(m_CurrentCombo); //always returning 0

        #region Discription
        // Could be 0+1 to 6 or 0+1 to 3
        // If m_CurrentCombo can be divided with no remainder of the ComboIncrement
        // And the m_CurrentCombo is not 0 then combo is dividable by the ComboIncrement
        // then increment the CurrentCombo as the combo will be between the albumRange
        // the albumRange is from 0 through to the maximum in the Album
        // CurrentCombo adds 1 to the track combo every ComboIncrement
        // CurrentCombo can never be more then the albumRange
        // hench the math.Max capping the maximum at the albumRange
        // Finally playing the CrossFade for the CurrentCombo track
        // CurrentCombo is the track to play from the music Index of the array
        #endregion
        if (m_CurrentCombo % m_ComboIncrement == 0 && m_CurrentCombo != 0) //returning error "attempting to devide by zero"
        {
            print("combo");
            m_CurrentCombo++;
            m_CurrentCombo = Math.Max(m_CurrentCombo, albumRange);
            CrossFadeAudioSource(m_TrackList[m_ChosenTrack].Music[m_CurrentCombo], .5f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
        }
    }

    #region In-Game Tracks
    //this needs to be changed for effiency || also need to add a system so it doesn't choose the same track twice
    public void SetupTrack()
    {
        //if (m_ChosenTrack == 0)
        //{
        //    CrossFadeAudioSource(m_TrackListZero[0], 5f);
        //    m_TrackZero = true;
        //}
        //else if (m_ChosenTrack == 1)
        //{
        //    CrossFadeAudioSource(m_TrackListOne[0], 5f);
        //    m_TrackOne = true;
        //}
        //else if (m_ChosenTrack == 2)
        //{
        //    CrossFadeAudioSource(m_TrackListTwo[0], 5f);
        //    m_TrackTwo = true;
        //}
        //else if (m_ChosenTrack == 3)
        //{
        //    CrossFadeAudioSource(m_TrackListThree[0], 5f);
        //    m_TrackThree = true;
        //}
    }

    //really shouldn't be doing this method, it gets messy || I hope there's a better way to do this, too many steps to add a new track
    public void PlayWaveTrack()
    {
        CrossFadeAudioSource(m_TrackList[m_ChosenTrack].Music[1], .5f);
        //if (m_TrackZero)
        //{
        //    CrossFadeAudioSource(m_TrackListZero[1], .8f);
        //}
        //else if (m_TrackOne)
        //{
        //    CrossFadeAudioSource(m_TrackListOne[1], .8f);
        //}
        //else if (m_TrackTwo)
        //{
        //    CrossFadeAudioSource(m_TrackListTwo[1], .8f);
        //}
        //else if (m_TrackThree)
        //{
        //    CrossFadeAudioSource(m_TrackListThree[1], .8f);
        //}
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

    private void TrackListThree()
    {
        //ignore the first track as it will play when the wave begins
        if (m_CurrentCombo == m_LowIntensityCombo && !m_CalledZero)
        {
            CrossFadeAudioSource(m_TrackListThree[2], .5f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
            m_CalledZero = true;
        }
        else if (m_CurrentCombo == m_MediumIntensityCombo && !m_CalledOne)
        {
            CrossFadeAudioSource(m_TrackListThree[3], .5f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
            m_CalledOne = true;
        }
        else if (m_CurrentCombo == m_HighIntensityCombo && !m_CalledTwo)
        {
            CrossFadeAudioSource(m_TrackListThree[4], .5f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
            m_CalledTwo = true;
        }
        else if (m_CurrentCombo == m_HigherIntensityCombo && m_CalledThree)
        {
            CrossFadeAudioSource(m_TrackListThree[5], .5f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
            m_CalledThree = true;
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
    #endregion

    public void PickBreakTrack()
    {
        m_ChosenTrack = m_Randomizer.SelectFlatDistributed();
        CrossFadeAudioSource(m_TrackList[m_ChosenTrack].Music[0], .5f);
        //ResetCalls();
        //SetupTrack();
        print("Chosen Track: " + m_ChosenTrack.ToString());
    }

    //crossfade between music when the player has higher or decreased combo
    private void CrossFadeAudioSource(AudioClip comboClip, float fadeTime)
    {
        d_AudioSource.CrossFade(comboClip, m_Volume, fadeTime);
    }
}
