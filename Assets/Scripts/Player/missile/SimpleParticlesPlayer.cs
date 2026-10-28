using System.Collections.Generic;
using UnityEngine;

//if multiple per game obj, must be attached to an empty child game obj to be inspector-set to using components
//slightly built out middleman of ISimpleParticles interface
public class SimpleParticlesPlayer : MonoBehaviour, ISimpleParticles
{
    [Header("Particle Systems Parent GameObject")]
    [SerializeField] GameObject multiSystem = null;

    ISimpleParticles particles;

    public void Play(Vector3 position, List<string> names)
    {
        particles.Play(position, names);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (multiSystem != null) particles = multiSystem.GetComponent<ISimpleParticles>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
