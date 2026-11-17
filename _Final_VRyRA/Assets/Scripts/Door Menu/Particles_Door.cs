using UnityEngine;
using UnityEngine.Events;

public class Particles_Door : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ActivateParticles()
    {
        gameObject.SetActive(true);
    }
}
