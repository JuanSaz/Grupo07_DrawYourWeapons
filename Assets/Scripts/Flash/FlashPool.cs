using System;
using UnityEngine;
using UnityEngine.Pool;
public class FlashPool
{
    private MyBehaviorType flashBehaviour;

    public ObjectPool<FlashEntity> pool;

    public FlashPool(MyBehaviorType flashBeh)
    {
        flashBehaviour = flashBeh;
        pool = new ObjectPool<FlashEntity>(CreatePoolItem, OnTakeFromPool, OnReturnedFromPool, OnDestroyPoolObject, false, 8, 40);
    }

    private void OnDestroyPoolObject(FlashEntity flash)
    {
        //Destroy(bullet.gameObject);
    }

    private void OnReturnedFromPool(FlashEntity flash)
    {
        flash.EntityGameObject.SetActive(false);
    }

    private void OnTakeFromPool(FlashEntity flash)
    {
        flash.EntityGameObject.SetActive(true);
        UpdateManager.Instance.Subscribe(flash);
        LevelManager.Instance.OnRoundRestart.AddListener(flash.Reset);
    }

    private FlashEntity CreatePoolItem()
    {
        FlashEntity flash = (FlashEntity)InstantiatorManager.Instance.Create(flashBehaviour);
        flash.WakeUp();
        flash.EntityGameObject.SetActive(false);
        return flash;
    }
}
