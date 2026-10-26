using UnityEngine;

public class BombLauncher : MonoBehaviour, IBombLauncher
{
    [SerializeField] GameObject bomb;
    [SerializeField] BombPool pool;
    [SerializeField] int population;
    [SerializeField] GameObject camera;
    [SerializeField] BombStats stats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        pool.Fire();
    }
}
