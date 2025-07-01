using UnityEngine;
[CreateAssetMenu(fileName = "Wall", menuName = "Scriptable Objects/MyBehavior/Wall")]
public class WallBehavior : MyBehavior
{
    public override Entity CreateEntity()
    {
        var wall = new WallEntity();
        return wall;
    }
}
