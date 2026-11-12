using System;
using UnityEngine;

// Zona invisible que guarda el punto de reaparición cuando el jugador la cruza.
// Usa un evento de C# (patrón Observer): el checkpoint avisa y no sabe quién escucha.
public class Checkpoint : MonoBehaviour
{
    public static event Action<Transform> OnCheckpointReached;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        // Si no se asigna un spawnPoint, usa la posición del propio checkpoint
        Transform point = spawnPoint != null ? spawnPoint : transform;
        OnCheckpointReached?.Invoke(point);
    }
}
