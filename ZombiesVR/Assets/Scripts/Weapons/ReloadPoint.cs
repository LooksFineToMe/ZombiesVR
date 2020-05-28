using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadPoint : MonoBehaviour
{
    public Shooting m_Gun;
    public bool magInGun;
    public void ReloadGun(int magcount)
    {
        magInGun = true;
        m_Gun.Reloading(magcount);
    }
}
