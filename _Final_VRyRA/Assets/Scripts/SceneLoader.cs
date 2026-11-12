using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Cambia de escena después de una espera (para que se vea la animación de muerte)
// y con fundido a negro si se le asigna un ScreenFader.
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName = "Menu";
    [SerializeField] private float delay = 3f;
    [SerializeField] private ScreenFader screenFader;

    private bool isLoading;

    // Conectar al onDefeated del jefe
    public void LoadScene()
    {
        if (isLoading) return;

        isLoading = true;
        StartCoroutine(LoadRoutine());
    }

    private IEnumerator LoadRoutine()
    {
        yield return new WaitForSeconds(delay);

        if (screenFader != null) yield return screenFader.FadeOut();

        SceneManager.LoadScene(sceneName);
    }
}
