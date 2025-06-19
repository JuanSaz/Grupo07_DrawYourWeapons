using UnityEngine;


public class PlayerEntity: Entity, IUpdatable
{
    public SO_PlayerInput inputs;
    private Rigidbody2D rb;
    private Vector2 dir = new Vector2(0,1);
    private float movementSpeed = 5.0f;
    private float rotationSpeed = 10.0f;
    public PlayerEntity() { }

    public override void WakeUp()
    {
        base.WakeUp();

        rb = InstantiatorManager.Instance.GetComponentFrom<Rigidbody2D>(EntityGameObject);
        Debug.Log(rb);
        UpdateManager.Instance.Subscribe(this);
    }
    public void UpdateMe(float deltaTime)
    {
        Movement();
    }
    private void Movement()
    {
        float moveInput = Input.GetAxis("p1V");
        float rotateInput = -Input.GetAxis("p1H");

        EntityGameObject.transform.Rotate(0f, 0f, rotateInput * rotationSpeed * Time.deltaTime);
        EntityGameObject.transform.position += EntityGameObject.transform.up * (moveInput * movementSpeed * Time.deltaTime);
    }

}
