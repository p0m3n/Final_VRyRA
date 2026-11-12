// Cualquier cosa que pueda recibir daño.
// El proyectil del Lich no necesita saber que es el jugador: solo que puede recibir daño.
public interface IDamageable
{
    void TakeDamage(int amount);
}
