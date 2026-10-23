using UnityEngine;

public interface ICombat
{
    public void HealthChanged(float health);
    public void VulnerabilityChanged() { }
}
