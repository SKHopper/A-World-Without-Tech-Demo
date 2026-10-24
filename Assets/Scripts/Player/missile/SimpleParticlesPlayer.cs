using System.Collections.Generic;
using UnityEngine;

public class SimpleParticlesPlayer : MonoBehaviour, ISimpleParticles
{
    [Header("Particle Systems Parent GameObject")]
    [SerializeField] GameObject multiSystem;
    ISimpleParticles particles;

    public void Play(Vector3 position, List<string> names)
    {
        particles.Play(position, names);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particles = multiSystem.GetComponent<ISimpleParticles>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
