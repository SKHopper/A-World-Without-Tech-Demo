using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UIElements;

//this component fires all child particle systems at a location+their transform
//should be attached to empty parent
//should be accessed via simpleParticlesPlayer
public class SimpleParticles : MonoBehaviour, ISimpleParticles
{
    SortedDictionary<string, Tuple<ParticleSystem, Vector3>> systems = new SortedDictionary<string, Tuple<ParticleSystem, Vector3>>();

    private IEnumerator StartDelay()
    {
        yield return new WaitForEndOfFrame();
        foreach (ParticleSystem system in GetComponentsInChildren<ParticleSystem>())
        {
            systems.Add(system.name, new Tuple<ParticleSystem, Vector3>(system, system.transform.position));
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartDelay());
    }

    void PlaySystem(ParticleSystem system, Vector3 position, Vector3 origin)
    {
        system.transform.position = position + origin;
        system.Play();
    }

    public void Play(Vector3 position, List<string> names)
    {
        if (names == null)
        {
            foreach (Tuple<ParticleSystem, Vector3> pair in systems.Values)
            {
                PlaySystem(pair.Item1, pair.Item2, position);
            }
        }
        else
        {
            foreach (string name in names)
            {
                Tuple<ParticleSystem, Vector3> pair = systems[name];
                PlaySystem(pair.Item1, pair.Item2, position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
