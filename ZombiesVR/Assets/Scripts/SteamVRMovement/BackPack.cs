using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BackPack : MonoBehaviour
{
    int glockAmmo = 0;
    int uziAmmo = 0;
    int shotGunAmmo = 0;
    int revolverAmmo = 0;
    public GameObject[] mags;
    public Transform[] magsPivot;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GlockMag"))
        {
            if (other.gameObject.GetComponent<Magazine>().magCount == other.gameObject.GetComponent<Magazine>().maxAmmo)
            {
                glockAmmo++;
                other.gameObject.GetComponent<Magazine>().Detachobject();
                GameObject mag = Instantiate(mags[0], magsPivot[0].transform.position, magsPivot[0].transform.rotation);
                mag.GetComponent<Rigidbody>().isKinematic = true;
                mag.transform.parent = magsPivot[0].transform;
                foreach (Transform trans in mag.GetComponentsInChildren<Transform>(true))
                {
                 trans.gameObject.layer = LayerMask.NameToLayer("PickedUpObject");
                }
            }
        }
        else if (other.gameObject.CompareTag("UziMag"))
        {
            if (other.gameObject.GetComponent<Magazine>().magCount == other.gameObject.GetComponent<Magazine>().maxAmmo)
            {
                uziAmmo++;
                other.gameObject.GetComponent<Magazine>().Detachobject();
                GameObject mag = Instantiate(mags[1], magsPivot[1].transform.position, magsPivot[1].transform.rotation);
                mag.GetComponent<Rigidbody>().isKinematic = true;
                mag.transform.parent = magsPivot[1].transform;
                foreach (Transform trans in mag.GetComponentsInChildren<Transform>(true))
                {
                    trans.gameObject.layer = LayerMask.NameToLayer("PickedUpObject");
                }
            }
        }
        else if (other.gameObject.CompareTag("ShotGunMag"))
        {
            if (other.gameObject.GetComponent<Magazine>().magCount == other.gameObject.GetComponent<Magazine>().maxAmmo)
            {
                shotGunAmmo++;
                other.gameObject.GetComponent<Magazine>().Detachobject();
                GameObject mag = Instantiate(mags[2], magsPivot[2].transform.position, magsPivot[2].transform.rotation);
                mag.GetComponent<Rigidbody>().isKinematic = true;
                mag.transform.parent = magsPivot[2].transform;
                foreach (Transform trans in mag.GetComponentsInChildren<Transform>(true))
                {
                    trans.gameObject.layer = LayerMask.NameToLayer("PickedUpObject");
                }
            }
        }
        else if (other.gameObject.CompareTag("RevolverMag"))
        {
            if (other.gameObject.GetComponent<Magazine>().magCount == other.gameObject.GetComponent<Magazine>().maxAmmo)
            {
                revolverAmmo++;
                other.gameObject.GetComponent<Magazine>().Detachobject();
                GameObject mag = Instantiate(mags[3], magsPivot[3].transform.position, magsPivot[3].transform.rotation);
                mag.GetComponent<Rigidbody>().isKinematic = true;
                mag.transform.parent = magsPivot[3].transform;
                foreach (Transform trans in mag.GetComponentsInChildren<Transform>(true))
                {
                    trans.gameObject.layer = LayerMask.NameToLayer("PickedUpObject");
                }
            }
        }
    }
}
