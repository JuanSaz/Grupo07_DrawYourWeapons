using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public GameObject EntityGameObject;
    public Dictionary<Type,object> MyComponents = new Dictionary<Type,object>();
    public MonoBehaviour monoRef;

    public Entity() { }
    public virtual void WakeUp()
    {

    }
}
