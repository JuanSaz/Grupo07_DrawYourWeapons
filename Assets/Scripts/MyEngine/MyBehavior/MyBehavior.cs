using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum MyBehaviorType //SOLO TENER UN SO POR TIPO
{
    Player,
    Bullet
}

public class MyBehavior : ScriptableObject
{
    [SerializeField] public MyBehaviorType type;
    [SerializeField] public GameObject prefab;
    public Entity entity;


    public virtual Entity CreateEntity()
    {
        return null;
    }
}
