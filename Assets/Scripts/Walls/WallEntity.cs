using UnityEngine;

public class WallEntity : Entity, IFixUpdatable, ICollidable
{
    float wallHeight = 4f;
    float wallWidth = 0.5f;
        
    private MyBoxCollider boxCollider;
    public MyBoxCollider MyBoxCollider => boxCollider;
    public Entity CollidableEntity => throw new System.NotImplementedException();
    public MyCircleCollider MyCircleCollider => throw new System.NotImplementedException();

    public WallEntity()
    {

    }

    public override void WakeUp()
    {
        base.WakeUp();
        boxCollider = new MyBoxCollider(wallWidth, wallHeight, EntityGameObject);
        GameManager.Instance.SetWallCollidable(this, true);
        UpdateManager.Instance.FixSubscribe(this);
    }

    public void FixUpdateMe()
    {
        WallPhysics();
    }
    private void WallPhysics()
    {
        for (int i = 0; i < GameManager.Instance.ActiveWallsColls.Count; i++)
        {
            if (GameManager.Instance.ActiveWallsColls[i] == null) continue;
            for (int j = 0; j < GameManager.Instance.ActivePlayersColls.Count; j++)
            {
                if (MyBoxCollider.IsBoxCollidingCircle(GameManager.Instance.ActivePlayersColls[j].MyCircleCollider))
                {
                    MyBoxCollider.SolveWithCircle(GameManager.Instance.ActivePlayersColls[j].MyCircleCollider);
                    Debug.Log("Resolviendo colision");
                }
            }
        }
    }
}
