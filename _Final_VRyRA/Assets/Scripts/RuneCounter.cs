using UnityEngine;
using UnityEngine.Events;

// Cuenta cuántas runas se activaron y avisa cuando se llega a la meta.
// Solo cuenta: no mueve puertas ni prende luces. Lo que pase al completarse
// se conecta en el Inspector con onCompleted.
// Es la clase base de TimedRuneCounter (misma idea, pero con tiempo límite).
public class RuneCounter : MonoBehaviour
{
    [SerializeField] protected int requiredCount = 3;

    public UnityEvent onCompleted;

    protected int currentCount;
    protected bool isCompleted;

    // Conectar al onActivated de cada runa
    public virtual void AddOne()
    {
        if (isCompleted) return;

        currentCount++;

        if (currentCount >= requiredCount)
            Complete();
    }

    protected virtual void Complete()
    {
        isCompleted = true;
        onCompleted.Invoke();
    }

    public virtual void ResetCounter()
    {
        currentCount = 0;
        isCompleted = false;
    }
}
