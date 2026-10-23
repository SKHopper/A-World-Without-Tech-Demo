using UnityEngine;

public enum MissileHitType
{
    maxDistance = 0,
    ground,
    enemy

}

//abstracted missile launcher obj
public interface IMissileLauncher
{
    public void TryFire();
    public void MissileHit(MissileHitType hit);
    public Vector3 Position();
}
