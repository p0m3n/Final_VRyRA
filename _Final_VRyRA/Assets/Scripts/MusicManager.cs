using System.Collections;
using UnityEngine;

// Música de fondo con transición suave (crossfade) entre canciones.
// Patrón Singleton: solo puede existir uno. Si hubiera dos, sonarían dos canciones a la vez.
// Desde código: MusicManager.Instance.PlayMusic(clip);
// Desde el Inspector: cualquier UnityEvent -> MusicManager.PlayMusic (AudioClip).
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioClip startMusic;
    [SerializeField][Range(0f, 1f)] private float volume = 0.4f;
    [SerializeField] private float crossfadeTime = 2f;

    // Dos fuentes: mientras una baja de volumen, la otra sube
    private AudioSource sourceA;
    private AudioSource sourceB;
    private AudioSource activeSource;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        sourceA = CreateSource();
        sourceB = CreateSource();
        activeSource = sourceA;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        if (startMusic == null) return;

        activeSource.clip = startMusic;
        activeSource.volume = volume;
        activeSource.Play();
    }

    private AudioSource CreateSource()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.loop = true;
        source.playOnAwake = false;
        source.spatialBlend = 0f; // 2D: la música no sale de ningún punto del espacio
        source.volume = 0f;
        return source;
    }

    public void PlayMusic(AudioClip newClip)
    {
        // Si ya está sonando esa canción, no la reinicia
        if (newClip != null && activeSource.clip == newClip && activeSource.isPlaying) return;

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(CrossfadeRoutine(newClip));
    }

    // Baja la música hasta el silencio (por ejemplo, al derrotar al jefe)
    public void StopMusic()
    {
        PlayMusic(null);
    }

    private IEnumerator CrossfadeRoutine(AudioClip newClip)
    {
        AudioSource from = activeSource;
        AudioSource to = activeSource == sourceA ? sourceB : sourceA;
        activeSource = to;

        float fromStartVolume = from.volume;

        if (newClip != null)
        {
            to.clip = newClip;
            to.volume = 0f;
            to.Play();
        }

        float elapsed = 0f;
        while (elapsed < crossfadeTime)
        {
            // unscaledDeltaTime: la transición funciona aunque el juego esté en pausa
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / crossfadeTime;

            from.volume = Mathf.Lerp(fromStartVolume, 0f, t);
            if (newClip != null) to.volume = Mathf.Lerp(0f, volume, t);

            yield return null;
        }

        from.Stop();
        from.volume = 0f;
        if (newClip != null) to.volume = volume;

        fadeRoutine = null;
    }
}
