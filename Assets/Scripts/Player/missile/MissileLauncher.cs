using Unity.VisualScripting;
using UnityEngine;

//IMissileLauncher may be found under starterassets and is communicated to starterAssetsInputs by FPController
public class MissileLauncher : MonoBehaviour, IMissileLauncher
{
    [SerializeField] MissileBehaviour missile;
    [SerializeField] GameObject camera;
    [Header("Stats")]
    [SerializeField] MissileStats stats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        missile.Toggle(true);
    }

    public void MissileHit(MissileHitType hit)
    {
        Debug.Log(hit);
    }

    public Vector3 Position()
    {
        return transform.position;
    }
}
