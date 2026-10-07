using UnityEngine;
using UnityEngine.AI;

//controller for enemy AI combat and movement

public class enemyAIController : MonoBehaviour
{
    GameObject player;
    //self
    NavMeshAgent agent;
    [SerializeField] LayerMask groundLayer, playerLayer;

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
    void Update()
    {
        Patrol();
    }

    //try move to random spot
    void Patrol()
    {
        if (!targetSet) 
        {
            SearchForTarget();
        } 
        if (targetSet) 
        {
            agent.SetDestination(target);
            Debug.Log("set target to: " + target);
        }
        if (Vector3.Distance(transform.position, target) < successDistance)
        {
            targetSet = false;
            Debug.Log("reached");
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
        Debug.Log(NavMesh.SamplePosition(target, out NavMeshHit hit, 1000, NavMesh.AllAreas));
        target = hit.position;

        //check if complete path available
        NavMeshPath testPath = new NavMeshPath();
        bool pathValid = agent.CalculatePath(target, testPath);
        targetSet = hit.hit && pathValid && (testPath.status == NavMeshPathStatus.PathComplete);

        return targetSet;
    }

}
