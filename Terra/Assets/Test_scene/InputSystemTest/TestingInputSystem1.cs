using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Prędkość ruchu
    private Vector2 moveInput;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Pobranie Rigidbody dla sfery
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Odczyt wartości wejścia (Vector2)
        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Przekształć ruch w przestrzeń 3D (X, 0, Z)
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        rb.velocity = moveDirection * speed;
    }
}