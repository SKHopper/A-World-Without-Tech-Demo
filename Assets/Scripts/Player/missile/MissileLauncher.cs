using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

//IMissileLauncher may be found under starterassets and is communicated to starterAssetsInputs by FPController
//player-side manager of MissileBehaviour (Missile gameobj)
public class MissileLauncher : MonoBehaviour, IMissileLauncher
{
    [SerializeField] MissileBehaviour missile;
    [SerializeField] GameObject camera;
    [Header("Stats")]
    [SerializeField] MissileStats stats;

    CombatComponent combat;

    bool gameScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameScene = SceneManager.GetActiveScene().name == "AWorldWithout";
        combat = GetComponent<CombatComponent>();
        if (gameScene) missile.Setup(
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

            case MissileHitType.maxDistance:
                //in this case other will be null
                break;

            case MissileHitType.enemy:
                combat.Attack(other, stats.damage);
                break;

            case MissileHitType.ground:
                break;

            case MissileHitType.oxygen:
                other.GetComponent<IBombHittable>().Hit();
                break;
        }
    }

    public Vector3 Position()
    {
        return transform.position;
    }
}
