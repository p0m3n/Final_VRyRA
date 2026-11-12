// Cualquier cosa que pueda "reaparecer" en el último punto seguro.
// La zona de muerte no necesita saber que es el jugador: solo que sabe reaparecer.
public interface IRespawnable
{
    void Respawn();
}
