using UnityEngine;

public class MyCircleCollider
{
    private float radius;
    private GameObject entity;
    public GameObject Entity => entity;
    public float Radius => radius;

    public MyCircleCollider(float radius, GameObject entity)
    {
        this.radius = radius;
        this.entity = entity;
    }

    public bool IsCircleCollidingCircle(MyCircleCollider otherCircle)
    {
        Vector2 sub = new Vector2(otherCircle.entity.transform.position.x - entity.transform.position.x, otherCircle.entity.transform.position.y - entity.transform.position.y);
        float centerDistance = sub.magnitude;

        return centerDistance < radius + otherCircle.radius;
    }
    public void SolveCircleCollidingStaticCircle(MyCircleCollider otherCircle)
    {
        Vector2 sub = new Vector2(otherCircle.entity.transform.position.x - entity.transform.position.x, otherCircle.entity.transform.position.y - entity.transform.position.y);
        sub.Normalize();
        Vector3 dir = new Vector3(-sub.x, -sub.y, 0);
        entity.transform.position = (otherCircle.entity.transform.position + (dir * (radius + otherCircle.radius)));
    }

    public Vector2 SolveDynamicCircleWithStaticCircle(MyCircleCollider otherCircle, Vector2 bulletDir)
    {
        Vector2 normal = new Vector2(entity.transform.position.x - otherCircle.entity.transform.position.x, entity.transform.position.y - otherCircle.entity.transform.position.y);
        Vector2 newPosition = ((Vector2)otherCircle.entity.transform.position + (normal * (radius + otherCircle.radius)));
        entity.transform.position = newPosition;
        float dot = Vector2.Dot(bulletDir, normal);
        
        return normal * (dot * 2);
    }

    public void ChangeRadius(float rad)
    {
        radius = rad;
    }

}
