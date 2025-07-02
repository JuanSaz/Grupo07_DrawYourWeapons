using UnityEngine;
using UnityEngine.Pool;
public class DrawSegmentsPool
{
    public ObjectPool<DrawingEntity> pool;
    private MyBehaviorType behaviorType;

    public DrawSegmentsPool(MyBehaviorType bulletBv)
    {
        behaviorType = bulletBv;
        pool = new ObjectPool<DrawingEntity>(CreatePoolItem, OnTakeFromPool, OnReturnedFromPool, OnDestroyPoolObject, false, 5, 10);
    }

    private DrawingEntity CreatePoolItem()
    {
        DrawingEntity segment = (DrawingEntity)InstantiatorManager.Instance.Create(behaviorType);
        segment.WakeUp();
        segment.EntityGameObject.SetActive(false);
        return segment;
    }

    private void OnTakeFromPool(DrawingEntity segment)
    {
        segment.EntityGameObject.SetActive(true);
        segment.ResetTime();
        GameManager.Instance.SetDrawingCollidable(segment, true);
        UpdateManager.Instance.Subscribe(segment);

    }

    private void OnReturnedFromPool(DrawingEntity segment)
    {
        segment.EntityGameObject.SetActive(false);
        segment.EntityGameObject.transform.position = Vector3.zero;
        GameManager.Instance.SetDrawingCollidable(segment, false);
        UpdateManager.Instance.Unsubscribe(segment);
    }

    private void OnDestroyPoolObject(DrawingEntity segment)
    {
        //GameObject.Destroy(obj);
    }
}
