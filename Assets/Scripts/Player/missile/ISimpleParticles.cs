using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

//to pass play messages down from non-particle objects to individual particle systems
public interface ISimpleParticles
{
    //null executes all in particles behaviour
    public void Play(Vector3 position, List<string> names = null);
}
