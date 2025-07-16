using System.Collections;
using UnityEngine;
public class PowerUpEntity : Entity, ICollidable, IUpdatable
{
    public Entity CollidableEntity => this;
    public MyCircleCollider MyCircleCollider => myCircleCollider;
    public MyBoxCollider MyBoxCollidier { get; private set; }

    protected MyCircleCollider myCircleCollider;

    protected float powerUpDuration;
    protected float currentTime;
    protected float isActive;

    public PlayerEntity playerOwner;

    public string pickUpSoundID;

    public PowerUpEntity(float lifeTime = 2.5f)
    {
        //GameManager.Instance.StartCoroutine(SetCollidableOn());
        GameManager.Instance.SetPowerUpCollidable(this, true);
    }
    public virtual void UpdateMe(float deltaTime)
    {
        currentTime += deltaTime;

        if (currentTime >= powerUpDuration)
        {
            StopPowerup();
        }
    }

    public override void WakeUp()
    {
        if(EntityGameObject == null) return;
        myCircleCollider = new MyCircleCollider(EntityGameObject.transform.localScale.x, EntityGameObject);
    }
    public virtual void SetOwner(PlayerEntity owner)
    {
        playerOwner = owner;
    }
    public virtual void OnPickedUp(PlayerEntity entity)
    {
        if(entity.CurrentPowerUp != null)
        {
            entity.CurrentPowerUp.StopPowerup();
        }

        SetOwner(entity);
        GameManager.Instance.SetPowerUpCollidable(this, false);
        EntityGameObject.SetActive(false);
        GameManager.Instance.PlaySound(pickUpSoundID);
    }
    public virtual void StopPowerup()
    {
        playerOwner.CurrentPowerUp = null;
        UpdateManager.Instance.Unsubscribe(this);
    }
    public virtual void StartPowerup()
    {
        currentTime = 0f;

        UpdateManager.Instance.Subscribe(this);
    }
    /*private IEnumerator SetCollidableOn()
    {
        yield return new WaitForSeconds(1f);
        if (EntityGameObject != null)
        {
            GameManager.Instance.SetPowerUpCollidable(this, true);
        }
    }*/
}
