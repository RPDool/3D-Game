using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamagable
{
    private float health = 100f;
    [SerializeField] private GameObject bulletHolePrefab; // Add this line to include the bullet hole prefab
    [SerializeField] private AudioSource audioSource; // Add this line to include the AudioSource
    [SerializeField] private AudioClip hitSound; // Add this line to include the hit sound clip

    public void Damage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage;
        Debug.Log($"Health is now {health}");

        // Instantiate the bullet hole sprite at the hit point
        if (bulletHolePrefab != null)
        {
            Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, hitNormal);
            Instantiate(bulletHolePrefab, hitPoint, rotation, transform);
        }

        // Play the hit sound
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
        else
        {
            Debug.LogError("AudioSource or hitSound is not assigned.");
        }

        if (health <= 0)
        {
            Debug.Log($"{gameObject.name} is destroyed.");
            Destroy(gameObject);
        }
    }
}