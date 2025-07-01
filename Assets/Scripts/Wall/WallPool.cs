using UnityEngine;
using UnityEngine.Pool;

public class WallPool 
{
    public ObjectPool<WallEntity> pool;

    public WallPool(MyBehaviorType bulletBeh)
    {
        pool = new ObjectPool<WallEntity>(CreatePoolItem, OnTakeFromPool, OnReturnedFromPool, OnDestroyPoolObject, false, 5, 10);
    }

    private WallEntity CreatePoolItem()
    {
        WallEntity wall = (WallEntity)InstantiatorManager.Instance.Create(MyBehaviorType.Wall);
        wall.EntityGameObject.SetActive(false);
        return wall;
    }

    private void OnTakeFromPool(WallEntity wall)
    {
        wall.EntityGameObject.SetActive(true);
        GameManager.Instance.ActiveWallsColls.Add(wall);
    }

    private void OnReturnedFromPool(WallEntity wall)
    {
        wall.EntityGameObject.SetActive(false);
        GameManager.Instance.ActiveWallsColls.Remove(wall);
    }

    private void OnDestroyPoolObject(WallEntity wall)
    {
        //GameObject.Destroy(obj);
    }
}
