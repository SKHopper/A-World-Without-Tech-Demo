using UnityEngine;

//observer of child bombs
public interface IBombPool
{
    public void Explosion(BombBehaviour subject);
}
