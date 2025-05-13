using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IUpdatable
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float speedMultiplier =  1;
    [SerializeField] private SO_PlayerInput inputs;
    private MyCircleCollider myColl;
    private ObjectPool<Bullet> bulletPool;
    private void Awake()
    {
        myColl = GetComponent<MyCircleCollider>();
        UpdateManager.Instance.Subscribe(this);
        bulletPool = FindAnyObjectByType<BulletPool>().pool;
    }
    public void UpdateMe(float deltaTime)
    {
        Movement();

        if(Input.GetButtonDown(inputs.shoot))
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
        UpdateManager.Instance.Unsubscribe(this);
        gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<MyCircleCollider>(out MyCircleCollider other))
        {
            myColl.SolveCircleCollidingStaticCircle(other);
        }

        if (collision.TryGetComponent<MyBoxCollider>(out MyBoxCollider otherBox))
        {
            myColl.SolveWithStaticBox(otherBox);
        }
    }
}
