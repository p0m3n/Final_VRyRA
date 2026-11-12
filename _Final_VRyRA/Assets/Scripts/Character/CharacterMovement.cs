using Meta.XR.ImmersiveDebugger.UserInterface;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference btnAction_Move;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private float verticalVelocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        btnAction_Move.action.Enable();
    }
    private void OnDisable()
    {
        btnAction_Move.action.Disable();
    }

    private void Update()
    {
        // Mientras reaparece, PlayerRespawn apaga el controller: no moverlo
        if (!controller.enabled) return;

        Vector2 inputVector = btnAction_Move.action.ReadValue<Vector2>();
        Vector3 move = (transform.right * inputVector.x) + (transform.forward * inputVector.y);

        // Gravedad: sin esto el jugador camina sobre el vacío
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f; // pequeño empuje hacia abajo para mantenerlo pegado al suelo

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * moveSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }
}
