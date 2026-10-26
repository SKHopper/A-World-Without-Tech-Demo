using UnityEngine;

//observer of child bombs
public interface IBombPool
{
    //reported by subject
    public void Explosion(BombBehaviour subject);
}
