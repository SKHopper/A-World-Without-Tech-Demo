using UnityEngine;

//observer classes derive to subscribe to combat comps
public interface ICombat
{
    public void HealthChanged(float health);
    public void VulnerabilityChanged() { }
}
