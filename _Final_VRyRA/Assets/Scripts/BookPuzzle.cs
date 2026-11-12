using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// El "Simón dice" de la zona 4.
// Máquina de estados sencilla (patrón State con un enum):
// Dormant -> ShowingSequence -> WaitingInput -> (siguiente ronda | Dormant si falla | Completed)
public class BookPuzzle : MonoBehaviour
{
    private enum PuzzleState { Dormant, ShowingSequence, WaitingInput, Completed }

    [Header("Piezas")]
    [SerializeField] private Book[] books;
    [SerializeField] private Rune necroRune;

    [Header("Rondas")]
    [SerializeField] private int[] roundLengths = { 2, 3, 4 };
    [SerializeField] private float delayBeforeSequence = 1f;
    [SerializeField] private float delayBetweenBooks = 0.3f;
    [SerializeField] private float delayBetweenRounds = 1.5f;

    [Header("Eventos")]
    public UnityEvent onRoundCompleted;
    public UnityEvent onPuzzleCompleted;
    public UnityEvent onFailed;

    private PuzzleState state = PuzzleState.Dormant;
    private int[] sequence;
    private int currentRound;
    private int inputIndex;

    private void Awake()
    {
        GenerateSequence();
    }

    private void OnEnable()
    {
        foreach (Book book in books)
            book.OnShot += HandleBookShot;
    }

    private void OnDisable()
    {
        foreach (Book book in books)
            book.OnShot -= HandleBookShot;
    }

    // Una secuencia al azar del largo de la ronda más larga, sin repetir el mismo libro dos veces seguidas.
    // Cada ronda usa el inicio de la misma secuencia: 2 libros, luego 3, luego 4.
    private void GenerateSequence()
    {
        int maxLength = roundLengths[roundLengths.Length - 1];
        sequence = new int[maxLength];

        for (int i = 0; i < maxLength; i++)
        {
            int index;
            do
            {
                index = Random.Range(0, books.Length);
            }
            while (i > 0 && index == sequence[i - 1]);

            sequence[i] = index;
        }
    }

    // Conectar al onActivated del libro de nigromancia
    public void StartSequence()
    {
        if (state != PuzzleState.Dormant) return;

        necroRune.gameObject.SetActive(false);
        StartCoroutine(PlaySequenceRoutine(delayBeforeSequence));
    }

    private IEnumerator PlaySequenceRoutine(float startDelay)
    {
        state = PuzzleState.ShowingSequence;

        foreach (Book book in books)
            book.SetAwake(true);

        yield return new WaitForSeconds(startDelay);

        int length = roundLengths[currentRound];
        for (int i = 0; i < length; i++)
        {
            yield return books[sequence[i]].Show();
            yield return new WaitForSeconds(delayBetweenBooks);
        }

        inputIndex = 0;
        state = PuzzleState.WaitingInput;
    }

    private void HandleBookShot(Book book)
    {
        // Mientras muestra la secuencia o está dormido, los libros no aceptan disparos
        if (state != PuzzleState.WaitingInput)
        {
            book.Reject();
            return;
        }

        if (book == books[sequence[inputIndex]])
        {
            book.FlashCorrect();
            inputIndex++;

            if (inputIndex == roundLengths[currentRound])
                CompleteRound();
        }
        else
        {
            Fail();
        }
    }

    private void CompleteRound()
    {
        currentRound++;
        onRoundCompleted.Invoke();

        if (currentRound == roundLengths.Length)
        {
            state = PuzzleState.Completed;
            foreach (Book book in books)
                book.LightUpForever();

            onPuzzleCompleted.Invoke();
        }
        else
        {
            StartCoroutine(PlaySequenceRoutine(delayBetweenRounds));
        }
    }

    private void Fail()
    {
        state = PuzzleState.Dormant;

        foreach (Book book in books)
        {
            book.Reject();
            book.SetAwake(false);
        }

        // El libro de nigromancia revive: hay que romperlo otra vez para repetir la ronda.
        // Las rondas ya ganadas se conservan (currentRound no se reinicia).
        necroRune.gameObject.SetActive(true);
        necroRune.ResetRune();

        onFailed.Invoke();
    }
}
