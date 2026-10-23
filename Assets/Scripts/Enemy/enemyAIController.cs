using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

//controller for enemy AI combat and movement

public enum enemyAIState
{
    //enemey will do roaming navigation
    idle = 0,
    //enemy will navigate toward the player
    chasing,
    //enemy will try to attack player
    attacking
}

public class EnemyAIController : MonoBehaviour, ICombat
{
    GameObject player;
    //self
    enemyAIState state;
    NavMeshAgent agent;
    [SerializeField] LayerMask groundLayer, playerLayer;
    Rigidbody rigidbody;
    [SerializeField] bool drawDebug;

    [Header("State Machine")]
    //state triggering
    [SerializeField] float sightRange;
    [SerializeField] float attackSuccessRange;
    [SerializeField] float checkSightDelay;
    bool playerCloseSight, playerCloseAttack, recentlySpotted;

    //melee
    [Header("Melee")]
    [SerializeField] float attackRange;
    [SerializeField] float pounceHeight;
    [SerializeField] float pounceSpeed;
    [SerializeField] float attackDelay;
    [SerializeField] float pounceDmg;
    [SerializeField] Vector3 pounceTraceOffset;
    [SerializeField] Vector3 pounceTraceDimensions;
    CombatComponent combat;
    bool midAttack = false;

    //patrolling
    Vector3 target;
    bool targetSet = false;
    [Header("Patrolling")]
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

    void UpdateState()
    {
        //look for player
        if (!recentlySpotted) StartCoroutine(CheckPlayerInSight());

        if (playerCloseSight)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
            {
                state = enemyAIState.attacking;
            } 
            else
            {
                state = enemyAIState.chasing;
            }
        }
        else state = enemyAIState.idle;
    }

    // Update is called once per frame
    //behaviour state machine
    void Update()
    {
        UpdateState();

        switch (state)
        {
            case enemyAIState.idle:
                Patrol();
                break;

            case enemyAIState.chasing:
                Chase();
                break;

            case enemyAIState.attacking:
                TryAttack();
                break;
        }
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

    void MoveInBounds()
    {
        agent.SetDestination(NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1000, NavMesh.AllAreas) ? hit.position : transform.position);
    }

    //move to closest mesh point if off after 5 seconds
    IEnumerator OutOfBoundsCheck()
    {
        yield return new WaitForSeconds(5f);
        if (!agent.isOnNavMesh && agent.enabled) MoveInBounds();
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
        if (agent.enabled && agent.isOnNavMesh)
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
        }

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
            yield return new WaitForSeconds(checkSightDelay);
            recentlySpotted = false;
        }
    }

    //jump at player (during melee, briefly disconnects navmesh)
    IEnumerator Pounce()
    {
        agent.enabled = false;
        yield return new WaitForEndOfFrame();
        
        //jump up const configured and forward toward player
        Vector3 toPlayer = player.transform.position - transform.position;
        toPlayer.y = 0;
        rigidbody.AddForce(
            toPlayer.normalized * pounceSpeed + Vector3.up * pounceHeight,
            ForceMode.Impulse
        );

        StartCoroutine(OutOfBoundsCheck());
    }

    //jump to damage player if out of cooldown
    void TryAttack()
    {
        if (combat.GetCanAttack() && !midAttack)
        {
            StartCoroutine(BeginAttack());
        }
    }

    //defer to tryAttack
    IEnumerator BeginAttack()
    {
        StartCoroutine(Pounce());
        midAttack = true;
        yield return new WaitForSeconds(attackDelay);//wait to get close to player
        if (midAttack) //if hit player collision will trigger attack
        {
            // check player location in bounds
            if (PlayerTrace()) Attack();
            else midAttack = false;
        }
    }

    private void OnDrawGizmos()
    {
        //visualise non-collision attack area
        if (drawDebug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(
                //values set in inspector
                transform.position + (Quaternion.LookRotation(-transform.right, Vector3.up) * pounceTraceOffset),
                pounceTraceDimensions
            );
        }
    }

    //is player in attack hitbox
    bool PlayerTrace()
    {
        return (player.transform.position - transform.position).magnitude < attackSuccessRange;
    }

    //deal damage to player
    void Attack() 
    {
        midAttack = false;
        combat.Attack(player.gameObject, pounceDmg);
    }

    //overlap casing
    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Terrain":
                EventHitGround();
                break;

            case "Player":
                EventHitPlayer();
                break;

            default:
                break;
        }
    }

    //check attack if close to player
    void EventHitPlayer() 
    {
        if (midAttack)
        {
            Attack();
        }
    }

    //check enable agent on hit ground
    void EventHitGround() 
    {
        if (!agent.enabled)
        {
            agent.enabled = true;
            MoveInBounds();
        }
    }

    //ICombat I/F Die
    public void HealthChanged(float health)
    {
        //report death to game state manager
    }
}