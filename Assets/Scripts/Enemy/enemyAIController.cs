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

    //state triggering
    [SerializeField] float sightRange, attackRange, checkDelay;
    bool playerCloseSight, playerCloseAttack, recentlySpotted;
    


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
    }

    // Update is called once per frame
    // Patrol until player sighted state machine
    void Update()
    {
        if (!recentlySpotted) StartCoroutine(CheckPlayerInSight());

        if (playerCloseSight)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
            {
                //attack
            }
            else
            {
                Chase();
            }
        }
        else Patrol();
    }

    //try move to random spot
    void Patrol()
    {
        if (!targetSet) SearchForTarget();
        if (targetSet) agent.SetDestination(target);

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

    void Chase()
    {
        agent.SetDestination(player.transform.position);
    }

    private IEnumerator CheckPlayerInSight()
    {
        recentlySpotted = playerCloseSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        if (recentlySpotted)
        {
            yield return new WaitForSeconds(checkDelay);
            recentlySpotted = false;
        }
    }
}
