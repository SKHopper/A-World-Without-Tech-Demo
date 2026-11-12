using UnityEngine;

//observer classes derive to observer their combat comps
public interface ICombat
{
    public void HealthChanged(float health, bool attacked = true);
    public void VulnerabilityChanged() { }
}
