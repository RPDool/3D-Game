using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f;
    private float lifetime = 5f;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with " + collision.gameObject.name);

        // Apply damage to the target and pass the hit point and hit normal
        IDamagable damageable = collision.gameObject.GetComponent<IDamagable>();
        if (damageable != null)
        {
            ContactPoint contact = collision.contacts[0];
            damageable.Damage(damageAmount, contact.point, contact.normal);
        }

        // Stop the bullet's movement
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Optionally, destroy the bullet after a short delay to allow the bullet hole to be visible
        Destroy(gameObject, 0.1f);
    }
}