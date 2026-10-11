using System.Collections;
using UnityEngine;
using UnityEngine.AI;

//controller for enemy AI combat and movement

public class enemyAIController : MonoBehaviour
{
    GameObject player;
    //self
    NavMeshAgent agent;
    [SerializeField] LayerMask groundLayer, playerLayer;
    Rigidbody rigidbody;

    //state triggering
    [SerializeField] float sightRange, attackRange, checkDelay;
    bool playerCloseSight, playerCloseAttack, recentlySpotted;

    //melee
    [SerializeField] float pounceHeight, pounceSpeed;
    CombatComponent combat;

    //patroling
    Vector3 target;
    bool targetSet = false;
    [SerializeField] float patrolRange;
    [SerializeField] float successDistance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        combat = GetComponent<CombatComponent>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    //behaviour state machine
    void Update()
    {
        //look for player
        if (!recentlySpotted) StartCoroutine(CheckPlayerInSight());

        if (playerCloseSight)
        {
            Chase();
            if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
            {
                TryAttack();
            }
        }
        else Patrol();
    }

    //use instead of SetDestination
    void SafeMoveTo(Vector3 destination)
    {
        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(destination);
        }
        StartCoroutine(OutOfBoundsCheck());
    }

    //move to closest mesh point if off after 5 seconds
    IEnumerator OutOfBoundsCheck()
    {
        yield return new WaitForSeconds(5f);
        if (!agent.isOnNavMesh && agent.enabled)
        {
            agent.SetDestination(NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1000, NavMesh.AllAreas) ? hit.position : transform.position);
        }
    }

    //try move to random spot
    void Patrol()
    {
        if (!targetSet) SearchForTarget();
        if (targetSet) SafeMoveTo(target);

        if (Vector3.Distance(transform.position, target) < successDistance)
        {
            targetSet = false;
        }
    }

    //find closest spot on navmesh to random point in patrol range from self
    //returns true if valid spot found
    bool SearchForTarget()
    {
        target = new Vector3(
            transform.position.x + Random.Range(-patrolRange, patrolRange), 
            transform.position.y, 
            transform.position.z + Random.Range(-patrolRange, patrolRange)
        );

        //get closest valid, necessary for variable Y axis terrain
        NavMesh.SamplePosition(target, out NavMeshHit hit, 1000, NavMesh.AllAreas);
        target = hit.position;

        //check if complete path available
        NavMeshPath testPath = new NavMeshPath();
        bool pathValid = agent.CalculatePath(target, testPath);
        targetSet = hit.hit && pathValid && (testPath.status == NavMeshPathStatus.PathComplete);

        return targetSet;
    }

    //AI move to player
    void Chase()
    {
        SafeMoveTo(player.transform.position);
    }

    //update if player visible, delay to prevent constant checking
    IEnumerator CheckPlayerInSight()
    {
        recentlySpotted = playerCloseSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        if (recentlySpotted)
        {
            yield return new WaitForSeconds(checkDelay);
            recentlySpotted = false;
        }
    }

    //Jump at player (during melee, briefly disconnects navmesh)
    IEnumerator Pounce()
    {
        agent.enabled = false;
        yield return new WaitForEndOfFrame();
        Vector3 toPlayer = player.transform.position - transform.position;
        toPlayer.y = 0;
        rigidbody.AddForce(
            toPlayer * pounceSpeed + Vector3.up * pounceHeight,
            ForceMode.Impulse
        );
        yield return new WaitForSeconds(2f);
        agent.enabled = true;
    }

    void TryAttack()
    {
        if (combat.GetCanAttack())
        {
            Attack();
        }
    }

    void Attack()
    {
        StartCoroutine(Pounce());
        Debug.Log("attack");
        combat.Attack(player.GetComponent<CombatComponent>(), 10);
    }
}
