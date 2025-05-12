using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour, IUpdatable
{
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

    private void Awake()
    {
        myColl = GetComponent<MyCircleCollider>();
        UpdateManager.Instance.Subscribe(this);
        bulletPool = FindAnyObjectByType<BulletPool>().pool;

        startPos = transform.position;
        startRot = transform.rotation;
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
        bullet.ImmunePlayer = gameObject;
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
        gameObject.SetActive(false);
        UpdateManager.Instance.Unsubscribe(this);
    }

    public bool IsAlive() => isAlive;

    public void AddPoint()
    {
        score++;
        Debug.Log($"Player {gameObject.name} tiene {score} puntos.");
    }

    public void ResetPlayer()
    {
        isAlive = true;
        gameObject.SetActive(true);
        transform.position = startPos;
        transform.rotation = startRot;
        UpdateManager.Instance.Subscribe(this);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<MyCircleCollider>(out MyCircleCollider other))
        {
            myColl.SolveCircleCollidingStaticCircle(other);
        }
    }
}
