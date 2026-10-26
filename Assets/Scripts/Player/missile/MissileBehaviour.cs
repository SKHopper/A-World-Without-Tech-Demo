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

public class MissileBehaviour : MonoBehaviour
{
    [SerializeField] LayerMask ground;
    Vector3 deactivePosition;
    bool active = false;
    GameObject playerCam;
    MissileStats stats;
    IMissileLauncher parentLauncher;
    //obj type SimpleParticlesPlayer (rather than SimpleParticles)
    ISimpleParticles player;
    SortedDictionary<MissileHitType, List<string>> hitTypeParticles = new SortedDictionary<MissileHitType, List<string>>();

    SortedDictionary<string, MissileHitType> hitTypeMap = new SortedDictionary<string, MissileHitType>()
    {
        { "Enemy", MissileHitType.enemy },
        { "WalkTerrain", MissileHitType.ground },
        { "OxygenBomb(Clone)", MissileHitType.oxygen }
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = deactivePosition = new Vector3(0, 0, 1000);
    }

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

    void Hit(MissileHitType type, GameObject other)
    {
        Effect(type);
        Toggle(false);
        parentLauncher.MissileHit(type, other);
    }

    //overlap collider
    void Collide()
    { 
        Collider[] hits = Physics.OverlapSphere(transform.position, stats.radius);

        foreach (Collider hit in hits)
        {
            if (hitTypeMap.TryGetValue(hit.gameObject.name, out MissileHitType type)) Hit(type, hit.gameObject);
        }
    }

    //
    void Constrain()
    {
        if (active && Vector3.Distance(transform.position, parentLauncher.Position()) > stats.maxDistance)
        {
            Hit(MissileHitType.maxDistance, null);
        }
    }
}
