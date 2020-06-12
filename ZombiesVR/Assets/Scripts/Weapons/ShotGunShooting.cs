using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class ShotGunShooting : MonoBehaviour
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
    public Transform[] barrelPivot;

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
    [Tooltip("The gun click noise when out of ammo")]
    public AudioSource gunClick;

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
    [Header("ShotGunSpecific")]
    public GameObject shellsToSpawn;
    public Transform[] shellLocations;
    public GameObject openBarrel;
    public bool gunCocked;
    public float shellVelocity = 4;
    [Header("Sounds")]
    public GameObject sound_Shot;
    public GameObject sound_Click;
    public GameObject sound_Cock;
    public GameObject sound_MagClipDown;
    public GameObject sound_MagClipUp;
    private void Start()
    {
        gunClick = GetComponent<AudioSource>();
        interactable = GetComponent<Interactable>();
        //rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        nextTimeToFire += Time.deltaTime;
        if (interactable.attachedToHand != null)
        {
            SteamVR_Input_Sources source = interactable.attachedToHand.handType;//Checks what hand the gun is in
            if (fireAction[source].state && nextTimeToFire >= fireRate && currentAmmo > 0 && semiAuto == false && gunCocked == true || fireAction[source].stateDown && currentAmmo > 0 && semiAuto == true && gunCocked == true)
            {
                print("implement ammo");
                currentAmmo--;
                nextTimeToFire = 0;
                Fire();
                Pulse(0.1f, 150, 75, source);//This Passes through the values for controller vibration
            }
            else if (fireAction[source].state && nextTimeToFire >= fireRate && currentAmmo <= 0 && semiAuto == false || fireAction[source].stateDown && currentAmmo <= 0 && semiAuto == true)
            {
                currentAmmo = 0;
                if (sound_Click != null) { Instantiate(sound_Click, barrelPivot[0].position, barrelPivot[0].rotation); }
                nextTimeToFire = 0;
                if (gunClick != null) { gunClick.Play(); }
                Pulse(0.1f, 75, 75, source);//This Passes through the values for controller vibration
            }
        }
        if (interactable.attachedToHand != null)
        {
            SteamVR_Input_Sources source = interactable.attachedToHand.handType;//Checks what hand the gun is in
            if (dropMagAction[source].state /*&& magInGun == true*/)
            {
                gunCocked = false;
                Pulse(0.1f, 75, 75, source);//This Passes through the values for controller vibration
                magInGun = false;
                animator.SetBool("Reload", true);
                DropMag();
            }
        }
        if (rb.velocity.magnitude > 2 && gunCocked == false)
        {
            gunCocked = true;
            animator.SetBool("Reload", false);
            openBarrel.SetActive(false);
            if (sound_Cock != null) { Instantiate(sound_Cock, barrelPivot[0].position, barrelPivot[0].rotation); }
        }
    }
    [ContextMenu("DropMag")]
    private void DropMag()
    {
        if (reloadPoint.magInGun == true)
        {
            StartCoroutine(ShellEject());
        }
        
        reloadPoint.magInGun = false;
        openBarrel.SetActive(true);
        //Instantiate(droppedMag, magazine.transform.position, Quaternion.identity).GetComponent<Magazine>().magCount = currentAmmo;
        //if (currentAmmo == 0) { spawner_Mag.SpawnAmmo(); }
        currentAmmo = 0;
        print("Drop Magazine");
        //remember to add a way for the dropped mag to carry the ammo count with it
    }
    IEnumerator ShellEject()
    {
        yield return new WaitForSeconds(.4f);
        foreach (Transform shellPivot in shellLocations)
        {
            Rigidbody shellrb = Instantiate(shellsToSpawn, shellPivot.transform.position, shellPivot.rotation).GetComponent<Rigidbody>();
            shellrb.velocity = shellPivot.forward * shellVelocity;
            magazine.SetActive(false);
        }
        spawner_Mag.SpawnAmmo();
        if (sound_MagClipDown != null) { Instantiate(sound_MagClipDown, barrelPivot[0].position, barrelPivot[0].rotation); }
        //Posibly add sound for eject
    }
    [ContextMenu("Fire")]
    void Fire()
    {
        if (muzzleflash != null) { muzzleflash.Play(); }
        //Spawns Bullet
        for (int i = 0; i < barrelPivot.Length; i++)
        {
            Rigidbody bulletrb = Instantiate(bullet, barrelPivot[i].position, barrelPivot[i].rotation).GetComponent<Rigidbody>();
            bulletrb.velocity = barrelPivot[i].forward * shootingSpeed;
        }
        if (sound_Shot != null) { Instantiate(sound_Shot, barrelPivot[0].position, barrelPivot[0].rotation); }
        //Rigidbody bulletrb = Instantiate(bullet, barrelPivot.position, barrelPivot.rotation).GetComponent<Rigidbody>();
        ////Adds velocity
        //bulletrb.velocity = barrelPivot.forward * shootingSpeed;
        //rb.AddRelativeTorque(recoilAmount, 0, 0);
        animator.SetTrigger("Fire");
        UpdateAmmoCount();
        recoil.Recoil();
    }
    [ContextMenu("Reload")]
    public void Reloading(int reloadAmount)
    {
        //Add reload
        if (sound_MagClipUp != null) { Instantiate(sound_MagClipUp, barrelPivot[0].position, barrelPivot[0].rotation); }
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
