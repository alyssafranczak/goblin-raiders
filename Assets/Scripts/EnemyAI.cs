using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
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
    public float walkSpeed = 2;
    public float chaseSpeed = 7;
    public float chaseDistance = 10;
    public float attackDistance = 2;
    public GameObject player;
    public GameObject witchProjectilePrefab;

    GameObject[] wanderpoints;
    Vector3 nextDestination;
    Animator anim;
    float distanceToPlayer;

    int currentDestinationIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        wanderpoints = GameObject.FindGameObjectsWithTag("Wanderpoint");
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

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
    }
    void Initialize()
    {
        currentState = FSMStates.Patrol;
        FindNextPoint();
    }

    void UpdatePatrolState()
    {
        print("Patrolling");

        anim.SetFloat("Speed_f", 0.4f);

        if(Vector3.Distance(transform.position, nextDestination) < 1)
        {
            FindNextPoint();
        }
        else if(distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);

        transform.position = Vector3.MoveTowards(transform.position, nextDestination, walkSpeed * Time.deltaTime);
    }
    void UpdateChaseState()
    {
        print("Chasing");

        nextDestination = player.transform.position;

        anim.SetFloat("Speed_f", 5f);

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);

        transform.position = Vector3.MoveTowards(transform.position, nextDestination, chaseSpeed * Time.deltaTime);
    }
    void UpdateAttackState()
    {
        nextDestination = player.transform.position;

        anim.SetInteger("WeaponType_int", 12);

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
        if (GameObject.FindGameObjectWithTag("WitchProjectile") == null)
        {
            EnemySpellCast();
        }
    }
    void UpdateDeadState()
    {

    }

    void FindNextPoint()
    {
        nextDestination = wanderpoints[currentDestinationIndex].transform.position;

        currentDestinationIndex = (currentDestinationIndex + 1) % wanderpoints.Length;
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionTarget = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

    }

    void EnemySpellCast()
    {
        Vector3 origin = transform.position + transform.forward + (transform.up * 1.5f);
        GameObject projectile = Instantiate(witchProjectilePrefab,
                origin, transform.rotation) as GameObject;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * 10f, ForceMode.VelocityChange);

        projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);
    }

}