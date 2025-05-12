using Unity.VisualScripting;
using UnityEngine;

public class MyBoxCollider : MonoBehaviour
{
    private float width;
    private float height;

    public Vector2 position => transform.position;
    public float Width { get => width; }
    public float Height { get => height; }

    [HideInInspector] public float halfWidth;
    [HideInInspector] public float halfHeight;
    private void Awake()
    {
        width = transform.localScale.x;
        height = transform.localScale.y;

        halfWidth = width * 0.5f;
        halfHeight = height * 0.5f;
    }

    private void OnDrawGizmos()
    {
        // Dibujar la caja en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }
    public void ChangeBoxDimensions(float width, float height)
    {
       this.width = width;
       this.height = height;
    }

}
