using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MyCircleCollider : MonoBehaviour
{
    [SerializeField] private float radius;
    public float Radius => radius;
    Vector2 wallCollisionNormal;


    private void Awake()
    {
        TryGetComponent<CircleCollider2D>(out CircleCollider2D circleCollider);
        ChangeRadius(circleCollider.radius);
    }
    private void OnDrawGizmos()
    {
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

    public Vector2 SolveDynamicCircleWithStaticCircle(MyCircleCollider otherCircle, Vector2 bulletDir)
    {
        Vector2 normal = new Vector2(transform.position.x - otherCircle.transform.position.x, transform.position.y - otherCircle.transform.position.y);

        Vector2 newPosition = ((Vector2)otherCircle.transform.position + (normal * (radius + otherCircle.radius)));

        transform.position = newPosition;

        float dot = Vector2.Dot(bulletDir, normal);

        return normal * (dot * 2); 
    }
    public void SolveWithStaticBox(MyBoxCollider box)
    {
        Vector3 newPosition = HandleBoxCollision(box); 
        transform.position = newPosition;
    }

    public Vector2 ProjectCircleOntoLine(MyBoxCollider otherBox, Vector2 bulletDir)
    {
        
        Vector3 newPosition = HandleBoxCollision(otherBox);
        transform.position = newPosition;

        float dot = Vector2.Dot(bulletDir, wallCollisionNormal);
        return bulletDir - 2 * dot * wallCollisionNormal; //Mantengo el mismo sentido de la dirección paralela e invierto la perpendicular a la normal
    }

    private Vector2 HandleBoxCollision(MyBoxCollider box)
    {
        Vector3 position = transform.position;
        Vector2 boxPos = box.position;

        // Calculamos las distancias a cada borde de la caja
        float leftPenetration = transform.position.x - (box.boxLeft - radius);
        float rightPenetration = (box.boxRight + radius) - transform.position.x;
        float bottomPenetration = transform.position.y - (box.boxBottom - radius);
        float topPenetration = (box.boxTop + radius) - transform.position.y;

        //Elige donde hay más penetración de las cuatro paredes para determinar donde chocó                                                                                                  
        float minPenetration = Mathf.Min(leftPenetration, rightPenetration, bottomPenetration, topPenetration); 


        wallCollisionNormal = Vector2.zero; //Normal del lado que golpeó

        if (minPenetration == leftPenetration)
        {
            wallCollisionNormal = Vector2.left;
            position.x = box.boxLeft - radius;
        }
        else if (minPenetration == rightPenetration)
        {
            wallCollisionNormal = Vector2.right;
            position.x = box.boxRight + radius;
        }
        else if (minPenetration == bottomPenetration)
        {
            wallCollisionNormal = Vector2.down;
            position.y = box.boxBottom - radius;
        }
        else 
        {
            wallCollisionNormal = Vector2.up;
            position.y = box.boxTop + radius;
        }

        return position; //Devuelve la posición "acomodada"
    }


    public void ChangeRadius(float rad)
    {
        radius = rad;
    }

}
