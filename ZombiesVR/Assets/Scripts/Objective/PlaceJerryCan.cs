using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceJerryCan : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject m_GlowingJerryCan;
    public GameObject m_RealJerryCan;

    private void Start()
    {
        m_GlowingJerryCan.SetActive(true);
        m_RealJerryCan.SetActive(false);
    }
    public void PlacedJerryCan()
    {
        m_GlowingJerryCan.SetActive(false);
        m_RealJerryCan.SetActive(true);
    }
}
