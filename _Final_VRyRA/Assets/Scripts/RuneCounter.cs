using UnityEngine;
using UnityEngine.Events;

// Cuenta cuántas runas se activaron y avisa cuando se llega a la meta.
// Solo cuenta: no mueve puertas ni prende luces. Lo que pase al completarse
// se conecta en el Inspector con onCompleted.
// Lo reutilizaremos en la zona 5 para contar las bolitas del Lich en cada fase.
public class RuneCounter : MonoBehaviour
{
    [SerializeField] private int requiredCount = 3;

    public UnityEvent onCompleted;

    private int currentCount;

    // Conectar al onActivated de cada runa
    public void AddOne()
    {
        currentCount++;

        if (currentCount == requiredCount)
            onCompleted.Invoke();
    }

    public void ResetCounter()
    {
        currentCount = 0;
    }
}
