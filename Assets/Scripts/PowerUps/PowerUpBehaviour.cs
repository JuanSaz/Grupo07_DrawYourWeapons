using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Scriptable Objects/MyBehavior/PowerUp")]

public class PowerUpBehaviour : MyBehavior
{

    [SerializeField] protected string pickUpSoundID;
    //public override Entity CreateEntity()
    //{
    //    var powerUpEntity = new PowerUpEntity();
    //    powerUpEntity.pickUpSoundID = pickUpSoundID;
    //    return powerUpEntity;
    //}
}
