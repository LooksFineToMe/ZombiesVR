using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour
{
    public GameObject mainMenuPannel;
    public GameObject creditsPannel;
    public Slider musicVol;
    public Slider soundVol;
    public Slider masterVol;
    public Slider gunFXVol;
    public AudioMixer music;
    public AudioMixer sound;
    private void Start()
    {
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualitySettings"));
        musicVol.value = PlayerPrefs.GetFloat("Music");
        soundVol.value = PlayerPrefs.GetFloat("Sound");
        masterVol.value = PlayerPrefs.GetFloat("Master");
        gunFXVol.value = PlayerPrefs.GetFloat("GunFX");

    }
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
    public void UISetMusicVolume(float volume)
    {
        music.SetFloat("Music", volume);
        PlayerPrefs.SetFloat("Music", volume);
    }
    public void UISetSoundEffectsVolume(float volume)
    {
        sound.SetFloat("Sound", volume);
        PlayerPrefs.SetFloat("Sound", volume);
    }
    public void UISetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualitySettings", qualityIndex);
    }
    public void SetMasterVolume(float volume)
    {
        sound.SetFloat("MasterVol", volume);
        PlayerPrefs.SetFloat("Master", volume);
    }
    public void UISetGunEffectsVolume(float volume)
    {
        sound.SetFloat("GunSoundFX", volume);
        PlayerPrefs.SetFloat("GunFX", volume);
    }
    public void UITouchPadMovement(bool touchPad)
    {
        if (touchPad == true)
        {
            PlayerPrefs.SetInt("TouchpadEnable", 1);
        }
        else if (touchPad == false)
        {
            PlayerPrefs.SetInt("TouchpadEnable", 0);
        }
    }
}
