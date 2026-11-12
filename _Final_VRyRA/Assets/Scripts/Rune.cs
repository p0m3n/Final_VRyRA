using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Reemplaza a Runes, Rune_Green y Rune_Pink.
// Una sola clase sirve para cualquier cristal: el tipo se elige en el Inspector.
public class Rune : MonoBehaviour, IEnergyReceiver
{
    [Header("Energía")]
    [SerializeField] private EnergyType acceptedEnergy;
    [SerializeField] private bool singleUse = true;
    [SerializeField] private bool startsLocked = false;

    [Header("Eventos")]
    public UnityEvent onActivated;
    public UnityEvent onRejected;

    [Header("Visual")]
    [SerializeField] private Renderer[] runeRenderers;
    [SerializeField] private Color activatedColor = Color.white;
    [SerializeField] private Color rejectedColor = Color.gray;
    [SerializeField] private float flashTime = 0.15f;

    [Header("Audio")]
    [SerializeField] private AudioClip activatedClip;
    [SerializeField] private AudioClip rejectedClip;

    private Collider runeCollider;
    private Color[] originalColors;
    private bool isActivated;
    private bool isLocked;
    private Coroutine flashRoutine;

    private void Awake()
    {
        // Si no se asignan en el Inspector, toma todos los renderers propios y de sus hijos.
        // Así funciona igual con un cristal solo que con una coraza hecha de varios cristales.
        if (runeRenderers == null || runeRenderers.Length == 0)
            runeRenderers = GetComponentsInChildren<Renderer>();

        runeCollider = GetComponent<Collider>();
        isLocked = startsLocked;

        originalColors = new Color[runeRenderers.Length];
        for (int i = 0; i < runeRenderers.Length; i++)
            originalColors[i] = runeRenderers[i].material.color;
    }

    // La llama la bala al tocar este cristal (ver Bullet.OnTriggerEnter)
    public void ReceiveEnergy(EnergyType energy)
    {
        if (isActivated) return;

        // Bloqueado: rechaza cualquier energía hasta que alguien lo desbloquee
        if (!isLocked && energy == acceptedEnergy)
            Activate();
        else
            Reject();
    }

    // Se llama desde un evento (por ejemplo, cuando un RuneCounter se completa)
    public void Unlock()
    {
        isLocked = false;
    }

    private void Activate()
    {
        if (singleUse)
        {
            isActivated = true;
            runeCollider.enabled = false;
        }

        if (flashRoutine != null) StopCoroutine(flashRoutine);

        SetColor(activatedColor);
        PlaySound(activatedClip);
        onActivated.Invoke();
    }

    private void Reject()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(RejectFlash());

        PlaySound(rejectedClip);
        onRejected.Invoke();
    }

    // Destello breve: el cristal cambia de color un instante y regresa
    private IEnumerator RejectFlash()
    {
        SetColor(rejectedColor);
        yield return new WaitForSeconds(flashTime);
        RestoreColors();
        flashRoutine = null;
    }

    // Deja el cristal como al inicio (lo usaremos en la zona 5 cuando reaparecen las bolitas)
    public void ResetRune()
    {
        isActivated = false;
        isLocked = startsLocked;
        runeCollider.enabled = true;
        RestoreColors();
    }

    private void SetColor(Color color)
    {
        for (int i = 0; i < runeRenderers.Length; i++)
            runeRenderers[i].material.color = color;
    }

    private void RestoreColors()
    {
        for (int i = 0; i < runeRenderers.Length; i++)
            runeRenderers[i].material.color = originalColors[i];
    }

    private void PlaySound(AudioClip clip)
    {
        // PlayClipAtPoint crea su propio objeto temporal:
        // el sonido se escucha aunque el cristal se desactive en ese mismo momento
        if (clip != null) AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}
