using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public GameObject EntityGameObject;
    public Dictionary<Type,object> MyComponents = new Dictionary<Type,object>();

    public Entity() { }
    public virtual void WakeUp()
    {

    }
}
