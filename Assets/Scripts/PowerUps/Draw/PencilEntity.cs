using System.Collections.Generic;
using UnityEngine;

public class PencilEntity : Entity, ICollidable, IUpdatable
{
    public Entity CollidableEntity => this;
    public MyCircleCollider MyCircleCollider { get; private set; }
    public MyBoxCollider MyBoxCollidier { get; private set; }


    private PlayerEntity ownerPlayer = null;
    private float pencilPowerUpDuration = 3f;
    private float currentTime = 0f;


    public PencilEntity()
    {
    }

    public override void WakeUp()
    {
        if (EntityGameObject != null)
        {
            MyCircleCollider = new MyCircleCollider(EntityGameObject.transform.localScale.x, EntityGameObject);

            GameManager.Instance.SetPowerUpCollidable(this, true);
        }
    }
    public void OnPickedUp(PlayerEntity player)
    {
        player.ActivatePencilPowerup();

        GameManager.Instance.SetPowerUpCollidable(this, false);
        EntityGameObject.SetActive(false);

    }

    public void StartPowerup(PlayerEntity player)
    {
        ownerPlayer = player;
        currentTime = 0f;

        // Activar powerup en el player
        ownerPlayer.SetPencilPowerup(true);

        // SUSCRIBIRSE al UpdateManager SOLO cuando es necesario
        UpdateManager.Instance.Subscribe(this);
    }

    public void UpdateMe(float deltaTime)
    {
        currentTime += deltaTime;


        if (currentTime >= pencilPowerUpDuration)
        {
            StopPowerup();
        }
    }



    public void StopPowerup()
    {
        ownerPlayer.SetPencilPowerup(false);
        ownerPlayer.OnPencilFinished();
        UpdateManager.Instance.Unsubscribe(this);
    }
}
