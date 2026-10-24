using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

//IMissileLauncher may be found under starterassets and is communicated to starterAssetsInputs by FPController
public class MissileLauncher : MonoBehaviour, IMissileLauncher
{
    [SerializeField] MissileBehaviour missile;
    [SerializeField] GameObject camera;
    [Header("Stats")]
    [SerializeField] MissileStats stats;
    CombatComponent combat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        combat = GetComponent<CombatComponent>();
        missile.Setup(
            this,
            camera,
            stats
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    ////override IMissileLauncher
    public void TryFire()
    {
        if (!missile.GetActive() & combat.GetCanAttack())
        {
            missile.Toggle(true);
        }
    }

    public void MissileHit(MissileHitType hit, GameObject other)
    {
        switch (hit)
        {
            case MissileHitType.enemy:
                combat.Attack(other, stats.damage);
                break;

            case MissileHitType.ground:
                break;

            case MissileHitType.maxDistance:
                //in this case other will be null
                break;

            default:
                break;
        }
    }

    public Vector3 Position()
    {
        return transform.position;
    }
}
