using UnityEngine;
using UnityEngine.Events;

// Zona invisible que avisa cuando el jugador entra.
// Sirve para: ganar la zona 3, cerrar la reja de la zona 4, despertar al Lich, etc.
public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool triggerOnce = true;

    public UnityEvent onPlayerEnter;

    private bool hasTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered && triggerOnce) return;
        if (!other.CompareTag(playerTag)) return;

        hasTriggered = true;
        onPlayerEnter.Invoke();
    }
}
