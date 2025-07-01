using UnityEngine;

public class WallEntity : Entity, ICollidable
{
    public Entity CollidableEntity => this;
    public MyCircleCollider MyCircleCollider => null;

    private MyBoxCollider boxCollider;
    public MyBoxCollider MyBoxCollidier => boxCollider;

    float wallHeight = 4f;
    float wallWidth = 0.5f;


    public override void WakeUp()
    {
        base.WakeUp();
        boxCollider = new MyBoxCollider(wallWidth, wallHeight, EntityGameObject);
        GameManager.Instance.SetWallCollidable(this, true);
    }
}
