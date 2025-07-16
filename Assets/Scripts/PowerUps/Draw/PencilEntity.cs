using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PencilEntity : PowerUpEntity
{
    public PencilEntity(float lifeTime = 2.5f) : base(lifeTime)
    {
        powerUpDuration = lifeTime;
    }

    public override void WakeUp()
    {
        if (EntityGameObject == null) return;

        myCircleCollider = new MyCircleCollider(EntityGameObject.transform.localScale.x, EntityGameObject);
    }

    public override void OnPickedUp(PlayerEntity entity)
    {
        base.OnPickedUp(entity);
        playerOwner.ActivatePowerUp(this);

    }

    public override void StopPowerup()
    {
        base.StopPowerup();
        playerOwner.SetPencilPowerup(false);
    }
    public override void StartPowerup()
    {
        base.StartPowerup();
        playerOwner.SetPencilPowerup(true);
    }



}

