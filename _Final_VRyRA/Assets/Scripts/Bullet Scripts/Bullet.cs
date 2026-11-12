using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    //public Transform cameraTransform;

    public float force;
    public float despawnTime;
    public float damage;

    public EnergyType type;

    private void OnTriggerEnter(Collider other)
    {
        // Le entrega su energía a cualquier objeto que sepa recibirla
        if (other.TryGetComponent(out IEnergyReceiver receiver))
            receiver.ReceiveEnergy(type);

        StopCoroutine(EnabledTime());
        DespawnSteps();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    public void OnEnable()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //cameraTransform = FindAnyObjectByType<CharacterCameraConstraint>().transform;
        rb.AddForce((-1 * gameObject.transform.up) * force, ForceMode.Impulse);
        StartCoroutine(EnabledTime());
    }

    IEnumerator EnabledTime()
    {
        yield return new WaitForSeconds(despawnTime);
        DespawnSteps();
    }

    private void DespawnSteps()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
