using Unity.VisualScripting;
using UnityEngine;

public class MyCircleCollider : MonoBehaviour
{
    [SerializeField] private float radius;
    public float Radius { get => radius;}

    private void Awake()
    {
        TryGetComponent<CircleCollider2D>(out CircleCollider2D circleCollider);
        ChangeRadius(circleCollider.radius);
    }
    public bool IsCircleCollidingCircle(MyCircleCollider otherCircle)
    {
        Vector2 sub = new Vector2(otherCircle.transform.position.x - transform.position.x, otherCircle.transform.position.y - transform.position.y);
        float centerDistance = sub.magnitude;

        return centerDistance < radius + otherCircle.radius;
    }
    public void SolveCircleCollidingStaticCircle(MyCircleCollider otherCircle)
    {
        Vector2 sub = new Vector2(otherCircle.transform.position.x - transform.position.x, otherCircle.transform.position.y - transform.position.y);
        sub.Normalize();
        Vector3 dir = new Vector3(-sub.x,-sub.y, 0);
        transform.position = (otherCircle.transform.position + (dir *(radius + otherCircle.radius)));
    }


    public void ChangeRadius(float rad)
    {
        radius = rad;
    }

}
