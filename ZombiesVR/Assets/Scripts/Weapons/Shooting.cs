using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
using System;

public class Shooting : MonoBehaviour
{
    [Header("SteamVR Inputs")]
    public SteamVR_Action_Boolean fireAction;
    public SteamVR_Action_Vibration trackPadHaptic;

    [Header("BulletSettings")]
    public GameObject bullet;
    public Transform barrelPivot;
    public float shootingSpeed = 1;
    public float fireRate = 0.5f;
    public float currentAmmo = 1;
    float timer;// For fireRate

    [Header("FeedBack")]
    public Text currentAmmoText;
    public AudioSource gunClick;
    public ParticleSystem muzzleflash;

    [Header("BulletEffects")]
    private Interactable interactable;//This script is needed to figure out the hand that is holding the gun
    [SerializeField] float nextTimeToFire = 0f;

    //public Animator animator;
    private void Start()
    {
        gunClick = GetComponent<AudioSource>();
        interactable = GetComponent<Interactable>();
        Reloading();
    }
    // Update is called once per frame
    void Update()
    {
        nextTimeToFire += Time.deltaTime;
        if (interactable.attachedToHand != null)
        {
            SteamVR_Input_Sources source = interactable.attachedToHand.handType;//Checks what hand the gun is in
            if (fireAction[source].state && nextTimeToFire >= fireRate && currentAmmo > 0)
            {
                print("implement ammo");
                //currentAmmo--;
                nextTimeToFire = 0;
                Fire();
                Pulse(0.1f, 150, 75, source);//This Passes through the values for controller vibration
            }
            else if (fireAction[source].state && nextTimeToFire >= fireRate && currentAmmo <= 0)
            {
                currentAmmo--;
                nextTimeToFire = 0;
                gunClick.Play();
                Pulse(0.1f, 75, 75, source);//This Passes through the values for controller vibration
            }
        }
    }
    void Fire()
    {
        if (muzzleflash != null) { muzzleflash.Play(); }
        //Spawns Bullet
        Rigidbody bulletrb = Instantiate(bullet, barrelPivot.position, barrelPivot.rotation).GetComponent<Rigidbody>();
        //Adds velocity
        bulletrb.velocity = barrelPivot.forward * shootingSpeed;
        UpdateAmmoCount();
    }
    private void Reloading()
    {
        //Add reload
        print("Reloading");
        if (gunClick != null) gunClick.Play();
    }
    void UpdateAmmoCount()
    {
        print("Add Ammo Count");
        //currentAmmoText.text = currentAmmo.ToString();
    }
    
    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources whichHand)
    {
        trackPadHaptic.Execute(0, duration, frequency, amplitude, whichHand);
    }
}
