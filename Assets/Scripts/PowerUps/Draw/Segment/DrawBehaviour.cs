using UnityEngine;


[CreateAssetMenu(fileName = "DrawSegment", menuName = "Scriptable Objects/MyBehavior/DrawSegment")]
public class DrawBehaviour : MyBehavior
{

    public override Entity CreateEntity()
    {
        var drawEntity = new DrawingEntity();
        return drawEntity;
    }
}
