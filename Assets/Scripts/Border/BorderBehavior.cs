using UnityEngine;
[CreateAssetMenu(fileName = "BorderBehavior", menuName = "Scriptable Objects/MyBehavior/BorderBehavior")]

public class BorderBehavior : MyBehavior
{
    public override Entity CreateEntity()
    {
        var border = new BorderEntity();
        return border;
    }
}
