using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MyCircleCollider : MonoBehaviour
{
    [SerializeField] private float radius;
    public float Radius { get => radius; }

    private void Awake()
    {
        TryGetComponent<CircleCollider2D>(out CircleCollider2D circleCollider);
        ChangeRadius(circleCollider.radius);
    }
    private void OnDrawGizmos()
    {
        // Dibujar el círculo en el editor
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
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
    public void SolveWithBox(MyBoxCollider box)
    {
        float collisionPointX = transform.position.x;
        float collisionPointY = transform.position.y;

        float minX = box.position.x - box.halfWidth + radius;
        float maxX = box.position.x + box.halfWidth - radius;
        float minY = box.position.y - box.halfHeight + radius;
        float maxY = box.position.y + box.halfHeight - radius;

        if (collisionPointX < minX)
        {
            transform.position = new Vector3(minX, collisionPointY, transform.position.z);
        }
        else if (collisionPointX > maxX)
        {
            transform.position = new Vector3(maxX, collisionPointY, transform.position.z);
        }

        if (collisionPointY < minY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        }
        else if (collisionPointY > maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        }
    }
    public void ChangeRadius(float rad)
    {
        radius = rad;
    }

}
