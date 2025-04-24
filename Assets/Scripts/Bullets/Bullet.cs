using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour, IUpdatable
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeSpan;
    private GameObject immunePlayer;
    public GameObject ImmunePlayer { get => immunePlayer; set { immunePlayer = value;} }

    public ObjectPool<Bullet> pool;
    private float timer = 0;

    private void Awake()
    {
        UpdateManager.Instance.Subscribe(this);
    }

    public void UpdateMe(float deltaTime)
    {
        if (timer < lifeSpan)
        {
            transform.position += transform.up * speed * Time.deltaTime;
            timer += Time.deltaTime;
            if(timer > lifeSpan)
            {
                timer = 0;
                UpdateManager.Instance.Unsubscribe(this);
                Reset();
                pool.Release(this);
            }
        }
    }

    public void Reset()
    {
        timer = 0;
        immunePlayer = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (collision.gameObject != immunePlayer)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
