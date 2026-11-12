using System.Collections;
using UnityEngine;

// Va en el mismo objeto que el CharacterController del jugador.
// Escucha los checkpoints y, cuando algo le pide reaparecer, hace el fundido y lo teletransporta.
public class PlayerRespawn : MonoBehaviour, IRespawnable
{
    [SerializeField] private ScreenFader screenFader;
    [SerializeField] private Transform startPoint;
    [SerializeField] private float blackTime = 0.3f;

    private CharacterController controller;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private bool isRespawning;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        SetSpawn(startPoint != null ? startPoint : transform);
    }

    // Suscribirse y desuscribirse del evento: si no se desuscribe,
    // un evento static seguiría apuntando a este objeto aunque se destruya
    private void OnEnable()
    {
        Checkpoint.OnCheckpointReached += SetSpawn;
    }

    private void OnDisable()
    {
        Checkpoint.OnCheckpointReached -= SetSpawn;
    }

    private void SetSpawn(Transform point)
    {
        spawnPosition = point.position;
        // Solo el giro horizontal: nunca inclinar la vista del jugador en VR
        spawnRotation = Quaternion.Euler(0f, point.eulerAngles.y, 0f);
    }

    public void Respawn()
    {
        if (isRespawning) return;
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        isRespawning = true;

        if (screenFader != null) yield return screenFader.FadeOut();

        // El CharacterController ignora los cambios de posición mientras está activo:
        // hay que apagarlo, mover al jugador y volver a prenderlo
        controller.enabled = false;
        transform.SetPositionAndRotation(spawnPosition, spawnRotation);
        controller.enabled = true;

        yield return new WaitForSeconds(blackTime);

        if (screenFader != null) yield return screenFader.FadeIn();

        isRespawning = false;
    }
}
