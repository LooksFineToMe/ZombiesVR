using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadPoint : MonoBehaviour
{
    public Shooting m_Gun;
    public void ReloadGun(int magcount)
    {
        m_Gun.Reloading(magcount);
    }
}
