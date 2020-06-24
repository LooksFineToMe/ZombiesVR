using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("Player's Stats")]
    public int health = 5;
    public int magCount;
    public bool playerIsDead;

    [Header("Damage Effects")]
    [Tooltip("The Box PP Volume attacted the player camera")]
    public Volume playerVolume;
    [Tooltip("How intense the vignette is when taking damage")]
    public float damageIntensity;
    [Tooltip("How quickly we want the damage vignette to reset")]
    public float vgReset = 1f;
    private Vignette vg;

    [Header("timers")]
    float coolDownTimer;
    public float hitCooldown;

    public MedpackSpawner medpackSpawner;

    public string scenename = "MainMenu";
    private void Start()
    {
        playerVolume.profile.TryGet(out vg);
    }

    private void Update()
    {
        if (coolDownTimer <= hitCooldown + .2f) { coolDownTimer += Time.deltaTime; }
        if (vg.intensity.value >= damageIntensity /*playerIsDead == true*/)
        {
            vg.intensity.value = Mathf.Lerp(vg.intensity.value, 0, vgReset * Time.deltaTime);
        }
    }

    public void TakeDamage()
    {
        if (coolDownTimer > hitCooldown)
        {
            coolDownTimer = 0;
            health -= 1;

            DamageEffect();

            print("player has been hit");
            if (health <= 0)
            {
                vg.color.value = Color.black;
                vg.intensity.value = 1f;
                damageIntensity = 1f;
                playerIsDead = true;
                Invoke("GameOver", 3.0f);
            }
            if (health >= 5)
            {
                health = 5;
            }
        }
        
    }
    public void HealPlayer()
    {
        health = 5;
        medpackSpawner.SpawnMedpack();
        DamageEffect();
    }

    [ContextMenu("Damage")]
    private void DamageEffect()
    {
        if (health >= 5)
        {
            vg.intensity.value = .9f;
            damageIntensity = 0f;
        }
        if (health == 4)
        {
            vg.intensity.value = .9f;
            damageIntensity = .3f;
        }
        if (health == 3)
        {
            vg.intensity.value = .9f;
            damageIntensity = .5f;
        }
        if (health == 2)
        {
            vg.intensity.value = 1;
            damageIntensity = .7f;
        }
        if (health == 1)
        {
            vg.intensity.value = 1f;
            damageIntensity = .9f;
        }
        


    }

    private void GameOver()
    {

        SceneManager.LoadScene(scenename);
    }
}
