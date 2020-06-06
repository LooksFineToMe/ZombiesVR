using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public Transform start;
    public Transform finish;
    public bool recoil;
    public float timer;
    public float recoilTime = .3f;
    public float recoilForce = 5f;
    public float recoilReturn = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 1) { timer += Time.deltaTime; }
        
        if (timer < recoilTime)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, finish.localPosition, recoilForce * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, finish.localRotation, recoilForce * Time.deltaTime);
        }
        else if (timer >= recoilTime)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, start.localPosition, recoilReturn * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, start.localRotation, recoilReturn * Time.deltaTime);
        }

        
    }
    public void Recoil()
    {
        timer = 0;
    }
}
