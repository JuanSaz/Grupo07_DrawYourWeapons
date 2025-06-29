using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum MyBehaviorType //SOLO TENER UN SO POR TIPO, Al agregar a la lista, Reimportar Todos Los Assets
    //Cuidado, require reinicio de programa y capaz se desarman todos los SO!
{
    Player1, Player2, Player3,Player4,
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
