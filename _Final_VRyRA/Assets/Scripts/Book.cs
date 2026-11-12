using System;
using System.Collections;
using UnityEngine;

// Un libro elemental de la zona 4.
// Solo sabe: flotar, "mostrarse" (luz + salto + nota) y avisar cuando le disparan verde.
// NO sabe cuál es el orden correcto: eso lo decide BookPuzzle.
public class Book : MonoBehaviour, IEnergyReceiver
{
    public event Action<Book> OnShot;

    [Header("Luz y sonido")]
    [SerializeField] private Light bookLight;
    [SerializeField] private AudioClip note;
    [SerializeField] private AudioClip rejectedClip;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color rejectedColor = Color.gray;

    [Header("Mostrarse")]
    [SerializeField] private float showTime = 0.6f;
    [SerializeField] private float jumpHeight = 0.25f;

    [Header("Flotar")]
    [SerializeField] private float bobHeight = 0.05f;
    [SerializeField] private float bobSpeed = 2f;

    private Vector3 basePosition;
    private Color originalLightColor;
    private bool isAwake;
    private bool isBusy;

    private void Awake()
    {
        basePosition = transform.localPosition;
        originalLightColor = bookLight.color;
        bookLight.enabled = false;
    }

    private void Update()
    {
        // Flota suave solo cuando está despierto y no está haciendo otra animación
        if (!isAwake || isBusy) return;

        float offset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.localPosition = basePosition + Vector3.up * offset;
    }

    public void ReceiveEnergy(EnergyType energy)
    {
        if (energy == EnergyType.Green)
            OnShot?.Invoke(this);
        else
            Reject();
    }

    public void SetAwake(bool awake)
    {
        isAwake = awake;
        if (!awake) transform.localPosition = basePosition;
    }

    // La secuencia: luz de su color, salto y su nota
    public IEnumerator Show()
    {
        isBusy = true;
        bookLight.color = originalLightColor;
        bookLight.enabled = true;
        if (note != null) AudioSource.PlayClipAtPoint(note, transform.position);

        float elapsed = 0f;
        while (elapsed < showTime)
        {
            elapsed += Time.deltaTime;
            // Sube y baja en forma de arco
            float height = Mathf.Sin((elapsed / showTime) * Mathf.PI) * jumpHeight;
            transform.localPosition = basePosition + Vector3.up * height;
            yield return null;
        }

        transform.localPosition = basePosition;
        bookLight.enabled = false;
        isBusy = false;
    }

    public void FlashCorrect()
    {
        if (note != null) AudioSource.PlayClipAtPoint(note, transform.position);
        StartCoroutine(FlashLight(correctColor, 0.3f));
    }

    public void Reject()
    {
        if (rejectedClip != null) AudioSource.PlayClipAtPoint(rejectedClip, transform.position);
        StartCoroutine(FlashLight(rejectedColor, 0.15f));
    }

    // Deja la luz encendida al ganar
    public void LightUpForever()
    {
        StopAllCoroutines();
        isBusy = false;
        bookLight.color = correctColor;
        bookLight.enabled = true;
    }

    private IEnumerator FlashLight(Color color, float duration)
    {
        bookLight.color = color;
        bookLight.enabled = true;
        yield return new WaitForSeconds(duration);
        bookLight.enabled = false;
        bookLight.color = originalLightColor;
    }
}
