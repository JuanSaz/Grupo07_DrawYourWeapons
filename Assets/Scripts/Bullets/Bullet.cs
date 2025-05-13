using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour, IUpdatable
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeSpan;
    private Player immunePlayer;
    private MyCircleCollider myColl;
    public Vector2 dir;
    public Player ImmunePlayer { get => immunePlayer; set { immunePlayer = value;} }

    public ObjectPool<Bullet> pool;
    private float timer = 0;

    private void Awake()
    {
        myColl = GetComponent<MyCircleCollider>();
    }
    public void UpdateMe(float deltaTime)
    {
        if (timer < lifeSpan)
        {
            transform.position += (Vector3)dir * speed * Time.deltaTime;
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
        transform.position = new Vector3(0, 0, 0);
        dir = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player playerHit))
        {
            if (playerHit == immunePlayer) return;

            UpdateManager.Instance.Unsubscribe(this);
            Reset();
            pool.Release(this);
            playerHit.GetKilled();
        }

        if (collision.TryGetComponent<MyBoxCollider>(out MyBoxCollider otherBox))
        {
            dir = myColl.ProjectCircleOntoLine(otherBox, dir);
            immunePlayer = null;
        }

        if(collision.TryGetComponent<MyCircleCollider>(out MyCircleCollider otherCircle))
        {
            if (otherCircle.IsCircleCollidingCircle(myColl))
            {
                dir = myColl.SolveDynamicCircleWithStaticCircle(otherCircle, dir);
            }
        }
    }

    
}
