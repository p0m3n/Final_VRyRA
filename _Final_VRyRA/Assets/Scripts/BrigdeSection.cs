using System.Collections;
using UnityEngine;

// Una sección del puente. Al iniciar se esconde y, cuando se llama Appear(),
// sube desde abajo hasta su posición final.
// Constrúyela en el editor YA en su posición final: el script la guarda al iniciar.
public class BridgeSection : MonoBehaviour
{
    [SerializeField] private float riseDistance = 3f;
    [SerializeField] private float riseTime = 0.8f;

    private Vector3 finalPosition;

    private void Awake()
    {
        finalPosition = transform.position;
        gameObject.SetActive(false);
    }

    // Conectar al onActivated del cristal que controla esta sección
    public void Appear()
    {
        // Una corrutina no puede iniciar en un objeto apagado: primero se activa
        gameObject.SetActive(true);
        StartCoroutine(RiseRoutine());
    }

    private IEnumerator RiseRoutine()
    {
        Vector3 startPosition = finalPosition + Vector3.down * riseDistance;
        float elapsed = 0f;

        while (elapsed < riseTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / riseTime);
            transform.position = Vector3.Lerp(startPosition, finalPosition, t);
            yield return null;
        }

        transform.position = finalPosition;
    }
}
