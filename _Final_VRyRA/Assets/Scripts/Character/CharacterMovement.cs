using Meta.XR.ImmersiveDebugger.UserInterface;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference btnAction_Move;
    [SerializeField] private float moveSpeed;
    private CharacterController controller;

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
        Vector2 inputVector = btnAction_Move.action.ReadValue<Vector2>();
        Vector3 move = (transform.right * inputVector.x) + (transform.forward * inputVector.y);

        controller.Move(move * moveSpeed * Time.deltaTime);
    }
}
