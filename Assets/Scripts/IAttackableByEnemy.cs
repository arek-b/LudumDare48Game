using UnityEngine;

public interface IAttackableByEnemy
{
    void BeAttacked(Transform attackSource);
    Transform transform { get; }
}