using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

//MissileHitType ParticleSystem names and Player
[Serializable] public struct MissileHitParticles
{
    [SerializeField] public MonoBehaviour systemsPlayer;
    [SerializeField] public List<string> maxDistance;
    [SerializeField] public List<string> enemy;
    [SerializeField] public List<string> ground;
    [SerializeField] public List<string> oxygen;
}

//collated missile obj information
[Serializable] public struct MissileStats
{
    //distance moved per second
    [SerializeField] public float speed;
    //distance from player to kill missile
    [SerializeField] public float maxDistance;
    //multiplier on camera rot to missile rot
    [SerializeField] public float sensitivity;
    //overlap sphere size
    [SerializeField] public float radius;
    [SerializeField] public float damage;
    [Header("VFX - MissileHitType ParticleSystem(s)")]
    [SerializeField] public MissileHitParticles particleSystemConfig;
}

//non-re-instanciated reused projectile
public class MissileBehaviour : MonoBehaviour
{
    [SerializeField] LayerMask ground;

    IMissileLauncher parentLauncher;
    //obj type SimpleParticlesPlayer (rather than SimpleParticles)
    GameObject playerCam;

    Vector3 deactivePosition;
    bool active = false;
    MissileStats stats;
    ISimpleParticles player;
    //map hit types to individual particle effect systems
    SortedDictionary<MissileHitType, List<string>> hitTypeParticles = new SortedDictionary<MissileHitType, List<string>>();
    //map hit gamObj names to enumerated hit types
    SortedDictionary<string, MissileHitType> hitTypeMap = new SortedDictionary<string, MissileHitType>()
    {
        { "Enemy",              MissileHitType.enemy },
        { "WalkTerrain",        MissileHitType.ground },
        { "OxygenBomb(Clone)",  MissileHitType.oxygen }
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = deactivePosition = new Vector3(0, 0, 1000);
    }

    //setup by parent launcher
    public void Setup(IMissileLauncher parent, GameObject camera, MissileStats newStats)
    {
        parentLauncher = parent;
        playerCam = camera;
        stats = newStats;

        player = stats.particleSystemConfig.systemsPlayer.GetComponent<ISimpleParticles>();
        //sorted dictionaries cannot be serialized
        hitTypeParticles.Add(MissileHitType.maxDistance, stats.particleSystemConfig.maxDistance);
        hitTypeParticles.Add(MissileHitType.ground, stats.particleSystemConfig.ground);
        hitTypeParticles.Add(MissileHitType.enemy, stats.particleSystemConfig.enemy);
        hitTypeParticles.Add(MissileHitType.oxygen, stats.particleSystemConfig.oxygen);
    }

    public bool GetActive()
    {
        return active;
    }

    void Deactivate()
    {
        active = false;
        transform.position = deactivePosition;
    }

    void Activate()
    {
        active = true;
        transform.position = playerCam.transform.position;
        transform.rotation = playerCam.transform.rotation;
    }

    public void Toggle(bool isActive)
    {
        if (isActive)
        {
            Activate();
        } 
        else
        {
            Deactivate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            Move();
            Rotate();
            Collide();
            Constrain();
        }
    }

    //move forward by speed stat
    void Move()
    {
        transform.position +=
        (
              transform.rotation
            * Vector3.forward
            * stats.speed
            * Time.deltaTime
        );

    }
    
    //rotate toward player look
    void Rotate()
    {
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            playerCam.transform.rotation,
            stats.sensitivity
        );
    }

    void Effect(MissileHitType type)
    {
        //VFX
        player.Play(
            transform.position,
            hitTypeParticles[type]
        );

        //SFX

    }

    //explode and report parent to go idle
    void Hit(MissileHitType type, GameObject other)
    {
        Effect(type);
        Toggle(false);
        parentLauncher.MissileHit(type, other);
    }

    //try hit overlapped colliders
    void Collide()
    { 
        Collider[] hits = Physics.OverlapSphere(transform.position, stats.radius);

        foreach (Collider hit in hits)
        {
            if (hitTypeMap.TryGetValue(hit.gameObject.name, out MissileHitType type)) Hit(type, hit.gameObject);
        }
    }

    //hit maxDistance if active and beyond stat value
    void Constrain()
    {
        if (active && Vector3.Distance(transform.position, parentLauncher.Position()) > stats.maxDistance) 
            Hit(MissileHitType.maxDistance, null);
    }
}
