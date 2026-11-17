using UnityEngine;
using UnityEngine.Events;

public class DetectionSphere : MonoBehaviour
{
    public UnityEvent dEvent1;
    public UnityEvent dEvent2;

    //private void OnTriggerEnter(Collider other)
    //{
    //    PlayerMain player = other.GetComponent<PlayerMain>();
    //    if (!player)
    //    {
    //        dEvent1.Invoke();
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        PlayerMain player = other.GetComponent<PlayerMain>();
        if (!player)
        {
            dEvent1.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMain player = other.GetComponent<PlayerMain>();
        if (!player)
        {
            dEvent2.Invoke();
        }
    }
}
