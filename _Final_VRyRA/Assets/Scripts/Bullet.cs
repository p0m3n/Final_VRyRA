using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public Transform cameraTransform;

    public float force;
    public float despawnTime;
    public float damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnEnable()
    {
        cameraTransform = FindAnyObjectByType<CharacterCameraConstraint>().transform;
        rb.AddForce(cameraTransform.forward * force, ForceMode.Impulse);
        StartCoroutine(EnabledTime());
    }

    IEnumerator EnabledTime()
    {
        yield return new WaitForSeconds(despawnTime);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
