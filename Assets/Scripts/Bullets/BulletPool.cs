using System;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;

    public ObjectPool<Bullet> pool;

    private void Awake()
    {
        pool = new ObjectPool<Bullet>(CreatePoolItem, OnTakeFromPool, OnReturnedFromPool,OnDestroyPoolObject, false, 8 , 40);
    }

    private void OnDestroyPoolObject(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    private void OnReturnedFromPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnTakeFromPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private Bullet CreatePoolItem()
    {
        Bullet bullet = Instantiate(bulletPrefab);
        bullet.gameObject.SetActive(false);
        bullet.pool = pool;
        return bullet;
    }
}
