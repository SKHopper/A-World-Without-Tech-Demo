using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombPool : MonoBehaviour, IBombPool
{
    List<BombBehaviour> idleQueue = new List<BombBehaviour>();
    List<BombBehaviour> liveQueue = new List<BombBehaviour>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(BombStats stats, GameObject camera, int count, GameObject original)
    {
        for (int index = 0; index < count; index++)
        {
            idleQueue.Add(Instantiate(original, transform.position, Quaternion.identity).GetComponent<BombBehaviour>());
            idleQueue[index].Setup(camera, stats, this);
        }
    }

    BombBehaviour GetNextBomb()
    {
        if (idleQueue.Count() == 0)
        {
            liveQueue.Add(liveQueue.First());
            liveQueue.RemoveAt(0);
        } 
        else
        {
            liveQueue.Add(idleQueue.First());
            idleQueue.RemoveAt(0);
        }

        return liveQueue.Last();
    }

    public void Fire()
    {
        GetNextBomb().Fire();
    }

    //IBombPool override
    public void Explosion(BombBehaviour subject)
    {
        liveQueue.Remove(subject);
        idleQueue.Add(subject);
    }
}
