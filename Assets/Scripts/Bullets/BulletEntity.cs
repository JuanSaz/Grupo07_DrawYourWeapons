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
    }
    public void UpdateMe(float deltaTime)
    {
        Move();
    }
    public void FixUpdateMe()
    {
        PhysicsUpdate();
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
                Reset();
            }
        }
    }

    private void PhysicsUpdate()
    {
        for (int i = 0; i < GameManager.Instance.ActivePlayersColls.Count; i++)
        {
            if (GameManager.Instance.ActivePlayersColls[i] == null || GameManager.Instance.ActivePlayersColls[i].CollidableEntity == immunePlayer) continue;
            if (MyCircleCollider.IsCircleCollidingCircle(GameManager.Instance.ActivePlayersColls[i].MyCircleCollider))//Si esta colisionando con un player
            {
                PlayerEntity playerHit = (PlayerEntity)GameManager.Instance.ActivePlayersColls[i].CollidableEntity;
                playerHit.GetKilled();
                Reset();
            }
        }
    }

    public void Reset()
    {
        UpdateManager.Instance.Unsubscribe(this);
        UpdateManager.Instance.FixUnsubscribe(this);
        GameManager.Instance.SetBulletCollidable(this, false);
        LevelManager.Instance.OnRoundRestart.RemoveListener(Reset);

        timer = 0;
        immunePlayer = null;
        EntityGameObject.transform.position = new Vector3(0, 0, 0);
        dir = Vector2.zero;

        InstantiatorManager.Instance.bulletPool.pool.Release(this);
    }
}
