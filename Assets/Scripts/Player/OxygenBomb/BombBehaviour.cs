using System;
using UnityEngine;
using Random = UnityEngine.Random;

//info needed by bombs collated to be set in launcher comp
[Serializable] public struct BombStats
{
    [SerializeField] public MonoBehaviour systemsPlayer;
    [SerializeField] public float launchSpeed;
    [SerializeField] public float dragCoefficient;
    [SerializeField] public float rotMult;
    [SerializeField] public float AOERadius;
    //vulnerability (see CombatComponent)
    [SerializeField] public float effectStrength;
    [SerializeField] public float effectDuration;
}

//central component for oxygen bomb instances
public class BombBehaviour : MonoBehaviour, IBombHittable
{
    Vector3 deactivePosition = new Vector3(0, 0, 1000);
    bool active = false;
    float speed;
    Vector3 direction;
    Quaternion rotation;
    float launchTime;
    float activeLifetime;
    BombStats stats;

    GameObject playerCam;
    //type BombPool
    IBombPool parentPool;
    //type SimpleParticlesPlayer
    ISimpleParticles player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    //start deactivated
    public void Setup(GameObject newCam, BombStats newStats, IBombPool newPool)
    {
        playerCam = newCam;
        stats = newStats;
        parentPool = newPool;
        transform.position = deactivePosition;

        player = stats.systemsPlayer.GetComponent<ISimpleParticles>();
    }

    // Update is called once per frame
    //move if active
    void Update()
    {
        if (active)
        {
            activeLifetime = Time.time - launchTime;
            Move();
        }
    }

    //activate and begin moving from player
    public void Fire()
    {
        direction = playerCam.transform.forward;
        rotation = Random.rotation;
        transform.position = playerCam.transform.position;
        activeLifetime = 0;
        launchTime = Time.time;
        active = true;
    }

    //exponential decay using N(t) = N[0]e^-dT
    void ApplyDrag()
    {
        speed = Time.deltaTime * (stats.launchSpeed * Convert.ToSingle(Math.Pow(Math.E, (stats.dragCoefficient * activeLifetime))));
    }

    //per frame rot and pos velocity application
    void Move()
    {
        ApplyDrag();
        transform.position += direction * speed;
        transform.rotation = transform.rotation * Quaternion.Slerp(Quaternion.identity, rotation, speed * stats.rotMult);
    }

    public bool GetActive() { return active; }

    void Deactivate()
    {
        active = false;
        transform.position = deactivePosition;
    }

    //apply stats based vulnerability to nearby combatants with canBeVulnerable
    void vulnerabilityAOE()
    {
        foreach (Collider hit in Physics.OverlapSphere(transform.position, stats.AOERadius))
        {
            if (hit.TryGetComponent(out CombatComponent combatant))
            {
                combatant.ApplyVulnerability(stats.effectDuration, stats.effectDuration);
            }
        }
    }

    //do aoe and visual effect, go idle
    public void Hit()
    {
        vulnerabilityAOE();
        player.Play(transform.position);
        Deactivate();
        parentPool.Explosion(this);
    }
}
