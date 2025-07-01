using UnityEngine;

public interface ICollidable
{
    public Entity CollidableEntity {  get;}
    public MyCircleCollider MyCircleCollider { get;}
    public MyBoxCollider MyBoxCollidier { get;}
}
