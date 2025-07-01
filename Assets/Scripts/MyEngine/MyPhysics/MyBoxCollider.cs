using Unity.VisualScripting;
using UnityEngine;

public class MyBoxCollider
{
    [SerializeField] private float width;
    [SerializeField] private float height;
    private GameObject entity;
    public GameObject Entity => entity;

    public Vector2 position => Entity.transform.position;
    public float Width { get => width; }
    public float Height { get => height; }
    public float boxRight;
    public float boxLeft;
    public float boxBottom;
    public float boxTop;
    public float halfWidth { get; private set; }
    public float halfHeight { get; private set; }

    public MyBoxCollider(float width, float height, GameObject entity)
    {
        this.width = width;
        this.height = height;
        this.entity = entity;

        halfWidth = width * 0.5f;
        halfHeight = height * 0.5f;
        boxLeft = position.x - halfWidth;
        boxRight = position.x + halfWidth;
        boxBottom = position.y - halfHeight;
        boxTop = position.y + halfHeight;
    }

    public void ChangeBoxDimensions(float width, float height)
    {
       this.width = width;
       this.height = height;
    }

}
