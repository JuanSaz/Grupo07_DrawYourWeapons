using UnityEngine;

public class MyBoxCollider
{
    private float width;
    private float height;
    private GameObject entity;

    public Vector2 position => entity.transform.position;
    public float Width { get => width; }
    public float Height { get => height; }
    public float boxRight;
    public float boxLeft;
    public float boxBottom;
    public float boxTop;
    public float halfWidth { get; private set; }
    public float halfHeight { get; private set; }
    Vector2 wallCollisionNormal;

    public MyBoxCollider(float width, float height, GameObject entity)
    {
        this.width = width;
        this.height = height;
        this.entity = entity;

        ChangeBoxDimensions(entity.transform.localScale.x, entity.transform.localScale.y);

        halfWidth = width * 0.5f;
        halfHeight = height * 0.5f;
        boxLeft = position.x - halfWidth;
        boxRight = position.x + halfWidth;
        boxBottom = position.y - halfHeight;
        boxTop = position.y + halfHeight;
    }

    public bool IsBoxCollidingCircle(MyCircleCollider circle)
    {
        float closestX = Mathf.Clamp(circle.Entity.transform.position.x, boxLeft, boxRight);
        float closestY = Mathf.Clamp(circle.Entity.transform.position.y, boxBottom, boxTop);

        float distanceX = circle.Entity.transform.position.x - closestX;
        float distanceY = circle.Entity.transform.position.y - closestY;
        float distanceSquared = distanceX * distanceX + distanceY * distanceY;

        return distanceSquared < (circle.Radius * circle.Radius);
    }

    public void SolveWithCircle(MyCircleCollider circle)
    {
        float closestX = Mathf.Clamp(circle.Entity.transform.position.x, boxLeft, boxRight);
        float closestY = Mathf.Clamp(circle.Entity.transform.position.y, boxBottom, boxTop);
        Vector2 closestPoint = new Vector2(closestX, closestY);

        Vector2 collisionVector = new Vector2(circle.Entity.transform.position.x - closestPoint.x, circle.Entity.transform.position.y - closestPoint.y);
        float distance = collisionVector.magnitude;

        if (distance < circle.Radius && distance != 0f)
        {
            float penetrationDepth = circle.Radius - distance;
            Vector2 collisionNormal = collisionVector.normalized;

            circle.Entity.transform.position += new Vector3(collisionNormal.x, collisionNormal.y, 0f) * penetrationDepth;
        }
    }

    //public void SolveWithStaticBox(MyBoxCollider box)
    //{
    //    Vector3 newPosition = HandleBoxCollision(box);
    //    entity.transform.position = newPosition;
    //}

    //public Vector2 ProjectCircleOntoLine(MyBoxCollider otherBox, Vector2 bulletDir)
    //{
    //    Vector3 newPosition = HandleBoxCollision(otherBox);
    //    entity.transform.position = newPosition;

    //    float dot = Vector2.Dot(bulletDir, wallCollisionNormal);
    //    return bulletDir - 2 * dot * wallCollisionNormal; //Mantengo el mismo sentido de la dirección paralela e invierto la perpendicular a la normal
    //}

    //private Vector2 HandleBoxCollision(MyBoxCollider box)
    //{
    //    Vector3 position = entity.transform.position;
    //    Vector2 boxPos = box.position;

    //    // Calculamos las distancias a cada borde de la caja
    //    float leftPenetration = entity.transform.position.x - (box.boxLeft - radius);
    //    float rightPenetration = (box.boxRight + radius) - entity.transform.position.x;
    //    float bottomPenetration = entity.transform.position.y - (box.boxBottom - radius);
    //    float topPenetration = (box.boxTop + radius) - entity.transform.position.y;

    //    //Elige donde hay más penetración de las cuatro paredes para determinar donde chocó                                                                                                  
    //    float minPenetration = Mathf.Min(leftPenetration, rightPenetration, bottomPenetration, topPenetration);

    //    wallCollisionNormal = Vector2.zero; //Normal del lado que golpeó

    //    if (minPenetration == leftPenetration)
    //    {
    //        wallCollisionNormal = Vector2.left;
    //        position.x = box.boxLeft - radius;
    //    }
    //    else if (minPenetration == rightPenetration)
    //    {
    //        wallCollisionNormal = Vector2.right;
    //        position.x = box.boxRight + radius;
    //    }
    //    else if (minPenetration == bottomPenetration)
    //    {
    //        wallCollisionNormal = Vector2.down;
    //        position.y = box.boxBottom - radius;
    //    }
    //    else
    //    {
    //        wallCollisionNormal = Vector2.up;
    //        position.y = box.boxTop + radius;
    //    }

    //    return position; //Devuelve la posición "acomodada"
    //}

    public void ChangeBoxDimensions(float width, float height)
    {
       this.width = width;
       this.height = height;
    }

}
