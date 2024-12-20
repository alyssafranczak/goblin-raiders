using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAINav : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    public FSMStates currentState;
    public float enemySpeed = 5;
    public float chaseDistance = 10;
    public float attackDistance = 2;
    public GameObject player;
    public GameObject[] spellProjectiles;
    public GameObject wandTip;
    public float shootRate = 2f;
    // public GameObject deadVFX;

    GameObject[] wanderpoints;
    Vector3 nextDestination;
    Animator anim;
    float distanceToPlayer;
    float elapsedTime = 0;
    bool isDead;

    //EnemyHealth enemyHealth;
    //int health;

    //Transform deadTransform;

    int currentDestinationIndex = 0;

    NavMeshAgent agent;

    public Transform enemyEyes;
    public float fov = 45f;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        //health = enemyHealth.currentHealth;

        switch (currentState)
        {
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;
            case FSMStates.Dead:
                UpdateDeadState();
                break;

        }

        elapsedTime += Time.deltaTime;

        /*
        if (health <= 0)
        {
            currentState = FSMStates.Dead;
        }*/
    }
    void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        isDead = false;
        wanderpoints = GameObject.FindGameObjectsWithTag("Wanderpoint");
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        wandTip = GameObject.FindGameObjectWithTag("WandTip");

        //enemyHealth = GetComponent<EnemyHealth>();
        //health = enemyHealth.currentHealth;

        currentState = FSMStates.Patrol;
        FindNextPoint();
    }

    void UpdatePatrolState()
    {
        print("Patrolling");

        anim.SetFloat("Speed_f", 0.4f);

        agent.stoppingDistance = 0;

        agent.speed = enemySpeed - (enemySpeed * 0.2f);

        if (Vector3.Distance(transform.position, nextDestination) < 1.5f)
        {
            FindNextPoint();
        }
        else if (CanSeePlayer())
        {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);

        agent.SetDestination(nextDestination);
    }
    void UpdateChaseState()
    {
        print("Chasing");

        anim.SetFloat("Speed_f", 5f);

        nextDestination = player.transform.position;

        agent.stoppingDistance = attackDistance;

        agent.speed = enemySpeed;

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            FindNextPoint();
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);

        agent.SetDestination(nextDestination);
    }
    void UpdateAttackState()
    {
        print("Attacking");

        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);

        anim.SetInteger("WeaponType_int", 12);

        EnemySpellCast();
    }
    void UpdateDeadState()
    {
        //anim.SetInteger("animState", 4);
        //deadTransform = gameObject.transform;
        isDead = true;

        //Destroy(gameObject, 3);
    }

    void FindNextPoint()
    {
        nextDestination = wanderpoints[currentDestinationIndex].transform.position;

        currentDestinationIndex = (currentDestinationIndex + 1)
            % wanderpoints.Length;

        agent.SetDestination(nextDestination);
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionTarget = (target - transform.position).normalized;
        directionTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    void EnemySpellCast()
    {
        if (!isDead)
        {
            if (elapsedTime >= shootRate)
            {
                var animDuration = anim.GetCurrentAnimatorStateInfo(0).length;
                Invoke("SpellCasting", animDuration);
                elapsedTime = 0.0f;

            }
        }
    }

    void SpellCasting()
    {
        int randProjectileIndex = Random.Range(0, spellProjectiles.Length);

        GameObject spellProjectile = spellProjectiles[randProjectileIndex];

        Instantiate(spellProjectile, wandTip.transform.position, wandTip.transform.rotation);
    }

    private void OnDestroy()
    {
        //Instantiate(deadVFX, deadTransform.position, deadTransform.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Vector3 frontRayPoint = enemyEyes.position + (enemyEyes.forward * chaseDistance);
        Vector3 leftRayPoint = Quaternion.Euler(0, fov * 0.5f, 0) * frontRayPoint;
        Vector3 rightRayPoint = Quaternion.Euler(0, -fov * 0.5f, 0) * frontRayPoint;

        Debug.DrawLine(enemyEyes.position, frontRayPoint, Color.cyan);
        Debug.DrawLine(enemyEyes.position, rightRayPoint, Color.magenta);
        Debug.DrawLine(enemyEyes.position, leftRayPoint, Color.yellow);
    }

    bool CanSeePlayer()
    {
        RaycastHit hit;

        Vector3 directionToPlayer = player.transform.position - enemyEyes.position;

        if (Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fov)
        {
            if (Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, chaseDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    print("Player in view");
                    return true;
                }

                return false;
            }

            return false;
        }

        return false;
    }
}
