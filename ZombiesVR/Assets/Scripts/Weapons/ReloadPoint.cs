using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadPoint : MonoBehaviour
{
    public Shooting m_Gun;
    public ShotGunShooting m_ShotGun;
    public bool magInGun;
    public void ReloadGun(int magcount)
    {
        if (m_Gun != null)
        {
            magInGun = true;
            m_Gun.Reloading(magcount);
        }
        if (m_ShotGun != null)
        {
            magInGun = true;
            m_ShotGun.Reloading(magcount);
        }
    }
}
