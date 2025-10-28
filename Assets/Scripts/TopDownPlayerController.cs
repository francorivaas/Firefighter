// TopDownPlayerController.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDownPlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [Tooltip("Velocidad de movimiento (unidades por segundo)")]
    public float moveSpeed = 5f;

    [Tooltip("¿Usar física para mover (MovePosition) o cambiar transform? Recomiendo true.")]
    public bool usePhysicsMovement = true;

    [Header("Rotación")]
    [Tooltip("Suavizado de la rotación. 0 = sin suavizado, valores mayores suavizan la rotación.")]
    [Range(0f, 20f)]
    public float rotationSmoothing = 8f;

    // Componentes
    Rigidbody2D rb;
    Camera mainCam;

    // Datos de estado
    Vector2 movementInput;
    Vector2 mouseWorldPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;

        if (mainCam == null)
            Debug.LogWarning("No se encontró Camera.main. Asegurate que la cámara principal tenga la etiqueta 'MainCamera'.");
    }

    void Update()
    {
        // Entrada de movimiento (WASD / flechas / joystick en ejes horizontales/verticales)
        movementInput.x = Input.GetAxisRaw("Horizontal"); // -1, 0, 1
        movementInput.y = Input.GetAxisRaw("Vertical");

        // Normalizar para evitar movimiento más rápido en diagonales
        if (movementInput.sqrMagnitude > 1f)
            movementInput.Normalize();

        // Obtener posición del mouse en mundo
        if (mainCam != null)
        {
            Vector3 mouseScreen = Input.mousePosition;
            Vector3 mouseWorld3 = mainCam.ScreenToWorldPoint(mouseScreen);
            mouseWorldPos = new Vector2(mouseWorld3.x, mouseWorld3.y);
        }
    }

    void FixedUpdate()
    {
        // --- Movimiento ---
        Vector2 currentPos = rb.position;
        Vector2 targetPos = currentPos + movementInput * moveSpeed * Time.fixedDeltaTime;

        if (usePhysicsMovement)
        {
            rb.MovePosition(targetPos);
        }
        else
        {
            rb.position = targetPos;
        }

        // --- Rotación: apuntar hacia el mouse ---
        Vector2 direction = mouseWorldPos - rb.position;
        if (direction.sqrMagnitude > 0.0001f)
        {
            // angle en grados. Ajusta el offset si tu sprite mira en otra dirección por defecto.
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // Muchas sprites "arriba" es +Y; si el sprite mira "up" por defecto, restá 90 grados.
            float desiredAngle = angle - 90f;

            if (rotationSmoothing <= 0f)
            {
                rb.MoveRotation(desiredAngle);
            }
            else
            {
                float smoothed = Mathf.LerpAngle(rb.rotation, desiredAngle, rotationSmoothing * Time.fixedDeltaTime);
                rb.MoveRotation(smoothed);
            }
        }
    }

    // Método público para cambiar velocidad desde otros scripts (opcional)
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}

