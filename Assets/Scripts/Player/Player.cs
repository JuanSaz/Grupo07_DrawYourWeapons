using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour, IUpdatable
{


    [SerializeField] private string playerName;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float speedMultiplier = 1;
    [SerializeField] private SO_PlayerInput inputs;

    private MyCircleCollider myColl;
    private ObjectPool<Bullet> bulletPool;
    private Vector3 startPos;
    private Quaternion startRot;
    private int score = 0;
    private bool isAlive = true;
    public bool IsAlive() => isAlive;

    public int Score {  get { return score; } set { score = value; } }
    
    public string PlayerName { get { return playerName; } set { playerName = value; } }

    private void Awake()
    {
        myColl = GetComponent<MyCircleCollider>();
        UpdateManager.Instance.Subscribe(this);
        bulletPool = FindAnyObjectByType<BulletPool>().pool;

        startPos = transform.position;
        startRot = transform.rotation;

        LevelManager.Instance.ActivePlayers.Add(this);
        LevelManager.Instance.TotalPlayers.Add(this);
    }

    public void UpdateMe(float deltaTime)
    {
        if (!isAlive) return;

        Movement();

        if (Input.GetButtonDown(inputs.shoot))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        var bullet = bulletPool.Get();
        bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
        bullet.dir = transform.up.normalized;
        bullet.ImmunePlayer = this;
        UpdateManager.Instance.Subscribe(bullet);
    }
    private void Movement()
    {
        float moveInput = Input.GetAxis(inputs.vertical);
        float rotateInput = -Input.GetAxis(inputs.horizontal);

        transform.Rotate(0f, 0f, rotateInput * rotationSpeed * Time.deltaTime);
        transform.position += transform.up * (moveInput * movementSpeed * Time.deltaTime);
    }

    public void GetKilled()
    {
        isAlive = false;
        UpdateManager.Instance.Unsubscribe(this);
        LevelManager.Instance.onPlayerKilled?.Invoke(this);
        gameObject.SetActive(false);
    }



    public void AddPoint()
    {
        score++;
        //Debug.Log($"Player {gameObject.name} tiene {score} puntos.");
    }

    public void ResetPlayer()
    {
        if (isAlive == false)
        {
            isAlive = true;
            gameObject.SetActive(true);
            transform.position = startPos;
            transform.rotation = startRot;
            UpdateManager.Instance.Subscribe(this);
            LevelManager.Instance.ActivePlayers.Add(this);
        }

        else
        {
            transform.SetPositionAndRotation(startPos, startRot);
        }

    }

    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<MyCircleCollider>(out MyCircleCollider other))
        {
            if (collision.CompareTag("Bullets")) return;//Si es una bala, la bala lo mata pero el player no resuelve la col
            myColl.SolveCircleCollidingStaticCircle(other);
        }

        if (collision.TryGetComponent<MyBoxCollider>(out MyBoxCollider otherBox))
        {
            //myColl.SolveWithStaticBox(otherBox);
        }
    }
}
