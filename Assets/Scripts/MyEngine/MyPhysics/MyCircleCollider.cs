using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MyCircleCollider : MonoBehaviour
{
    [SerializeField] private float radius;
    public float Radius => radius;

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
    public void SolveWithStaticBox(MyBoxCollider box)
    {
        Vector2 normal;
        Vector3 newPosition = HandleBoxCollision(box, out normal);
        transform.position = newPosition;
    }

    public Vector2 ProjectCircleOntoLine(MyBoxCollider otherBox, Vector2 bulletDir)
    {
        Vector2 normal;
        Vector3 newPosition = HandleBoxCollision(otherBox, out normal);
        transform.position = newPosition;

        float dot = Vector2.Dot(bulletDir, normal);
        return bulletDir - 2 * dot * normal;
    }

    private Vector2 HandleBoxCollision(MyBoxCollider box, out Vector2 normal)
    {
        Vector3 position = transform.position;
        Vector2 boxPos = box.position;

        // Calculamos las distancias a cada borde de la caja
        float leftPenetration = transform.position.x - (box.boxLeft - radius);
        float rightPenetration = (box.boxRight + radius) - transform.position.x;
        float bottomPenetration = transform.position.y - (box.boxBottom - radius);
        float topPenetration = (box.boxTop + radius) - transform.position.y;

        float minPenetration = Mathf.Min(leftPenetration, rightPenetration, bottomPenetration, topPenetration);


        normal = Vector2.zero;

        if (minPenetration == leftPenetration)
        {
            normal = Vector2.left;
            position.x = box.boxLeft - radius;
        }
        else if (minPenetration == rightPenetration)
        {
            normal = Vector2.right;
            position.x = box.boxRight + radius;
        }
        else if (minPenetration == bottomPenetration)
        {
            normal = Vector2.down;
            position.y = box.boxBottom - radius;
        }
        else 
        {
            normal = Vector2.up;
            position.y = box.boxTop + radius;
        }

        return position;
    }


    public void ChangeRadius(float rad)
    {
        radius = rad;
    }

}
