using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class JerryCan_Properties : MonoBehaviour
{
    bool m_Used;
    bool m_Refilling;
    public Image m_RefillUI;
    float m_RefillSpeed = 0.2f;
    Interactable m_Interactable;
    public WinManager m_WinManager;
    // Start is called before the first frame update
    void Start()
    {
        m_Interactable = GetComponent<Interactable>();
        m_RefillUI.enabled = false;
        m_RefillUI.fillAmount = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Refilling == true)
        {
            m_RefillUI.fillAmount = Mathf.Lerp(m_RefillUI.fillAmount, 2f, Time.deltaTime * m_RefillSpeed);
            if (m_RefillUI.fillAmount >= 1) { CarFilled(); }
        }
        
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("JerryCan") && m_Used == false && m_Interactable.attachedToHand == true)
        {
            //OLDSYSTEM!
            //m_Used = true;
            //m_Refilling = true;
            //m_RefillUI.enabled = true;
            //OLDSYSTEM
            m_Used = true;
            other.gameObject.GetComponentInParent<PlaceJerryCan>().PlacedJerryCan();
            CarFilled();
        }
    }

    public void Refill()
    { 
    
    }

    private void CarFilled()
    {
        m_Refilling = false;
        m_WinManager.CheckRefills();
        m_Interactable.attachedToHand.DetachObject(gameObject);
        //m_RefillUI.enabled = false;
        Destroy(gameObject);
    }
}
