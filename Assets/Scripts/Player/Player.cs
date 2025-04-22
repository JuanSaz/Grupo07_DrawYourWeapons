using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IUpdatable
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float speedMultiplier =  1;
    [SerializeField] private GameObject bulletPrefab;
    private MyCircleCollider myColl;

    private void Awake()
    {
        myColl = GetComponent<MyCircleCollider>();
        UpdateManager.Instance.Subscribe(this);
    }
    public void UpdateMe(float deltaTime)
    {
        Movement();
        if(Input.GetButtonDown("Shoot"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }

    private void Movement()
    {
        float moveInput = Input.GetAxis("Vertical");
        float rotateInput = -Input.GetAxis("Horizontal");


        transform.Rotate(0f, 0f, rotateInput * rotationSpeed * Time.deltaTime);
        transform.position += transform.up * (moveInput * movementSpeed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<MyCircleCollider>(out MyCircleCollider other))
        {
            myColl.SolveCircleCollidingStaticCircle(other);
            
        }

    }
}
