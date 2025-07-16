using UnityEngine;


public class SpeedBoostEntity : PowerUpEntity
{
    public SpeedBoostEntity(float lifeTime = 2.5f) : base(lifeTime)
    {
        powerUpDuration = lifeTime;
    }

    public override void OnPickedUp(PlayerEntity entity)
    {
        base.OnPickedUp(entity);
        if (entity.CurrentPowerUp == null)
        {
            playerOwner.ActivatePowerUp(this);
        }     
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
