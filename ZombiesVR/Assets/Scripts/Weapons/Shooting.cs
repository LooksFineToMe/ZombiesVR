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
    public SteamVR_Action_Boolean dropMagAction;
    public SteamVR_Action_Vibration trackPadHaptic;

    [Header("BulletSettings")]
    public GameObject bullet;
    public Transform barrelPivot;
    public float shootingSpeed = 1;
    public float fireRate = 0.5f;
    public int currentAmmo = 0;
    public bool magInGun;
    public ReloadPoint reloadPoint;
    float timer;// For fireRate

    [Header("FeedBack")]
    public Text currentAmmoText;
    public AudioSource gunClick;
    public ParticleSystem muzzleflash;
    public GameObject magazine;
    public GameObject droppedMag;

    [Header("BulletEffects")]
    private Interactable interactable;//This script is needed to figure out the hand that is holding the gun
    [SerializeField] float nextTimeToFire = 0f;

    //public Animator animator;
    private void Start()
    {
        gunClick = GetComponent<AudioSource>();
        interactable = GetComponent<Interactable>();
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
                currentAmmo--;
                nextTimeToFire = 0;
                Fire();
                Pulse(0.1f, 150, 75, source);//This Passes through the values for controller vibration
            }
            else if (fireAction[source].state && nextTimeToFire >= fireRate && currentAmmo <= 0)
            {
                currentAmmo--;
                nextTimeToFire = 0;
                if (gunClick != null) { gunClick.Play(); }
                Pulse(0.1f, 75, 75, source);//This Passes through the values for controller vibration
            }
        }
        if (interactable.attachedToHand != null)
        {
            SteamVR_Input_Sources source = interactable.attachedToHand.handType;//Checks what hand the gun is in
            if (dropMagAction[source].state && magInGun == true)
            {
                Pulse(0.1f, 75, 75, source);//This Passes through the values for controller vibration
                magInGun = false;
                DropMag();
            }
        }
    }

    private void DropMag()
    {
        reloadPoint.magInGun = false;
        magazine.SetActive(false);
        Instantiate(droppedMag, magazine.transform.position, Quaternion.identity).GetComponent<Magazine>().magCount = currentAmmo;
        currentAmmo = 0;
        print("Drop Magazine");
        //remember to add a way for the dropped mag to carry the ammo count with it
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
    public void Reloading(int reloadAmount)
    {
        //Add reload
        magInGun = true;
        magazine.SetActive(true);
        currentAmmo = reloadAmount;
        print("Reloading: " + reloadAmount + "Bullets");
        if (gunClick != null) { gunClick.Play(); }
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
