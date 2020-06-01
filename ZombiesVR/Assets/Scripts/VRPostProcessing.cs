using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class VRPostProcessing : MonoBehaviour
{
    [Header("Damage Effects")]
    [Tooltip("The Box PP Volume attacted the player camera")]
    public Volume playerVolume;
    [Tooltip("How intense the vignette is when taking damage")]
    public float damageIntensity = .5f;
    [Tooltip("How quickly we want the damage vignette to reset")]
    public float vgReset = 1f;

    private Vignette vg;

    private void Start()
    {
        playerVolume.profile.TryGet(out vg);
    }

    private void Update()
    {
        if (vg.intensity.value >= .1)
        {
            vg.intensity.value = Mathf.Lerp(vg.intensity.value, 0, vgReset * Time.deltaTime);
        }
    }

    public void TakeDamage()
    {
        DamageEffect();
        print("player has been hit");
    }

    [ContextMenu("Damage")]
    public void DamageEffect()
    {
        vg.intensity.value = damageIntensity;
    }

    private void GameOver()
    {
        print("Game is Over");
    }
}
