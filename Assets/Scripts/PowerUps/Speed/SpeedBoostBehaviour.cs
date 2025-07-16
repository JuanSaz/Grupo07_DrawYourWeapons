using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpSpeed", menuName = "Scriptable Objects/MyBehavior/PowerUpSpeed")]

public class SpeedBoostBehaviour : PowerUpBehaviour
{
    public override Entity CreateEntity()
    {
        var powerUpEntity = new SpeedBoostEntity();
        powerUpEntity.pickUpSoundID = pickUpSoundID;
        return powerUpEntity;
    }
}
