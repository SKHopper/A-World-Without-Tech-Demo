using UnityEngine;

public class BombLauncher : MonoBehaviour, IBombLauncher
{
    [SerializeField] BombBehaviour bomb;
    [SerializeField] GameObject camera;
    [SerializeField] BombStats stats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bomb.Setup(
            camera, 
            stats
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //override IBombLauncher

    public void TryFire()
    {
        bomb.Launch();

        //Instantiate(bomb, transform.position, Quaternion.identity);
    }
}
