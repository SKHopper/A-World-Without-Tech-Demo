using UnityEngine;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;

//potential reasons for missile terminus
public enum MissileHitType
{
    maxDistance = 0,
    ground,
    enemy,
    oxygen
}

//abstracted missile launcher obj
public interface IMissileLauncher
{
    public void TryFire();
    public void MissileHit(MissileHitType hit, GameObject other);
    public Vector3 Position();
}
