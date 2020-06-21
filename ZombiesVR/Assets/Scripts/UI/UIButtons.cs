using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIButtons : MonoBehaviour
{
    public GameObject mainMenuPannel;
    public GameObject creditsPannel;
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
    public void UICredits()
    {
        mainMenuPannel.SetActive(false);
        creditsPannel.SetActive(true);
    }
    public void UIBackToMainMenu()
    {
        mainMenuPannel.SetActive(true);
        creditsPannel.SetActive(false);
    }
}
