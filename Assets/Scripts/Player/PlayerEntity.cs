using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class PlayerEntity: Entity, IUpdatable, IFixUpdatable, ICollidable
{
    public PlayerBehavior playerBehavior;
    public SO_PlayerInput inputs;
    private MyCircleCollider circleCollider;
    private float colliderRadius = 0.5f;
    private Vector2 dir = new Vector2(0,1);
    private float movementSpeed = 3.0f;
    private float rotationSpeed = 180f;

    private Vector3 startPos;
    private Quaternion startRot;

    private string playerName;
    private int score = 0;

    private bool isAlive = true;
    public bool IsAlive() => isAlive;
    public Entity CollidableEntity => this;

    public int Score { get { return score; } set { score = value; } }
    public string PlayerName { get { return playerName; } set { playerName = value; } }

    public MyCircleCollider MyCircleCollider => circleCollider;

    public PlayerEntity() 
    {
    
    }

    public override void WakeUp()
    {
        base.WakeUp();

        circleCollider = new MyCircleCollider(colliderRadius, EntityGameObject);
        LevelManager.Instance.OnRoundRestart.AddListener(ResetPlayer);

        UpdateManager.Instance.Subscribe(this);
        UpdateManager.Instance.FixSubscribe(this);
        GameManager.Instance.SetPlayerCollidable(this,true);

        startPos = EntityGameObject.transform.position;
        startRot = EntityGameObject.transform.rotation;
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
        BulletEntity bullet = InstantiatorManager.Instance.bulletPool.pool.Get();
        bullet.EntityGameObject.transform.SetPositionAndRotation(EntityGameObject.transform.position, EntityGameObject.transform.rotation);
        bullet.dir = EntityGameObject.transform.up.normalized;
        bullet.ImmunePlayer = this;
        
    }
    public void AddPoint()
    {
        score++;
    }
    public void GetKilled()
    {
        isAlive = false;
        UpdateManager.Instance.Unsubscribe(this);//saca de update
        UpdateManager.Instance.FixUnsubscribe(this);//saca de fixupdate
        GameManager.Instance.SetPlayerCollidable(this, false);//saca de colisiones
        LevelManager.Instance.onPlayerKilled?.Invoke(this);
        EntityGameObject.gameObject.SetActive(false);
    }
    public void ResetPlayer()
    {
        if (isAlive == false)
        {
            isAlive = true;
            EntityGameObject.gameObject.SetActive(true);
            EntityGameObject.transform.position = startPos;
            EntityGameObject.transform.rotation = startRot;
            UpdateManager.Instance.Subscribe(this);//pone en update
            UpdateManager.Instance.FixSubscribe(this);//pone en fixupdate
            GameManager.Instance.SetPlayerCollidable(this, true);//pone en colisiones
            LevelManager.Instance.ActivePlayers.Add(this);
        }

        else
        {
            EntityGameObject.transform.SetPositionAndRotation(startPos, startRot);
        }

    }
    private void Physics()
    {
        for (int i = 0; i < GameManager.Instance.ActivePlayersColls.Count; i++)
        {
            if (GameManager.Instance.ActivePlayersColls[i] == null || GameManager.Instance.ActivePlayersColls[i].CollidableEntity == this) continue;//Si es nulo o es si mismo
            if (MyCircleCollider.IsCircleCollidingCircle(GameManager.Instance.ActivePlayersColls[i].MyCircleCollider))//Si esta colisionando con otro player
            {
                circleCollider.SolveCircleCollidingStaticCircle(GameManager.Instance.ActivePlayersColls[i].MyCircleCollider);//Mueve solo a este player
            }
        }
    }
   
}
