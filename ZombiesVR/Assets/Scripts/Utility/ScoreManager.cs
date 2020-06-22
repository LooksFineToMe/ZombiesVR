using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] public int m_CurrentScore;
    [SerializeField] public int m_HighScore;
    [SerializeField] TextMeshPro m_ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_ScoreText.text = "Points: " + m_CurrentScore.ToString();
    }
}
