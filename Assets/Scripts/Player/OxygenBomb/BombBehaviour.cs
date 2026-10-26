using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable] public struct BombStats
{
    [SerializeField] public float launchSpeed;
    [SerializeField] public float dragCoefficient;
    [SerializeField] public float rotMult;
}

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
    IBombPool parentPool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void Setup(GameObject newCam, BombStats newStats, IBombPool newPool)
    {
        playerCam = newCam;
        stats = newStats;
        parentPool = newPool;
        transform.position = deactivePosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            activeLifetime = Time.time - launchTime;
            Move();
        }
    }

    public void Fire()
    {
        direction = playerCam.transform.forward;
        rotation = Random.rotation;
        transform.position = playerCam.transform.position;
        activeLifetime = 0;
        launchTime = Time.time;
        active = true;
    }

    void ApplyDrag()
    {
        speed = Time.deltaTime * (stats.launchSpeed * Convert.ToSingle(Math.Pow(Math.E, (stats.dragCoefficient * activeLifetime))));
    }

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

    public void Hit()
    {
        Deactivate();
        parentPool.Explosion(this);
    }
}
