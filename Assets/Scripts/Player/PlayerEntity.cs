using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.U2D;
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
    private float timeBetweenShots = 0.85f;
    private float shotTimer = 0f;

    private float flashOffset = 0.5f;
    private float drawingOffset = 0.75f;


    private Vector3 startPos;
    private Quaternion startRot;
    private Color entityColor;
    public Color EntityColor => entityColor;

    private string playerName;
    private int score = 0;

    private bool isAlive = true;
    public bool IsAlive() => isAlive;

    public int Score { get { return score; } set { score = value; } }
    public string PlayerName { get { return playerName; } set { playerName = value; } }

    public Entity CollidableEntity => this;
    public MyCircleCollider MyCircleCollider => circleCollider;
    public MyBoxCollider MyBoxCollidier => null;

    private bool hasPencilPowerup = false;
    private PencilEntity currentPencil = null;

    //Audio
    public string shootSoundID;

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

        CheckColor(playerBehavior.type);

        startPos = EntityGameObject.transform.position;
        startRot = EntityGameObject.transform.rotation;
    }
    public void UpdateMe(float deltaTime)
    {
        Movement();
        if (shotTimer > 0)
        {
            shotTimer -= Time.deltaTime;
            if (shotTimer <= 0)
            {
                shotTimer = 0;
            }
        }
        if (Input.GetButtonDown(inputs.shoot))
        {
            if(shotTimer <= 0)
            {
                GameManager.Instance.PlaySound(shootSoundID);
                Shoot();
            }
        }
        
    }
    public void FixUpdateMe()
    {
        Physics();

        if (hasPencilPowerup && (Input.GetAxisRaw(inputs.horizontal) != 0 || Input.GetAxisRaw(inputs.vertical) != 0))
        {
            DrawingEntity segment = InstantiatorManager.Instance.DrawSegmentsPool.pool.Get();
            segment.SetOwner(this);
            segment.EntityGameObject.transform.position = EntityGameObject.transform.position + (-EntityGameObject.transform.up * drawingOffset);
        }

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
        FlashEntity flash = InstantiatorManager.Instance.flashPool.pool.Get();
        bullet.EntityGameObject.transform.SetPositionAndRotation(EntityGameObject.transform.position, EntityGameObject.transform.rotation);
        flash.EntityGameObject.transform.SetPositionAndRotation(EntityGameObject.transform.position + (EntityGameObject.transform.up * flashOffset), EntityGameObject.transform.rotation);
        bullet.dir = EntityGameObject.transform.up.normalized;
        bullet.ImmunePlayer = this;
        shotTimer = timeBetweenShots;
    }

    public void ActivatePencilPowerup()
    {
        if (hasPencilPowerup) return;

        currentPencil = new PencilEntity();
        currentPencil.WakeUp();
        currentPencil.StartPowerup(this);
    }

    public void SetPencilPowerup(bool active)
    {
        hasPencilPowerup = active;
    }

    public void OnPencilFinished()
    {
        currentPencil = null;
    }
    public void AddPoint()
    {
        score++;
    }

    private void CheckColor(MyBehaviorType playerType)
    {
        switch (playerType)
        {
            case MyBehaviorType.Player1:
                entityColor = new Color(0.254902f, 0.2901961f, 0.7019608f);
                break;
            case MyBehaviorType.Player2:
                entityColor = new Color(0.9333333f, 0.1294118f, 0.1294118f);
                break;
            case MyBehaviorType.Player3:
                entityColor = new Color(0.8392157f, 0.7450981f, 0.254902f);
                break;
            case MyBehaviorType.Player4:
                entityColor = new Color(0.2352941f, 0.6588235f, 0.04705882f);
                break;
        }
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
            shotTimer = 0;
            EntityGameObject.gameObject.SetActive(true);
            EntityGameObject.transform.position = startPos;
            EntityGameObject.transform.rotation = startRot;
            UpdateManager.Instance.Subscribe(this);//pone en update
            UpdateManager.Instance.FixSubscribe(this);//pone en fixupdate
            GameManager.Instance.SetPlayerCollidable(this, true);//pone en colisiones
            LevelManager.Instance.ActivePlayers.Add(this);
            hasPencilPowerup = false;
        }
        else
        {
            EntityGameObject.transform.SetPositionAndRotation(startPos, startRot);
        }

        if (currentPencil != null)
        {
            currentPencil.StopPowerup();
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

        for (int i = 0; i < GameManager.Instance.ActiveWallsColls.Count; i++)
        {
            if (MyCircleCollider.IsBoxCollidingCircle(GameManager.Instance.ActiveWallsColls[i].MyBoxCollidier))
            {
                MyCircleCollider.SolveWithStaticBox(GameManager.Instance.ActiveWallsColls[i].MyBoxCollidier);
            }
        }

        for (int i = 0; i < GameManager.Instance.ActiveDrawSegments.Count; i++)
        {
            DrawingEntity draw = GameManager.Instance.ActiveDrawSegments[i] as DrawingEntity;
            if (GameManager.Instance.ActiveDrawSegments[i] == null) continue;
            if (MyCircleCollider.IsCircleCollidingCircle(GameManager.Instance.ActiveDrawSegments[i].MyCircleCollider))//Si esta colisionando con otro player
            {
                circleCollider.SolveCircleCollidingStaticCircle(GameManager.Instance.ActiveDrawSegments[i].MyCircleCollider);//Mueve solo a este player
            }
        }

        for (int i = GameManager.Instance.ActivePowerUpColls.Count - 1; i >= 0; i--)
        {
            if (GameManager.Instance.ActivePowerUpColls[i] is PencilEntity powerUp)
            {
                if (MyCircleCollider.IsCircleCollidingCircle(powerUp.MyCircleCollider))
                {
                    powerUp.OnPickedUp(this);
                    GameManager.Instance.ActivePowerUpColls.RemoveAt(i);
                }
            }
        }
    }
}
