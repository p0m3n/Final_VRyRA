using UnityEngine;

// Proyectil del Lich: avanza en línea recta (NO persigue al jugador).
// Si el jugador se mueve a tiempo, falla.
// Lo crea y recicla un BulletPooling (patrón Object Pool), igual que las balas del jugador.
public class LichProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 4.5f;
    [SerializeField] private float lifeTime = 6f;
    [SerializeField] private int damage = 1;

    private float timer;

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= lifeTime) gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignora triggers (checkpoints, bolitas, balas) y al propio Lich
        if (other.isTrigger) return;
        if (other.GetComponentInParent<LichBoss>() != null) return;

        if (other.TryGetComponent(out IDamageable damageable))
            damageable.TakeDamage(damage);

        // Choca con el jugador, una pared o el suelo: desaparece
        gameObject.SetActive(false);
    }
}
