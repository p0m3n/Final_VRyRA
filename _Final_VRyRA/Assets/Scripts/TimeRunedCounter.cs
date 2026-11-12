using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Igual que RuneCounter, pero hay que activar todas las runas antes de que se acabe el tiempo.
// El reloj empieza con la PRIMERA runa activada. Si se acaba, todas las runas se reinician.
// Hereda de RuneCounter: reutiliza el conteo y solo agrega el temporizador.
public class TimedRuneCounter : RuneCounter
{
    [SerializeField] private float timeLimit = 6f;
    [SerializeField] private Rune[] runes;

    public UnityEvent onTimerStarted;
    public UnityEvent onTimeout;

    private Coroutine timerRoutine;

    public override void AddOne()
    {
        if (isCompleted) return;

        if (currentCount == 0)
        {
            timerRoutine = StartCoroutine(TimerRoutine());
            onTimerStarted.Invoke();
        }

        base.AddOne();
    }

    protected override void Complete()
    {
        // Se logró a tiempo: el reloj ya no importa
        if (timerRoutine != null) StopCoroutine(timerRoutine);
        timerRoutine = null;

        base.Complete();
    }

    private IEnumerator TimerRoutine()
    {
        yield return new WaitForSeconds(timeLimit);

        timerRoutine = null;

        foreach (Rune rune in runes)
            rune.ResetRune();

        ResetCounter();
        onTimeout.Invoke();
    }
}
