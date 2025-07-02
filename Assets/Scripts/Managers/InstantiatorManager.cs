using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class InstantiatorManager : MonoBehaviour
{
    public static InstantiatorManager Instance { get; private set; }
    [SerializeField] private List<MyBehavior> behaviors = new List<MyBehavior>();
    private Dictionary<MyBehaviorType, MyBehavior> behaviorDictionary = new Dictionary<MyBehaviorType, MyBehavior>();

    [SerializeField] public List<AssetReference> objectsToPreload;
    private int assetsPreloadedAmount;

    public BulletPool bulletPool;
    public FlashPool flashPool;
    public WallPool wallPool;
    public DrawSegmentsPool DrawSegmentsPool;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var behavior in behaviors)
        {
            behaviorDictionary.Add(behavior.type, behavior);
        }
    }

    public Entity Create(MyBehaviorType behaviorType)
    {
        behaviorDictionary.TryGetValue(behaviorType, out MyBehavior myBehavior);

        Entity newEntity = myBehavior.CreateEntity();
        newEntity.EntityGameObject = Instantiate(myBehavior.prefab);
        return newEntity;
    }

    public T GetComponentFrom<T>(GameObject go) where T : Component
    {
        return go.GetComponent<T>();
    }

    public void PreloadAddressables()
    {
        assetsPreloadedAmount = 0;      //Empieza el contador en cero

        foreach (AssetReference assetReference in objectsToPreload)
        {
            AsyncOperationHandle handle = assetReference.LoadAssetAsync<ScriptableObject>();
        }
    }

    public void CreatePools()
    {
        bulletPool = new BulletPool(MyBehaviorType.Bullet);
        flashPool = new FlashPool(MyBehaviorType.Flash);
        wallPool = new WallPool(MyBehaviorType.Wall);
        DrawSegmentsPool = new DrawSegmentsPool(MyBehaviorType.DrawSegment);
    }
}
