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
    [SerializeField] float m_TimeReset = 2;
    [Tooltip("The Combo Threshhold at which the music will change")]
    [SerializeField] int m_ComboThreshold = 10;
    private int m_ComboTrack = 1;

    private float m_CurrentTime; //current value decaying over time
    #endregion

    #region Audio
    [Header("Audio")]
    [SerializeField] float m_Volume = 1;
    [Tooltip("The bottom Audio Source Component. Holds the SFX for when the player hits a combo milestone")]
    [SerializeField] AudioSource m_AudioSFX;
    private int m_ChosenTrack;
    private Randomizer m_Randomizer;
    [SerializeField] RandomTrackPicker[] m_TrackList;
    private DoubleAudioSource d_AudioSource;
    #endregion

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
        if (m_CurrentCombo > 1)
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

    public void BeatCombo()
    {
        int albumRange = m_TrackList[m_ChosenTrack].Music.Length - 1; //element 0 is always filled with a break track and autoswitches to
                                                                      //element 1 on wave start up

        //increment combo meter
        m_CurrentCombo++;
        m_CurrentTime = m_TimeReset;

        //function to reset
        //every tenth kill play next track so
        if (m_CurrentCombo == m_ComboThreshold && m_ComboTrack != albumRange)
        {
            m_ComboTrack++;
            //Plays audio everytime audio source changes
            print("current section selected: " + m_ComboTrack);
            CrossFadeAudioSource(m_TrackList[m_ChosenTrack].Music[m_ComboTrack], .5f);
            m_AudioSFX.PlayOneShot(m_AudioSFX.clip);
            m_CurrentCombo = 0;
        }
    }
    
    public void PlayWaveTrack()
    {
        CrossFadeAudioSource(m_TrackList[m_ChosenTrack].Music[1], .5f);
    }

    public void SetupWaveTrack()
    {
        m_CurrentCombo = 0;
        m_ComboTrack = 1;
        m_ChosenTrack = m_Randomizer.SelectFlatDistributed();
        CrossFadeAudioSource(m_TrackList[m_ChosenTrack].Music[0], .5f);
        print("Chosen Track: " + m_ChosenTrack.ToString());
    }

    //crossfade between music when the player has higher or decreased combo
    private void CrossFadeAudioSource(AudioClip comboClip, float fadeTime)
    {
        d_AudioSource.CrossFade(comboClip, m_Volume, fadeTime);
    }
}
