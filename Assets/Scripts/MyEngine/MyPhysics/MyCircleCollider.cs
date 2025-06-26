using UnityEngine;

public class MyCircleCollider
{
    private float radius;
    private GameObject entity;
    public float Radius => radius;
    Vector2 wallCollisionNormal;

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


    public void ChangeRadius(float rad)
    {
        radius = rad;
    }

}
