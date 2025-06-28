using System;
using UnityEngine;
using UnityEngine.Pool;

public class BulletEntity : Entity ,IUpdatable, IFixUpdatable, ICollidable
{
    private float speed = 5f;
    private float lifeSpan = 5f;
    private float timer = 0f;

    private PlayerEntity immunePlayer;

    private MyCircleCollider circleCollider;
    private float colliderRadius = 0.35f;
    public Vector2 dir = new Vector2(0, 1);
    private float movementSpeed = 5.0f;

    public ObjectPool<BulletEntity> pool;
    public Entity CollidableEntity => this;

    public MyCircleCollider MyCircleCollider => circleCollider;

    public PlayerEntity ImmunePlayer { get => immunePlayer; set => immunePlayer = value; }

    public BulletEntity()
    {
    }
    public override void WakeUp()
    {
        base.WakeUp();
        circleCollider = new MyCircleCollider(colliderRadius, EntityGameObject);
        UpdateManager.Instance.Subscribe(this);
        UpdateManager.Instance.FixSubscribe(this);
        GameManager.Instance.SetBulletCollidable(this, true);
    }
    public void UpdateMe(float deltaTime)
    {
        Move();
    }
    public void FixUpdateMe()
    {
        
    }

    private void Move()
    {
        if(timer < lifeSpan)
        {
            EntityGameObject.transform.position += (Vector3)dir * speed * Time.deltaTime;
            timer += Time.deltaTime;
            if (timer > lifeSpan)
            {
                timer = 0;
                UpdateManager.Instance.Unsubscribe(this);
                UpdateManager.Instance.FixUnsubscribe(this);
                GameManager.Instance.SetBulletCollidable(this, false);
                Reset();
                InstantiatorManager.Instance.bulletPool.pool.Release(this);
            }
        }
    }

    public void Reset()
    {
        timer = 0;
        immunePlayer = null;
        EntityGameObject.transform.position = new Vector3(0, 0, 0);
        dir = Vector2.zero;
    }
}
