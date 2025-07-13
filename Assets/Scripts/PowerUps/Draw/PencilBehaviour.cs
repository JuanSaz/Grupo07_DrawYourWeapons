using UnityEngine;

[CreateAssetMenu(fileName = "PencilPowerUp", menuName = "Scriptable Objects/MyBehavior/PencilPowerUp")]

public class PencilBehaviour : MyBehavior
{
    [SerializeField] string pickUpSoundID;
    public override Entity CreateEntity()
    {
        var drawingEntity = new PencilEntity();
        drawingEntity.pickUpSoundID = pickUpSoundID;
        return drawingEntity;
    }
}
