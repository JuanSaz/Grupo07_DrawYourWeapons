using UnityEngine;


[CreateAssetMenu(fileName = "WallBehavior", menuName = "Scriptable Objects/MyBehavior/Wall")]

public class WallBehavior : MyBehavior
{
    public override Entity CreateEntity()
    {
        return new WallEntity();
    }
}
