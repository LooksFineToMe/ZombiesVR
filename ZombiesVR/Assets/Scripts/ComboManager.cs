using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [Header("Combos")]
    [Tooltip("The Players current combo, updated per zombie elimination.")]
    [SerializeField] public int m_CurrentCombo;
    [Tooltip("How long the player can maintain their combo. Current Time is reset to this value per elimination.")]
    [SerializeField] float m_TimeReset = 2;
    private float m_CurrentTime; //current value decaying over time
    private bool m_HasCombo = false;
    private bool m_ResetAudio = false;

    [Header("Audio")]
    [SerializeField] AudioClip[] m_TrackListOne;
    private bool called = false;

    private DoubleAudioSource d_AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        d_AudioSource = GetComponent<DoubleAudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
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

    private void ResetParameters()
    {
        m_ResetAudio = false;
    }

    public void AddCombo()
    {
        m_CurrentCombo += 1;
        m_CurrentTime = m_TimeReset;

        if (m_CurrentCombo == 2)
        {
            m_HasCombo = true;
            CrossFadeAudioSource(m_TrackListOne[2], .5f);
        }
        else if (m_CurrentCombo == 14)
        {
            CrossFadeAudioSource(m_TrackListOne[3], 1f);
        }
    }

    [ContextMenu("CrossFade")]
    private void CrossFadeAudioSource(AudioClip comboClip, float fadeTime)
    {
        d_AudioSource.CrossFade(comboClip, 1, fadeTime);
    }
}
