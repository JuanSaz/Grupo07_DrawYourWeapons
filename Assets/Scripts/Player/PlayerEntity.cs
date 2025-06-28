using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class PlayerEntity: Entity, IUpdatable, IFixUpdatable, ICollidable
{
    public SO_PlayerInput inputs;
    private Rigidbody2D rb;
    private MyCircleCollider circleCollider;
    private float colliderRadius = 0.5f;
    private Vector2 dir = new Vector2(0,1);
    private float movementSpeed = 5.0f;
    private float rotationSpeed = 180f;

    public Entity CollidableEntity => this;

    public MyCircleCollider MyCircleCollider => circleCollider;

    public PlayerEntity() { }

    public override void WakeUp()
    {
        base.WakeUp();

        rb = InstantiatorManager.Instance.GetComponentFrom<Rigidbody2D>(EntityGameObject);
        circleCollider = new MyCircleCollider(colliderRadius, EntityGameObject);
        UpdateManager.Instance.Subscribe(this);
        UpdateManager.Instance.FixSubscribe(this);
        GameManager.Instance.SetPlayerCollidable(this,true);
    }
    public void UpdateMe(float deltaTime)
    {
        Movement();
        if (Input.GetButtonDown(inputs.shoot))
        {
            Shoot();
        }
    }
    public void FixUpdateMe()
    {
        Physics();
    }
    private void Movement()
    {
        float moveInput = Input.GetAxis(inputs.vertical);
        float rotateInput = -Input.GetAxis(inputs.horizontal);

        EntityGameObject.transform.Rotate(0f, 0f, rotateInput * rotationSpeed * Time.deltaTime);
        EntityGameObject.transform.position += EntityGameObject.transform.up * (moveInput * movementSpeed * Time.deltaTime);
    }
    private void Shoot()
    {
        //var bullet = bulletPool.Get();
        BulletEntity bullet = InstantiatorManager.Instance.bulletPool.pool.Get();
        bullet.EntityGameObject.transform.SetPositionAndRotation(EntityGameObject.transform.position, EntityGameObject.transform.rotation);
        bullet.dir = EntityGameObject.transform.up.normalized;
        bullet.ImmunePlayer = this;
        UpdateManager.Instance.Subscribe(bullet);
    }
    private void Physics()
    {
        foreach(ICollidable playerColl in GameManager.Instance.ActivePlayersColls)
        {
            if(playerColl == null || playerColl.CollidableEntity == this) continue;//Si es nulo o es si mismo
            if(MyCircleCollider.IsCircleCollidingCircle(playerColl.MyCircleCollider))//Si esta colisionando con otro player
            {
                circleCollider.SolveCircleCollidingStaticCircle(playerColl.MyCircleCollider);//Mueve solo a este player
            }
        }
    }

}
