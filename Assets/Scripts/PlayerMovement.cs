using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float moveSpeed;
    [SerializeField] float maxSpeed;

    Vector3 movement;
    float moveX;
    float moveY;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        movement = new Vector3(moveX, 0, moveY).normalized;

        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);

        rb.AddForce(movement * moveSpeed, ForceMode.Impulse);
    }

    void OnMove(InputValue MoveValue)
    {
        Vector2 moveVector = MoveValue.Get<Vector2>().normalized;

        moveX = moveVector.x;
        moveY = moveVector.y;
    }
}
