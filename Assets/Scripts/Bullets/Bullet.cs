using UnityEngine;

public class Bullet : MonoBehaviour, IUpdatable
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeSpan;
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
                Destroy(gameObject);//NO DESTRUIR USAR POOL
            }
        }
        

    }
}
