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
    [SerializeField] public int m_HeathPoints = 10;
    [SerializeField] public int m_AttackDamage = 1;
    [SerializeField] Animator m_Animations;
    [SerializeField] public bool withinRange;
    [SerializeField] GameObject m_DeathRagDoll;

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
        print(clipinfo[0].clip.name);

        if (m_Target != null)
        {
            withinRange = CalculateDistance();

            if (withinRange)
            {
                m_Animations.SetBool("Attacking", true);
                m_NavMesh.speed = .5f;
                MoveTowards();
            }

            if (!withinRange)
            {
                m_Animations.SetBool("Attacking", false);
                if (clipinfo[0].clip.name == "ZOMBIE_Walk")
                {
                    m_NavMesh.speed = 1f;
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
            m_NavMesh.isStopped = true;
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
    public void TakePlayerDamage(int damageSource)
    {
        m_HeathPoints -= damageSource;

        if (m_HeathPoints <= 0)
        {
            m_Eliminated = true; //bool to tell the AI to stop moving

            m_Spawner.m_LivingZombies.Remove(this);

            m_RH.ragdolled = true;
            //get all rigibodies and disable "Is Kinematic" so the ragdoll can take over
            Destroy(this.gameObject, 5);//keep this to optimise performence
        }
    }
}
