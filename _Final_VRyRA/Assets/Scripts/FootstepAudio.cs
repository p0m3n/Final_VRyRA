using UnityEngine;

// Pasos del jugador: suena un paso cada cierta distancia caminada.
// Script aparte (no dentro de CharacterMovement) para que cada clase tenga una sola responsabilidad.
[RequireComponent(typeof(CharacterController))]
public class FootstepAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float stepDistance = 0.7f;
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;

    private CharacterController controller;
    private float distanceSinceLastStep;
    private int lastClipIndex = -1;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Sin pasos si está reapareciendo o si está en el aire (cayendo al abismo)
        if (!controller.enabled || !controller.isGrounded) return;

        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;

        distanceSinceLastStep += horizontalVelocity.magnitude * Time.deltaTime;

        if (distanceSinceLastStep >= stepDistance)
        {
            distanceSinceLastStep = 0f;
            PlayStep();
        }
    }

    private void PlayStep()
    {
        if (footstepClips == null || footstepClips.Length == 0) return;

        // Elige un clip distinto al anterior y varía el tono: así no suena repetitivo
        int index = Random.Range(0, footstepClips.Length);
        if (footstepClips.Length > 1 && index == lastClipIndex)
            index = (index + 1) % footstepClips.Length;

        lastClipIndex = index;
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(footstepClips[index]);
    }
}
