using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatorManager : MonoBehaviour
{
    public static InstantiatorManager Instance { get; private set; }
    [SerializeField] private List<MyBehavior> behaviors = new List<MyBehavior>();
    private Dictionary<MyBehaviorType, MyBehavior> behaviorDictionary = new Dictionary<MyBehaviorType, MyBehavior>();
    public BulletPool bulletPool;

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
        bulletPool = new BulletPool(MyBehaviorType.Bullet);
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

    public Entity CreateAddressable(MyBehaviorType behaviorType)
    {
        behaviorDictionary.TryGetValue(behaviorType, out MyBehavior myBehavior);
        Entity newEntity = myBehavior.CreateEntity();
        newEntity.EntityGameObject = Instantiate(myBehavior.prefab);
        return newEntity;
    }
}
