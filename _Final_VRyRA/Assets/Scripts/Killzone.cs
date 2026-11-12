using UnityEngine;

// Zona de muerte: cualquier cosa que sepa reaparecer, reaparece.
// Va debajo del abismo de la zona 3.
public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IRespawnable respawnable))
            respawnable.Respawn();
    }
}
