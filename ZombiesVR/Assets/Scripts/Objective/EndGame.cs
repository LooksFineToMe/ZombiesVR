using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndGame : MonoBehaviour
{
    bool gameOver = false;
    float timer;
    string scenename;
    private void Update()
    {
        if (gameOver == true)
        {
            timer += Time.deltaTime;
        }
        if (timer > 10)
        {
            SceneManager.LoadScene(scenename);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameOver = true;
        }
    }

}
