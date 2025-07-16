using UnityEngine;


public class SpeedBoostEntity : PowerUpEntity
{
    public SpeedBoostEntity(float lifeTime = 2f) : base(lifeTime)
    {
        powerUpDuration = lifeTime;
    }

    public override void WakeUp()
    {
        base.WakeUp();
    }

    public override void OnPickedUp(PlayerEntity entity)
    {
        base.OnPickedUp(entity);

        playerOwner.ActivatePowerUp(this);
    }

    public override void StopPowerup()
    {
        base.StopPowerup();
        playerOwner.SetSpeedBoostPowerUp(false);
    }
    public override void StartPowerup()
    {
        base.StartPowerup();
        playerOwner.SetSpeedBoostPowerUp(true);
    }


}
