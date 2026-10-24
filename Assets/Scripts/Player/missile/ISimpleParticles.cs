using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface ISimpleParticles
{
    public void Play(Vector3 position, List<string> names);
}
