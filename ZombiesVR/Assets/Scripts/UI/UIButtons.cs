using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIButtons : MonoBehaviour
{
    public void UIStart(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
    public void UIQuit()
    {
        Application.Quit();
    }
    public void UIOptions()
    { 
    
    }
}
