using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpPencil", menuName = "Scriptable Objects/MyBehavior/PowerUpPencil")]

public class PencilBehaviour : PowerUpBehaviour
{
    public override Entity CreateEntity()
    {
        var powerUpEntity = new PencilEntity();
        powerUpEntity.pickUpSoundID = pickUpSoundID;
        return powerUpEntity;
    }
}
