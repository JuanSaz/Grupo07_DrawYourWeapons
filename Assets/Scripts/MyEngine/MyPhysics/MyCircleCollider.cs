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
    public void SolveWithStaticBox(MyBoxCollider box)
    {
        Vector3 newPosition = HandleBoxCollision(box);
        entity.transform.position = newPosition;
    }

    public Vector2 ProjectCircleOntoLine(MyBoxCollider otherBox, Vector2 bulletDir)
    {
        Vector3 newPosition = HandleBoxCollision(otherBox);
        entity.transform.position = newPosition;

        float dot = Vector2.Dot(bulletDir, wallCollisionNormal);
        return bulletDir - 2 * dot * wallCollisionNormal; //Mantengo el mismo sentido de la dirección paralela e invierto la perpendicular a la normal
    }

    public bool IsBoxCollidingCircle(MyBoxCollider box)
    {
        Vector2 circleCenter = entity.transform.position;  
        Vector2 boxCenter = box.Entity.transform.position;  

        float angle = -box.Entity.transform.eulerAngles.z * Mathf.Deg2Rad; 
        Vector2 relativePosition = circleCenter - boxCenter;

        Vector2 localCirclePos = new Vector2(
            relativePosition.x * Mathf.Cos(angle) - relativePosition.y * Mathf.Sin(angle),
            relativePosition.x * Mathf.Sin(angle) + relativePosition.y * Mathf.Cos(angle)
        );

        float clampedX = Mathf.Clamp(localCirclePos.x, -box.halfWidth, box.halfWidth);
        float clampedY = Mathf.Clamp(localCirclePos.y, -box.halfHeight, box.halfHeight);

        Vector2 closestPoint = new Vector2(clampedX, clampedY);
        float distanceSquared = (localCirclePos - closestPoint).sqrMagnitude;

        return distanceSquared <= radius * radius;
    }
    private Vector2 HandleBoxCollision(MyBoxCollider box)
    {
        Vector2 circleCenter = entity.transform.position;
        Vector2 boxCenter = box.Entity.transform.position;

        float angle = -box.Entity.transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 relativePosition = circleCenter - boxCenter;

        Vector2 localPos = new Vector2(
            relativePosition.x * Mathf.Cos(angle) - relativePosition.y * Mathf.Sin(angle),
            relativePosition.x * Mathf.Sin(angle) + relativePosition.y * Mathf.Cos(angle)
        );

        float leftPenetration = (localPos.x + radius) + box.halfWidth;
        float rightPenetration = box.halfWidth - (localPos.x - radius);
        float bottomPenetration = (localPos.y + radius) + box.halfHeight;
        float topPenetration = box.halfHeight - (localPos.y - radius);

        //Calcula cual es la colision con el lado mas cercano, a partir de ese lado se hacen los calculos de reposicion
        float minPenetration = Mathf.Min(leftPenetration, rightPenetration, bottomPenetration, topPenetration);

        Vector2 localNormal = Vector2.zero;
        Vector2 localPosCorrected = localPos;

        //Asigna normal de choque dependiendo del lado golpeado
        if (minPenetration == leftPenetration)
        {
            localNormal = Vector2.left;
            localPosCorrected.x = -box.halfWidth - radius;
        }
        else if (minPenetration == rightPenetration)
        {
            localNormal = Vector2.right;
            localPosCorrected.x = box.halfWidth + radius;
        }
        else if (minPenetration == bottomPenetration)
        {
            localNormal = Vector2.down;
            localPosCorrected.y = -box.halfHeight - radius;
        }
        else
        {
            localNormal = Vector2.up;
            localPosCorrected.y = box.halfHeight + radius;
        }

        Vector2 correctedRelativePos = new Vector2(
            localPosCorrected.x * Mathf.Cos(angle) + localPosCorrected.y * Mathf.Sin(angle),
            -localPosCorrected.x * Mathf.Sin(angle) + localPosCorrected.y * Mathf.Cos(angle)
        );

        Vector2 correctedWorldPos = boxCenter + correctedRelativePos;

        wallCollisionNormal = new Vector2(
            localNormal.x * Mathf.Cos(angle) + localNormal.y * Mathf.Sin(angle),
            -localNormal.x * Mathf.Sin(angle) + localNormal.y * Mathf.Cos(angle)
        ).normalized;

        return correctedWorldPos;
    }
    public void ChangeRadius(float rad)
    {
        radius = rad;
    }

}
