using UnityEngine;

//player-side manager of BombBehaviour (OxygenBomb)
public class BombLauncher : MonoBehaviour, IBombLauncher
{
    [SerializeField] GameObject bomb;
    [SerializeField] BombPool pool;
    [SerializeField] int population;
    [SerializeField] GameObject camera;
    [SerializeField] float oxygenCost;
    [SerializeField] BombStats stats;

    OxygenComponent oxygen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        oxygen = GetComponent<OxygenComponent>();
        pool.Setup(
            stats,
            camera, 
            population,
            bomb
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //override IBombLauncher
    public void TryFire()
    {
        oxygen.ChangeOxygen(-oxygenCost);
        pool.Fire();
    }
}
