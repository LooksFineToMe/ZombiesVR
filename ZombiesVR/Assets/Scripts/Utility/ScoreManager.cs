using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] public int m_CurrentScore;
    [SerializeField] public int m_HighScore;
    [Tooltip("Text to be displayed in the world")]
    [SerializeField] TextMeshPro m_ScoreText;
    [Tooltip("The Prefabbed gameobject that contains the TMPro Text")]
    [SerializeField] GameObject m_ScorePopUp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_ScoreText.text = "Points: " + m_CurrentScore.ToString();
    }

    /// <summary>
    /// spawns an object that displays the amount of score the player has been awarded
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="score"></param>
    public void ScorePopUp(Vector3 pos, int score)
    {
        m_ScorePopUp.GetComponentInChildren<TextMeshPro>().text = score.ToString();

        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 2.2f, transform.position.z);

        GameObject popUp = Instantiate(m_ScorePopUp, pos, Quaternion.identity);
        Destroy(popUp, 1.23f);
        Debug.Log("Pos" + pos);
        Debug.Log("Score" + score);
    }
}
