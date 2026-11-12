using System.Collections;
using UnityEngine;

// Fundido a negro para VR: un Quad pegado frente a la cámara con material negro.
// Solo se encarga de oscurecer y aclarar la vista.
public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Renderer fadeRenderer;
    [SerializeField] private float fadeTime = 0.4f;

    private Material fadeMaterial;

    private void Awake()
    {
        if (fadeRenderer == null) fadeRenderer = GetComponent<Renderer>();
        fadeMaterial = fadeRenderer.material;

        SetAlpha(0f);
        fadeRenderer.enabled = false;
    }

    public IEnumerator FadeOut()
    {
        yield return FadeTo(1f);
    }

    public IEnumerator FadeIn()
    {
        yield return FadeTo(0f);
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        fadeRenderer.enabled = true;

        float startAlpha = fadeMaterial.color.a;
        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeTime));
            yield return null;
        }

        SetAlpha(targetAlpha);

        // Totalmente transparente: se apaga para no gastar en dibujarlo
        if (targetAlpha <= 0f) fadeRenderer.enabled = false;
    }

    private void SetAlpha(float alpha)
    {
        Color color = fadeMaterial.color;
        color.a = alpha;
        fadeMaterial.color = color;
    }
}
