using UnityEngine;

public class BorderEntity : Entity, ICollidable
{
    public Entity CollidableEntity => this;
    public MyCircleCollider MyCircleCollider => null;

    private MyBoxCollider boxCollider;
    public MyBoxCollider MyBoxCollidier => boxCollider;

    float wallHeight = 1f;
    float wallWidth = 23f;

    public override void WakeUp()
    {
        base.WakeUp();
        boxCollider = new MyBoxCollider(wallWidth, wallHeight, EntityGameObject);
        GameManager.Instance.SetWallCollidable(this, true);
    }
}
