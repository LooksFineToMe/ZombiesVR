﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIZombie : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float m_MovementSpeed = 5f;
    [SerializeField] float m_RotationSpeed = 5f;
    [SerializeField] Rigidbody[] m_rb;
    [SerializeField]public RagdollHelper m_RH;
    [Header("Combat")]
    [SerializeField] bool m_Eliminated = false;
    [SerializeField] float m_AttackRange = 2f;
    [SerializeField] float m_TimeToGetUp = 2f;
    [SerializeField] public float m_HeathPoints = 100;
    [SerializeField] public int m_AttackDamage = 1;
    [SerializeField] Animator m_Animations;
    [SerializeField] public bool withinRange;

    [HideInInspector] public bool crawling = false;
    private bool isBleeding = false;
    private float m_BleedingSpeed;

    private float attackTime;
    private float walkTime;

    private bool attackedPlayer;

    private Vector3 agentVelocity = Vector3.zero;

    private NavMeshAgent m_NavMesh;
    [Header("Player Specific")]
    [SerializeField] GameObject m_Target;

    private List<GameObject> m_PlayerList = new List<GameObject>();

    [Header("Wave Specific")]
    [SerializeField] public int m_WaveValue = 1;

    [HideInInspector] public WaveManager m_Spawner;
    public AnimatorClipInfo[] clipinfo;//Matts

    // Start is called before the first frame update
    void Start()
    {
        m_NavMesh = GetComponent<NavMeshAgent>();
        m_NavMesh.speed = m_MovementSpeed;
        m_NavMesh.angularSpeed = m_RotationSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        clipinfo = m_Animations.GetCurrentAnimatorClipInfo(0);
        //print(clipinfo[0].clip.name);

        if (m_Target != null && m_RH.ragdolled == false)
        {
            
            withinRange = CalculateDistance();

            if (withinRange && !crawling)
            {
                m_Animations.SetBool("Attacking", true);
                m_NavMesh.speed = .5f;
                RotateTowards();
            }

            if (!withinRange)
            {
                m_Animations.SetBool("Attacking", false);
                if (clipinfo[0].clip.name == "Zombie_Walk")
                {
                    m_NavMesh.speed = 1f;
                    MoveTowards();
                }
                else if (clipinfo[0].clip.name == "Zombie_Crawl")
                {
                    m_NavMesh.speed = 0.5f;
                    MoveTowards();
                }
            }
        }
        else
        {
            FindTarget();
        }

        if (isBleeding)
            BleedingOut();
    }
    private void Update()
    {

    }

    //for testing purposes, will intergrate AI Wander soon
    private void FindTarget()
    {
        if (m_Spawner.m_Players.Count != 0)
        {
            m_Target = m_Spawner.m_Players[UnityEngine.Random.Range(0, m_Spawner.m_Players.Count)];
        }
        else
        {
            this.enabled = false;
        }
    }

    private void MoveTowards()
    {
        if (!m_Eliminated)
        {
            Vector3 target = m_Target.transform.position - transform.position;

            if (target != Vector3.zero)
            {
                m_NavMesh.destination = m_Target.transform.position;

                Quaternion newRot = Quaternion.LookRotation(target);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, m_RotationSpeed * Time.deltaTime);
            }
        }
    }
    
    private void RotateTowards()
    {
        Vector3 target = m_Target.transform.position - transform.position;
        if (target != Vector3.zero)
        {
            Quaternion newRot = Quaternion.LookRotation(target);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, m_RotationSpeed * Time.deltaTime);
        }

    }

    private bool CalculateDistance()
    {
        Vector3 position = transform.position;
        Vector3 targetPos = m_Target.transform.position;

        Vector3 distance = position - targetPos;

        float largeDistance = Mathf.Max(Mathf.Abs(distance.x), Mathf.Abs(distance.z));

        if (largeDistance <= m_AttackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetWaveManager(WaveManager waveManager)
    {
        m_Spawner = waveManager;
    }

    //lose hp, did we knock the player over with the weapon? or did we kill him with the next blow || call function on collision enter
    public void TakePlayerDamage(float damageSource/*, bool knocked*/)
    {
        m_HeathPoints -= damageSource;

        if (m_HeathPoints <= 0)
        {
            DeahtEvent();
        }
    }

    public IEnumerator ZombieRagdoll()
    {
        m_RH.ragdolled = true;
        yield return new WaitForSeconds(m_TimeToGetUp);
        m_RH.ragdolled = false;
    }

    [ContextMenu("Kill")]
    private void DeahtEvent()
    {
        m_Eliminated = true; //bool to tell the AI to stop moving

        m_NavMesh.velocity = Vector3.zero;
        m_NavMesh.isStopped = true;

        m_Spawner.m_LivingZombies.Remove(this);

        m_RH.ragdolled = true;
        //get all rigibodies and disable "Is Kinematic" so the ragdoll can take over
        Destroy(this.gameObject, 5);//keep this to optimise performence
    }

    //call this in body part scripts
    public void CallBleedOut(float bleedSpeed)
    {
        isBleeding = true;
        m_BleedingSpeed += bleedSpeed; //adds the private float to + this local variable
    }

    private void BleedingOut() //if bleed out is true then call this function every frame until death
    {
        if (m_HeathPoints >= 0 && !m_Eliminated)
        {
            m_HeathPoints -= m_BleedingSpeed * Time.deltaTime;
        }
        else if (m_HeathPoints <= 0)
        {
            DeahtEvent();
        }
    }

    public IEnumerator CreateCrawler()
    {
        m_RH.ragdolled = true;
        m_NavMesh.velocity = Vector3.zero;
        m_NavMesh.isStopped = true;
        yield return new WaitForSeconds(m_TimeToGetUp);
        m_RH.ragdolled = false;
        m_Animations.SetTrigger("Crawler");
        m_NavMesh.velocity = agentVelocity;
        m_NavMesh.isStopped = false;
    }

    [ContextMenu("Stagger")]
    public void Stagger()
    {
        if (!crawling)
        {
            m_Animations.SetBool("Staggered", true);
            m_NavMesh.velocity = Vector3.zero;
            m_NavMesh.isStopped = true;

            Invoke(nameof(ResetStagger), .01f);
        }
    }

    private void ResetStagger()
    {
        m_NavMesh.velocity = agentVelocity;
        m_NavMesh.isStopped = false;
        m_Animations.SetBool("Staggered", false);
    }
}
