using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Vidas del jugador. Va en el PlayerController, junto a PlayerRespawn.
// Al quedarse sin vidas avisa con un evento static (el Lich lo escucha para reiniciar su fase)
// y le pide a PlayerRespawn que lo regrese al último checkpoint.
public class PlayerHealth : MonoBehaviour, IDamageable
{
    public static event Action OnPlayerDied;

    [SerializeField] private int maxLives = 3;
    [SerializeField] private float invulnerableTime = 1f;

    public UnityEvent onDamaged;

    private int currentLives;
    private bool isInvulnerable;
    private IRespawnable respawnable;

    private void Awake()
    {
        currentLives = maxLives;
        TryGetComponent(out respawnable);
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable || currentLives <= 0) return;

        currentLives -= amount;
        onDamaged.Invoke();

        if (currentLives <= 0)
            Die();
        else
            StartCoroutine(InvulnerableRoutine());
    }

    private void Die()
    {
        OnPlayerDied?.Invoke();

        if (respawnable != null) respawnable.Respawn();

        currentLives = maxLives;
        StartCoroutine(InvulnerableRoutine());
    }

    // Un momento sin poder recibir daño después de cada golpe,
    // para que un mismo ataque no quite varias vidas de golpe
    private IEnumerator InvulnerableRoutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerableTime);
        isInvulnerable = false;
    }
}
