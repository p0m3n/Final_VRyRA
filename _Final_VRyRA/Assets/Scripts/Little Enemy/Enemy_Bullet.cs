using System.Collections;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    public Rigidbody rb;
    //public Transform cameraTransform;

    public float force;
    public float despawnTime;
    public float damage;

    private TrailRenderer trail;

    private void OnTriggerEnter(Collider other)
    {
     
        if (other.isTrigger) return;
        if (other.GetComponentInParent<LittleEnemy>() != null) return;
        if (other.TryGetComponent(out IDamageable damageable))
            damageable.TakeDamage(Mathf.RoundToInt(damage)); // TakeDamage pide int y damage es float

       
        DespawnSteps();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        trail = GetComponentInChildren<TrailRenderer>(); 
    }

    public void OnEnable()
    {
        // desde donde desapareció el disparo anterior hasta el enemigo
        if (trail != null) trail.Clear();

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //cameraTransform = FindAnyObjectByType<CharacterCameraConstraint>().transform;
        rb.AddForce((gameObject.transform.forward) * force, ForceMode.Impulse);
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
