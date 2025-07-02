using UnityEngine;

[CreateAssetMenu(fileName = "PencilPowerUp", menuName = "Scriptable Objects/MyBehavior/PencilPowerUp")]

public class PencilBehaviour : MyBehavior
{
    public override Entity CreateEntity()
    {
        var drawingEntity = new PencilEntity();
        return drawingEntity;
    }
}
