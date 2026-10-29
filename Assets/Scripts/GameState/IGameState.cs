using UnityEngine;

public enum DeathType
{
    zeroHealth,
    zeroOxygen
}

//abstracted gamestate or main menu manager (observer)
public interface IGameState
{
    public void ReportDeath(GameObject corpse, DeathType causeOfDeath = DeathType.zeroHealth) { }
    public void ReportQuit();
}
