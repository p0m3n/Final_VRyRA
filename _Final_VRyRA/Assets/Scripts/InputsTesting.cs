using UnityEngine;
using UnityEngine.InputSystem; // Make sure to include this namespace!

public class InputsTesting : MonoBehaviour
{
    [SerializeField] private InputActionReference btnAction;
    public BulletPooling bulletPool;
    public Transform shootPoint;

    private void OnEnable()
    {
        btnAction.action.performed += PressedButton;
    }

    private void OnDisable()
    {
        btnAction.action.performed -= PressedButton;
    }

    private void PressedButton(InputAction.CallbackContext context)
    {
        bulletPool.GetObject(shootPoint);
    }
}
