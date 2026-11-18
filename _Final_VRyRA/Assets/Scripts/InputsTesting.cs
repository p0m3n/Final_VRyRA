//using UnityEngine;
//using UnityEngine.InputSystem; // Make sure to include this namespace!

//public class InputsTesting : MonoBehaviour
//{
//    [SerializeField] private InputActionReference btnAction_ShootG;
//    [SerializeField] private InputActionReference btnAction_ShootP;
//    public BulletPooling bulletPool_Green;
//    public BulletPooling bulletPool_Pink;
//    public Transform shootPoint_G;
//    public Transform shootPoint_P;

//    [Header("Sonidos")]
//    [SerializeField] private AudioClip shootGreenClip;
//    [SerializeField] private AudioClip shootPinkClip;
//    [SerializeField][Range(0f, 1f)] private float shootVolume = 0.8f;

//    private void OnEnable()
//    {
//        btnAction_ShootG.action.performed += Pressed_RTrigger;
//        btnAction_ShootP.action.performed += Pressed_LTrigger;

//        btnAction_ShootG.action.Enable();
//        btnAction_ShootP.action.Enable();
//    }

//    private void OnDisable()
//    {
//        btnAction_ShootG.action.performed -= Pressed_RTrigger;
//        btnAction_ShootP.action.performed -= Pressed_LTrigger;
//        btnAction_ShootG.action.Disable();
//        btnAction_ShootP.action.Disable();
//    }


//    // El sonido sale de la mano que dispara
//    private void PlayShot(AudioClip clip, Transform point)
//    {
//        if (clip != null) AudioSource.PlayClipAtPoint(clip, point.position, shootVolume);
//    }

//    private void Pressed_RTrigger(InputAction.CallbackContext context)
//    {
//        Debug.Log("GATILLO VERDE recibido"); // TEMPORAL: para probar en el lente
//        bulletPool_Green.GetObject(shootPoint_G);
//        PlayShot(shootGreenClip, shootPoint_G);
//    }

//    private void Pressed_LTrigger(InputAction.CallbackContext context)
//    {
//        Debug.Log("GATILLO ROSA recibido"); // TEMPORAL
//        bulletPool_Pink.GetObject(shootPoint_P);
//        PlayShot(shootPinkClip, shootPoint_P);
//    }
//}



using System.Collections; // TEMPORAL: para el Start de diagnóstico
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

        btnAction_ShootG.action.Enable();
        btnAction_ShootP.action.Enable();
    }

    private void OnDisable()
    {
        btnAction_ShootG.action.performed -= Pressed_RTrigger;
        btnAction_ShootP.action.performed -= Pressed_LTrigger;
        btnAction_ShootG.action.Disable();
        btnAction_ShootP.action.Disable();
    }

    // TEMPORAL: diagnóstico para el lente. Espera 5 s a que se conecten los controles
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);

        Debug.Log("GATILLO verde activo: " + btnAction_ShootG.action.enabled
            + " | controles encontrados: " + btnAction_ShootG.action.controls.Count);

        foreach (InputDevice device in InputSystem.devices)
            Debug.Log("GATILLO dispositivo: " + device.name
                + " | layout: " + device.layout
                + " | usos: " + string.Join(",", device.usages));
    }

    // El sonido sale de la mano que dispara
    private void PlayShot(AudioClip clip, Transform point)
    {
        if (clip != null) AudioSource.PlayClipAtPoint(clip, point.position, shootVolume);
    }

    private void Pressed_RTrigger(InputAction.CallbackContext context)
    {
        Debug.Log("GATILLO VERDE recibido"); // TEMPORAL: para probar en el lente
        bulletPool_Green.GetObject(shootPoint_G);
        PlayShot(shootGreenClip, shootPoint_G);
    }

    private void Pressed_LTrigger(InputAction.CallbackContext context)
    {
        Debug.Log("GATILLO ROSA recibido"); // TEMPORAL
        bulletPool_Pink.GetObject(shootPoint_P);
        PlayShot(shootPinkClip, shootPoint_P);
    }
}
