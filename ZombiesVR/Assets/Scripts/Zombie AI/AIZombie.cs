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
    [SerializeField] RagdollHelper m_RH;
    [Header("Combat")]
    [SerializeField] bool m_Eliminated = false;
    [SerializeField] float m_AttackRange = 2f;
    [SerializeField] float m_TimeToGetUp = 2f;
    [SerializeField] public int m_HeathPoints = 10;
    [SerializeField] public int m_AttackDamage = 1;
    [SerializeField] Animator m_Animations;
    [SerializeField] public bool withinRange;
    [SerializeField] GameObject m_DeathRagDoll;

    [HideInInspector] public bool crawling = false;
    
    //Creating a quick timer just to get them walking again
    //TESTING ONLY
    float timer;
    float riseAgain = 4;
    //these floats are just used in the update function
    //TESTING ONLY

    private float attackTime;
    private float walkTime;

    private bool attackedPlayer;

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
            }

            if (!withinRange)
            {
                m_Animations.SetBool("Attacking", false);
                if (clipinfo[0].clip.name == "ZOMBIE_Walk")
                {
                    m_NavMesh.speed = 1f;
                    MoveTowards();
                }
                else if (clipinfo[0].clip.name == "Zombie_Crawl")
                {
                    m_NavMesh.speed = 0.8f;
                    MoveTowards();
                }
            }
        }
        else
        {
            FindTarget();
        }
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= riseAgain)
        {
            m_RH.ragdolled = false;
        }
#if UNITY_EDITOR
        if (m_Eliminated)
            TakePlayerDamage(m_HeathPoints);
#endif
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
        if (!m_Eliminated)
        {
            m_NavMesh.destination = m_Target.transform.position;

            Quaternion newRot = Quaternion.LookRotation(m_NavMesh.desiredVelocity);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, m_RotationSpeed * Time.deltaTime);
        }
        else if (m_Eliminated)
        {
            m_NavMesh.speed = 0;
        }
    }

    //not for this project
    private void RotateTowards()
    {
        Quaternion newRot = Quaternion.LookRotation(m_NavMesh.desiredVelocity);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, m_RotationSpeed * Time.deltaTime);
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
    public void TakePlayerDamage(int damageSource/*, bool knocked*/)
    {
        m_HeathPoints -= damageSource;
        timer = 0;

        if (m_HeathPoints <= 0)
        {
            m_Eliminated = true; //bool to tell the AI to stop moving

            m_Spawner.m_LivingZombies.Remove(this);

            m_RH.ragdolled = true;
            //get all rigibodies and disable "Is Kinematic" so the ragdoll can take over
            Destroy(this.gameObject, 5);//keep this to optimise performence
        }
    }

    public void Knock()
    {
        m_NavMesh.speed = 0;
        m_RH.ragdolled = true;
        Invoke(nameof(ResetKnock), m_TimeToGetUp);
    }

    public IEnumerator CreateCrawler()
    {
        m_RH.ragdolled = true;
        m_NavMesh.speed = 0;
        yield return new WaitForSeconds(m_TimeToGetUp);
        m_Animations.SetTrigger("Crawler");
        m_NavMesh.speed = 1;
    }

    public void Stagger()
    {
        m_Animations.SetBool("Staggered", true);
        Invoke(nameof(ResetStagger), .01f);
    }

    private void ResetStagger()
    {
        m_Animations.SetBool("Staggered", false);
    }

    public void ResetKnock()
    {
        m_NavMesh.speed = 1;
        m_RH.ragdolled = false;
    }
}
