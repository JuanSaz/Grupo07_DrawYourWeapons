using Unity.VisualScripting;
using UnityEngine;

public class MyBoxCollider : MonoBehaviour
{
    [SerializeField] private float width;
    [SerializeField] private float height;

    public Vector2 position => transform.position;
    public float Width { get => width; }
    public float Height { get => height; }
    public float boxRight;
    public float boxLeft;
    public float boxBottom;
    public float boxTop;
    public float halfWidth { get; private set; }
    public float halfHeight { get; private set; }
    private void Awake()
    {
        ChangeBoxDimensions(width, height);

        halfWidth = width * 0.5f;
        halfHeight = height * 0.5f;
        boxLeft = position.x - halfWidth;
        boxRight = position.x + halfWidth;
        boxBottom = position.y - halfHeight;
        boxTop = position.y + halfHeight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }
    public void ChangeBoxDimensions(float width, float height)
    {
       this.width = width;
       this.height = height;
    }

}
