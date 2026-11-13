using UnityEngine;
using UnityEngine.InputSystem; // Make sure to include this namespace!

public class InputsTesting : MonoBehaviour
{
    [SerializeField] private InputActionReference btnAction_ShootG;
    [SerializeField] private InputActionReference btnAction_ShootP;
    public BulletPooling bulletPool_Green;
    public BulletPooling bulletPool_Pink;
    public Transform shootPoint_G;
    public Transform shootPoint_P;

    [Header("Sonidos")]
    [SerializeField] private AudioClip shootGreenClip;
    [SerializeField] private AudioClip shootPinkClip;
    [SerializeField][Range(0f, 1f)] private float shootVolume = 0.8f;

    private void OnEnable()
    {
        btnAction_ShootG.action.performed += Pressed_RTrigger;
        btnAction_ShootP.action.performed += Pressed_LTrigger;
    }

    private void OnDisable()
    {
        btnAction_ShootG.action.performed -= Pressed_RTrigger;
        btnAction_ShootP.action.performed -= Pressed_LTrigger;
    }

    private void Pressed_RTrigger(InputAction.CallbackContext context)
    {
        bulletPool_Green.GetObject(shootPoint_G);
        PlayShot(shootGreenClip, shootPoint_G);
    }

    private void Pressed_LTrigger(InputAction.CallbackContext context)
    {
        bulletPool_Pink.GetObject(shootPoint_P);
        PlayShot(shootPinkClip, shootPoint_P);
    }

    // El sonido sale de la mano que dispara
    private void PlayShot(AudioClip clip, Transform point)
    {
        if (clip != null) AudioSource.PlayClipAtPoint(clip, point.position, shootVolume);
    }
}
