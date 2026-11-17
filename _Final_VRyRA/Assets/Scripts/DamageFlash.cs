using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Renderer flashRenderer;
    [SerializeField] private Texture vignetteTexture;
    [SerializeField] private Color flashColor = new Color(1f, 0f, 0f, 0.7f);
    [SerializeField] private float fadeTime = 0.5f;

    private Material flashMaterial;
    private Coroutine flashRoutine;

    private void Awake()
    {
        if (flashRenderer == null) flashRenderer = GetComponent<Renderer>();
  
        flashMaterial = flashRenderer.material;

        if (vignetteTexture != null) flashMaterial.mainTexture = vignetteTexture;

        flashRenderer.enabled = false;
    }

 
    public void Flash()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    // Prende el rojo y lo desvanece poco a poco
    private IEnumerator FlashRoutine()
    {
        flashRenderer.enabled = true;
        float elapsed = 0f;
      
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            Color color = flashColor;
            color.a = Mathf.Lerp(flashColor.a, 0f, elapsed / fadeTime);
            flashMaterial.color = color;
            yield return null;
        }

        flashRenderer.enabled = false;
        flashRoutine = null;
    }
}
