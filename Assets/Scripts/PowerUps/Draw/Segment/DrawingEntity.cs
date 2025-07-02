using UnityEngine;
public class DrawingEntity : Entity, ICollidable, IUpdatable
{
    public Entity CollidableEntity => this;

    public MyBoxCollider MyBoxCollidier => boxCollider;

    public MyCircleCollider MyCircleCollider => circleCollider;


    private MyCircleCollider circleCollider;
    private MyBoxCollider boxCollider;

    public PlayerEntity playerOwner;
    float radius => EntityGameObject.transform.localScale.x;

    float lifeTime;
    float currentTime;
    private bool isActive;
    public DrawingEntity(float lifetimeReceived = 2.5f)
    {
        lifeTime = lifetimeReceived;
    }

    public override void WakeUp()
    {
        base.WakeUp();
        circleCollider = new MyCircleCollider(radius, EntityGameObject);
        LevelManager.Instance.OnRoundRestart.AddListener(DeleteAllSegments);
    }
    void DeleteAllSegments()
    {
        InstantiatorManager.Instance.DrawSegmentsPool.pool.Release(this);
    }
    public void UpdateMe(float deltaTime)
    {
        currentTime += Time.deltaTime;

        if (currentTime >= lifeTime)
        {
            DeactivateSegment();
        }
    } 

    public void SetOwner(PlayerEntity player)
    {
        playerOwner = player;
    }

    public void DeactivateSegment()
    {
        InstantiatorManager.Instance.DrawSegmentsPool.pool.Release(this);
    }

    public void ResetTime()
    {
        currentTime = 0f;
    }

}
