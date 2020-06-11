using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class AIZombie : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float m_MovementSpeed = 5f;
    [SerializeField] float m_RotationSpeed = 5f;
    [Tooltip("If enabled, turn this zombie into a runner")]
    [SerializeField] public bool isRunner = false;
    [Tooltip("After calling the Scream Method, have a random chance to enable Zombie Run")]
    [SerializeField] int m_RunnerChance = 10;
    [SerializeField] Rigidbody[] m_rb;
    [SerializeField] public RagdollHelper m_RH;
    private bool canWalk;       //the canWalk bool is first called in the FINDTARGET() function, if canWalk is false, 
                                //the zombie will not move from their current position

    [Header("Sight")]
    [Tooltip("A random number picked between 1 and the value set here")]
    [SerializeField] int m_ScreamChance = 5;
    [Tooltip("The Height of the zombies sight to call scream")]
    [SerializeField] float m_HeightMultiplier;
    [Tooltip("The maximum amount of distance the zombie can see and enable the scream method randomly")]
    [SerializeField] float m_SightDistance = 10;
    private bool calledScream = false;
    private bool canScream = false;
    private int m_RandScream;
    private bool m_PickedScreamNumber;

    [Header("Combat")]
    [SerializeField] public bool m_Eliminated = false;
    [SerializeField] float m_AttackRange = 2f;
    [SerializeField] float m_TimeToGetUp = 2f;
    [SerializeField] public float m_HeathPoints = 100;
    [SerializeField] public int m_AttackDamage = 1;
    [SerializeField] Animator m_Animations;
    [SerializeField] public bool withinRange;
    [SerializeField] Transform m_Spine;
    [HideInInspector] public bool m_FightingPlayer;
    [HideInInspector] public bool powerDeath;
    private bool m_PickedFightNumber;


    [HideInInspector] public bool crawling = false;
    [HideInInspector] public bool headless = false;
    private bool isBleeding = false;
    private float m_BleedingSpeed;
    
    private Vector3 agentVelocity = Vector3.zero;

    private NavMeshAgent m_NavMesh;
    [Header("Player Specific")]
    [SerializeField] GameObject m_Target;

    private List<GameObject> m_PlayerList = new List<GameObject>();

    [Header("Wave Specific")]
    [SerializeField] public int m_WaveValue = 1;

    [HideInInspector] public WaveManager m_Spawner;
    [HideInInspector] public ComboManager m_ComboManager;
    public AnimatorClipInfo[] clipinfo;//Matts

    // Start is called before the first frame update
    void Start()
    {
        m_NavMesh = GetComponent<NavMeshAgent>();
        m_NavMesh.speed = m_MovementSpeed;
        m_NavMesh.angularSpeed = m_RotationSpeed;

        m_HeightMultiplier = 1.15f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        clipinfo = m_Animations.GetCurrentAnimatorClipInfo(0);
        //print(clipinfo[0].clip.name);

        if (m_Target != null)
        {
            withinRange = CalculateDistance();

            if (withinRange && !crawling && !m_Eliminated &&!m_RH.ragdolled)
            {
                AttackPlayer();
            }
            else if (!withinRange && !m_Eliminated && !m_RH.ragdolled)
            {
                m_FightingPlayer = false; //bool to tell the body parts to apply damage

                if (!canWalk)
                {
                    Invoke(nameof(ResetCanWalk), 2);
                }
                else if (!canWalk && isRunner)
                {
                    Invoke(nameof(ResetCanWalk), 3);
                }

                if (canWalk)
                {
                    MoveTowards(1);
                }
                else if (crawling)
                {
                    MoveTowards(.03f);
                }

                if (!calledScream)
                    canScream = true;
                else
                    canScream = false;
            }
        }
        else
        {
            FindTarget();
        }

        if (isBleeding)
            BleedingOut();

        if (!calledScream && m_Target != null)
            ZombieSight();
    }

    private void ResetCanWalk()
    {
        canWalk = true;
        m_Animations.SetBool("Attacking", false);
    }

    //could do something with this if it was better
    public void Twitch()
    {
        if (m_Spine != null)
        {
            float rotY = m_Spine.rotation.y;
            m_Spine.localEulerAngles = Vector3.Lerp(m_Spine.localEulerAngles, new Vector3(0, rotY + .5f, 0), 1 * Time.deltaTime);
        }
    }

    //for testing purposes, will intergrate AI Wander soon
    private void FindTarget()
    {
        if (m_Spawner.m_Players.Count != 0)
        {
            m_Target = m_Spawner.m_Players[Random.Range(0, m_Spawner.m_Players.Count)];
            canWalk = true;
        }
        else
        {
            this.enabled = false;
        }
    }

    private void AttackPlayer()
    {
        m_FightingPlayer = true;
        m_NavMesh.velocity = Vector3.zero;
        m_NavMesh.isStopped = false;
        canWalk = false;
        m_Animations.SetBool("Walking", false);
        m_Animations.SetBool("Running", false);
        m_Animations.SetBool("Attacking", true);
        RotateTowards();

        canScream = false;
        //if (!m_PickedFightNumber && fightingPlayer)
        //{
        //    int randomNumber = Random.Range(1, 4);
        //    m_Animations.SetTrigger("Attack" + randomNumber);
        //    m_PickedFightNumber = true;
        //    Invoke(nameof(ResetNumberPick), 1.5f);
        //}
    }

    //private void ResetNumberPick()
    //{
    //    m_PickedFightNumber = false;
    //}

    private void MoveTowards(float agentSpeed)
    {
        if (!m_Eliminated && canWalk)
        {
            if (m_Spawner.m_LivingZombies.Count == 1 && !isRunner && !crawling)
            {
                isRunner = true;
            }

            Vector3 target = m_Target.transform.position - transform.position;
            m_FightingPlayer = false; //set to false so the zombies don't deal damage they're not supposed to

            if (target != Vector3.zero)
            {
                if (!isRunner)
                {
                    m_Animations.SetBool("Walking", true);
                    m_Animations.SetBool("Running", false);
                    m_NavMesh.speed = agentSpeed;

                }
                else if (isRunner)
                {
                    m_Animations.SetBool("Walking", false);
                    m_Animations.SetBool("Running", true);
                    m_NavMesh.speed = agentSpeed * 4;
                }

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

    //so the wave manager knows when the zombie has been eliminated or not
    public void SetWaveManager(WaveManager waveManager, ComboManager comboManager)
    {
        m_Spawner = waveManager;
        m_ComboManager = comboManager;
    }

    //lose hp call function on collision enter
    public void TakePlayerDamage(float damageSource/*, bool knocked*/)
    {
        m_HeathPoints -= damageSource;

        if (m_HeathPoints <= 0)
        {
            if (!powerDeath)
            {
                DeahtEvent();
            }
            else
            {

            }
        }
    }

    [ContextMenu("Kill")]
    private void DeahtEvent()
    {
        m_Eliminated = true;

        m_NavMesh.velocity = Vector3.zero;
        m_NavMesh.isStopped = true;

        m_Spawner.m_LivingZombies.Remove(this);
        m_ComboManager.AddCombo();

        m_RH.ragdolled = true;
        //get all rigibodies and disable "Is Kinematic" so the ragdoll can take over
        Destroy(this.gameObject, 5);//keep this to optimise performence
    }

    [ContextMenu("Death Animation")] //for testing
    public void CallDeathAnimation()
    {
        if (!crawling && !m_Eliminated)
        {
            m_Animations.SetTrigger("DeathAnimation");
            m_Eliminated = true;
            canWalk = false;
            m_Eliminated = true;
            m_NavMesh.velocity = Vector3.zero;
            m_NavMesh.isStopped = true;
            m_Spawner.m_LivingZombies.Remove(this);
            m_ComboManager.AddCombo();
            Invoke(nameof(DeathAnimation), .85f);
        }
    }

    private void DeathAnimation()
    {
        m_RH.ragdolled = true;
        Destroy(this.gameObject, 5);
    }

    [ContextMenu("Ragdoll")] //method for testing purposes
    void Ragdoll()
    {
        StartCoroutine(ZombieRagdoll());
    }

    public IEnumerator ZombieRagdoll()
    {
        m_RH.ragdolled = true;
        canWalk = false;
        m_NavMesh.velocity = Vector3.zero;
        m_NavMesh.isStopped = true;
        yield return new WaitForSeconds(m_TimeToGetUp);
        m_RH.ragdolled = false;
    }

    //call this in body part scripts
    public void CallBleedOut(float bleedSpeed)
    {
        isBleeding = true;
        m_BleedingSpeed += bleedSpeed; //adds the private float to + this local variable
    }

    private void BleedingOut() //if isBleading is true then call this function every frame until death
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

    //when the zombie loses one of their legs, call this method to start the Crawling animation
    public IEnumerator CreateCrawler()
    {
        m_RH.ragdolled = true;
        crawling = true;
        m_NavMesh.velocity = Vector3.zero;
        m_NavMesh.isStopped = true;
        if (isRunner)
            isRunner = false;
        yield return new WaitForSeconds(m_TimeToGetUp);
        m_RH.ragdolled = false;
        m_Animations.SetTrigger("Crawler");
        m_NavMesh.velocity = agentVelocity;
        m_NavMesh.isStopped = false;
    }

    [ContextMenu("Scream")] //to be called randomly when within zombie sight
    private void ZombieScream()
    {
        canWalk = false;
        m_Animations.SetTrigger("Scream");
        m_NavMesh.velocity = Vector3.zero;
        m_NavMesh.isStopped = true;
        Invoke(nameof(ResetScream), 3f);

        if (!isRunner) //after calling scream have a random chance to set the zombie to runner
        {
            int rand = Random.Range(1, m_RunnerChance);

            if (rand == 1)
            {
                isRunner = true;
            }
        }
    }

    private void ResetScream()                                    
    {
        canWalk = true;
        m_NavMesh.velocity = agentVelocity;
        m_NavMesh.isStopped = false;
    }

    [ContextMenu("Stagger")]
    public void Stagger()
    {
        if (!crawling && !m_Eliminated)
        {
            canWalk = false;
            m_Animations.SetBool("Staggered", true);
            m_NavMesh.velocity = Vector3.zero;
            m_NavMesh.isStopped = true;

            StartCoroutine(ResetStagger());
        }
    }

    //called from ragdoll helper
    public IEnumerator GetUpFromBelly()
    {
        m_Animations.SetBool("GetUpFromBelly", true);
        canWalk = false;
        m_NavMesh.velocity = Vector3.zero;
        m_NavMesh.isStopped = true;
        yield return new WaitForSeconds(2.5f);
        canWalk = true;
        m_NavMesh.velocity = agentVelocity;
        m_NavMesh.isStopped = false;
    }

    //called from ragdoll helper
    public IEnumerator GetUpFromBack()
    {
        m_Animations.SetBool("GetUpFromBack", true);
        canWalk = false;
        m_NavMesh.velocity = Vector3.zero;
        m_NavMesh.isStopped = true;
        yield return new WaitForSeconds(2.5f);
        canWalk = true;
        m_NavMesh.velocity = agentVelocity;
        m_NavMesh.isStopped = false;
    }

    private IEnumerator ResetStagger()
    {
        yield return new WaitForSeconds(.01f);
        m_Animations.SetBool("Staggered", false);
        yield return new WaitForSeconds(1);
        m_NavMesh.velocity = agentVelocity;
        m_NavMesh.isStopped = false;
        canWalk = true;
    }

    private void ZombieSight() //this method calls the zombie scream randomly if the player is close enough and a ray hits them
    {
        float distance = Vector3.Distance(transform.position, m_Target.transform.position);
        float screamDistance = m_SightDistance / 2;

        RaycastHit hit;
        //zombie's line of sight and distance
        Debug.DrawRay(transform.position + Vector3.up * m_HeightMultiplier, transform.forward * m_SightDistance, Color.red);
        //minimum distance the player can be to call the scream method
        Debug.DrawRay(transform.position + Vector3.up, transform.forward * screamDistance, Color.blue);

        if (Physics.Raycast(transform.position + Vector3.up * m_HeightMultiplier, transform.forward, out hit, m_SightDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (distance > screamDistance)
                {
                    if (!m_PickedScreamNumber && !m_Eliminated)
                    {
                        m_RandScream = Random.Range(1, m_ScreamChance);
                        m_PickedScreamNumber = true;
                        Invoke(nameof(ResetScreamNumber), 1.5f);
                        if (m_RandScream == 1 && !calledScream && !crawling && !headless && canScream)
                        {
                            ZombieScream();
                            calledScream = true;
                        }
                    }
                }
            }
        }
    }

    private void ResetScreamNumber()
    {
        m_PickedScreamNumber = false;
    }
}
