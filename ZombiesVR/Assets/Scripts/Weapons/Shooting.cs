using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
using System;

public class Shooting : MonoBehaviour
{
    //=================================================================================
    [Header("SteamVR Inputs")]
    [Tooltip("The fire button")]
    public SteamVR_Action_Boolean fireAction;

    [Tooltip("Drop mag button")]
    public SteamVR_Action_Boolean dropMagAction;

    [Tooltip("The trackpad haptic setting")]
    public SteamVR_Action_Vibration trackPadHaptic;

    //=================================================================================

    [Header("BulletSettings")]
    [Tooltip("The Bullet prefab that'll spawn when the player shoots")]
    public GameObject bullet;

    [Tooltip("The pivot point where the bullet will shoot")]
    public Transform barrelPivot;

    [Tooltip("The speed of the bullet when fired")]
    public float shootingSpeed = 1;

    [Tooltip("The firerate of the gun if it's not semi auto")]
    public float fireRate = 0.5f;

    [Tooltip("The current ammo in the gun")]
    public int currentAmmo = 0;

    [Tooltip("Checks if there is a mag in the gun")]
    public bool magInGun;

    [Tooltip("The amount of recoil the gun will produce when shot")]
    public int recoilAmount = -15;// Recoil must be -(Number)

    [Tooltip("The reloadPoint Class")]
    public ReloadPoint reloadPoint;

    [Tooltip("Is the gun Fully auto or semi auto")]
    public bool semiAuto;

    public GunRecoil recoil;

    //=================================================================================

    [Header("FeedBack")]

    [Tooltip("The muzzle flash of the gun")]
    public ParticleSystem muzzleflash;

    [Tooltip("The magazine in the gun")]
    public GameObject magazine;

    [Tooltip("The Mag that'll drop")]
    public GameObject droppedMag;

    //=================================================================================
    [Header("BulletEffects")]
    [Tooltip("Accesses to interactable Class")]
    private Interactable interactable;//This script is needed to figure out the hand that is holding the gun

    [Tooltip("")]
    [SerializeField] float nextTimeToFire = 0f;

    [Header("Spawning")]
    [Tooltip("The Mag that'll drop")]
    public Spawner_Mag spawner_Mag;

    [Tooltip("Rigibody of the gun for recoil")]
    public Rigidbody rb;
    //=================================================================================
    public Animator animator;
    public bool isCocked;
    //==========================================
    [Header("GunSounds")]
    public GameObject sound_Shot;
    public GameObject sound_Click;
    public GameObject sound_Cock;
    public GameObject sound_MagClipDown;
    public GameObject sound_MagClipUp;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        //rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.SetBool("OutOfAmmo", true);
    }
    // Update is called once per frame
    void Update()
    {   
        nextTimeToFire += Time.deltaTime;
        if (interactable.attachedToHand != null)
        {
            SteamVR_Input_Sources source = interactable.attachedToHand.handType;//Checks what hand the gun is in
            if (fireAction[source].state && nextTimeToFire >= fireRate && currentAmmo > 0 && semiAuto == false || fireAction[source].stateDown && currentAmmo > 0 && semiAuto == true && isCocked == true)
            {
                print("implement ammo");
                currentAmmo--;
                nextTimeToFire = 0;
                Fire();
                Pulse(0.1f, 150, 75, source);//This Passes through the values for controller vibration
            }
            else if (fireAction[source].state && nextTimeToFire >= fireRate && currentAmmo <= 0 && semiAuto == false || fireAction[source].stateDown && currentAmmo <= 0 && semiAuto == true /*&& isCocked == true*/)
            {
                animator.SetBool("OutOfAmmo", true);
                if (sound_Click != null) { Instantiate(sound_Click, barrelPivot.position, barrelPivot.rotation); }
                currentAmmo = 0;
                nextTimeToFire = 0;
                Pulse(0.1f, 75, 75, source);//This Passes through the values for controller vibration
            }
            if (currentAmmo < 1)
            {
                animator.SetBool("OutOfAmmo", true);
                isCocked = false;
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
    public void CockingGun()
    {
        if (sound_Cock != null) { Instantiate(sound_Cock, barrelPivot.position, barrelPivot.rotation); }
        isCocked = true;
        animator.SetBool("OutOfAmmo", false);
    }
    [ContextMenu("DropMag")]
    private void DropMag()
    {
        if (sound_MagClipDown != null) { Instantiate(sound_MagClipDown, barrelPivot.position, barrelPivot.rotation); }
        reloadPoint.magInGun = false;
        magazine.SetActive(false);
        Instantiate(droppedMag, magazine.transform.position, Quaternion.identity).GetComponent<Magazine>().magCount = currentAmmo;
        if (currentAmmo == 0) { spawner_Mag.SpawnAmmo(); }        
        currentAmmo = 0;
        print("Drop Magazine");
        //remember to add a way for the dropped mag to carry the ammo count with it
    }
    [ContextMenu("Fire")]
    void Fire()
    {
        animator.SetTrigger("Fire");
        if (muzzleflash != null) { muzzleflash.Play(); }
        if (sound_Shot != null) { Instantiate(sound_Shot, barrelPivot.position, barrelPivot.rotation); }
        //Spawns Bullet
        Rigidbody bulletrb = Instantiate(bullet, barrelPivot.position, barrelPivot.rotation).GetComponent<Rigidbody>();
        //Adds velocity
        bulletrb.velocity = barrelPivot.forward * shootingSpeed;
        //rb.AddRelativeTorque(recoilAmount, 0, 0);
        UpdateAmmoCount();
        recoil.Recoil();
    }
    [ContextMenu("Reload")]
    public void Reloading(int reloadAmount)
    {
        //Add reload
        if (sound_MagClipUp != null) { Instantiate(sound_MagClipUp, barrelPivot.position, barrelPivot.rotation); }
        magInGun = true;
        magazine.SetActive(true);
        currentAmmo = reloadAmount;
        print("Reloading: " + reloadAmount + "Bullets");
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
