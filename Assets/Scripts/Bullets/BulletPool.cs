using System;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool
{
    private MyBehaviorType bulletBehavior;

    public ObjectPool<BulletEntity> pool;

    public BulletPool(MyBehaviorType bulletBeh) 
    {
        bulletBehavior = bulletBeh;
        pool = new ObjectPool<BulletEntity>(CreatePoolItem, OnTakeFromPool, OnReturnedFromPool, OnDestroyPoolObject, false, 8, 40);
    }

    private void OnDestroyPoolObject(BulletEntity bullet)
    {
        //Destroy(bullet.gameObject);
    }

    private void OnReturnedFromPool(BulletEntity bullet)
    {
        bullet.EntityGameObject.SetActive(false);
    }

    private void OnTakeFromPool(BulletEntity bullet)
    {
        bullet.EntityGameObject.SetActive(true);
    }

    private BulletEntity CreatePoolItem()
    {
        BulletEntity bullet = (BulletEntity)InstantiatorManager.Instance.Create(bulletBehavior);
        bullet.EntityGameObject.SetActive(false);
        bullet.pool = pool;
        return bullet;
    }
}
