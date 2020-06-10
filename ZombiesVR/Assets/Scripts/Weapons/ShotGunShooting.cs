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
            if (fireAction[source].state && nextTimeToFire >= fireRate && currentAmmo > 0 && semiAuto == false || fireAction[source].stateDown && currentAmmo > 0 && semiAuto == true)
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
                Pulse(0.1f, 75, 75, source);//This Passes through the values for controller vibration
                magInGun = false;
                animator.SetBool("Reload", true);
                DropMag();
            }
        }
        if (rb.velocity.magnitude > 2)
        {
            animator.SetBool("Reload", false);
            openBarrel.SetActive(false);
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
            shellrb.velocity = shellPivot.forward * 4f;
            magazine.SetActive(false);
            spawner_Mag.SpawnAmmo();
        }
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
        //Rigidbody bulletrb = Instantiate(bullet, barrelPivot.position, barrelPivot.rotation).GetComponent<Rigidbody>();
        ////Adds velocity
        //bulletrb.velocity = barrelPivot.forward * shootingSpeed;
        //rb.AddRelativeTorque(recoilAmount, 0, 0);
        UpdateAmmoCount();
        recoil.Recoil();
    }
    [ContextMenu("Reload")]
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
