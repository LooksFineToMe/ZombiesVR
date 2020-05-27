﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIZombie : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float m_MovementSpeed = 5f;
    [SerializeField] float m_RotationSpeed = 5f;
    [Header("Combat")]
    [SerializeField] float m_AttackRange = 2f;
    [SerializeField] public int m_HeathPoints = 10;
    [SerializeField] public int m_AttackDamage = 1;
    [SerializeField] Animator m_Animations;
    [SerializeField] public bool withinRange;

    private NavMeshAgent m_NavMesh;
    [Header("Player Specific")]
    [SerializeField] GameObject m_Target;

    private List<GameObject> m_PlayerList = new List<GameObject>();

    [Header("Wave Specific")]
    [SerializeField] public int m_WaveValue = 1;

    [HideInInspector] public WaveManager m_Spawner;

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
        if (m_Target != null)
        {
            withinRange = CalculateDistance();

            if (withinRange)
            {
                AttackPlayer();
            }

            if (!withinRange)
            {
                m_NavMesh.isStopped = false;
                MoveTowards();
            }
        }
        else
        {
            FindTarget();
        }
    }

    private void AttackPlayer()
    {
        print("fighting");
        m_NavMesh.isStopped = true;
        m_Animations.SetBool("Attacking", true);
    }

    //for testing purposes, will intergrate AI Wander soon
    private void FindTarget()
    {
        if (m_Spawner.m_Players.Count != 0)
        {
            m_Target = m_Spawner.m_Players[Random.Range(0, m_Spawner.m_Players.Count)];
        }
        else
        {
            this.enabled = false;
        }
    }

    private void MoveTowards()
    {
        if (!m_NavMesh.isStopped)
        {
            m_Animations.SetBool("Attacking", false);
            m_NavMesh.destination = m_Target.transform.position;
        }
    }

    //not for this project
    private void RotateTowards()
    {
        Vector3 dir = m_Target.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, dir, m_RotationSpeed, 0);

        transform.rotation = Quaternion.LookRotation(newDir);
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

    //lose hp || call function on collision enter
    public void TakePlayerDamage()
    {
        m_HeathPoints --;

        if (m_HeathPoints <= 0)
        {
            m_Spawner.m_LivingZombies.Remove(this);
            //change this later
            Destroy(this.gameObject);
        }
    }
}
