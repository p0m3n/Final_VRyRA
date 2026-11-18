//using Meta.XR.ImmersiveDebugger.UserInterface;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class CharacterMovement : MonoBehaviour
//{
//    [SerializeField] private InputActionReference btnAction_Move;
//    [SerializeField] private float moveSpeed;
//    [SerializeField] private float gravity = -9.81f;

//    private CharacterController controller;
//    private float verticalVelocity;

//    private void Start()
//    {
//        controller = GetComponent<CharacterController>();
//    }

//    private void OnEnable()
//    {
//        btnAction_Move.action.Enable();
//    }
//    private void OnDisable()
//    {
//        btnAction_Move.action.Disable();
//    }

//    private void Update()
//    {
//        // Mientras reaparece, PlayerRespawn apaga el controller: no moverlo
//        if (!controller.enabled) return;

//        Vector2 inputVector = btnAction_Move.action.ReadValue<Vector2>();
//        Vector3 move = (transform.right * inputVector.x) + (transform.forward * inputVector.y);

//        // Gravedad: sin esto el jugador camina sobre el vacío
//        if (controller.isGrounded && verticalVelocity < 0f)
//            verticalVelocity = -2f; // pequeño empuje hacia abajo para mantenerlo pegado al suelo

//        verticalVelocity += gravity * Time.deltaTime;

//        Vector3 velocity = move * moveSpeed;
//        velocity.y = verticalVelocity;

//        controller.Move(velocity * Time.deltaTime);
//    }
//}


using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference btnAction_Move;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private float verticalVelocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform;
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

        // Así, si miras al suelo o al techo, no intentas caminar hacia abajo o hacia arriba.
        Quaternion headYaw = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
        Vector3 forward = headYaw * Vector3.forward; // adelante de la cabeza, plano
        Vector3 right = headYaw * Vector3.right;     // derecha de la cabeza, plana

        Vector3 move = (right * inputVector.x) + (forward * inputVector.y);
        move = Vector3.ClampMagnitude(move, 1f);

        // Gravedad: sin esto el jugador camina sobre el vacío
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f; // pequeño empuje hacia abajo para mantenerlo pegado al suelo

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * moveSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }
}
