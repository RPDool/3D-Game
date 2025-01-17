using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform cam;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint; 
    [SerializeField] private AudioSource shootSound; // Add this line to include an AudioSource for the shooting sound
    [SerializeField] private AudioClip shootClip; // Add this line to include an AudioClip for the shooting sound

    private float timeSinceLastShot;

    void Awake()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Bullet"));
    }

    private void Start()
    {
        PlayerShoot.reloadInput += StartReload;
    }

    private void OnDisable() => gunData.reloading = false;

    public void StartReload()
    {
        if (!gunData.reloading && this.gameObject.activeSelf)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;

        gunData.reloading = false;
    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    public void Shoot()
    {
        if (gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                Debug.Log("Shooting...");

                GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                Debug.Log("Projectile instantiated");

                projectile.transform.Rotate(0, 90, 0);

                // Ensure the MeshCollider is convex
                MeshCollider meshCollider = projectile.GetComponent<MeshCollider>();
                if (meshCollider != null)
                {
                    meshCollider.convex = true; 
                }

                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    rb.AddForce(cam.forward * gunData.projectileSpeed, ForceMode.Impulse);

                    Collider projectileCollider = projectile.GetComponent<Collider>();
                    Collider playerCollider = GetComponent<Collider>(); 
                    if (projectileCollider != null && playerCollider != null)
                    {
                        Physics.IgnoreCollision(projectileCollider, playerCollider); 
                    }
                }
                else
                {
                    Debug.LogError("Projectile prefab does not have a Rigidbody component.");
                }

                gunData.currentAmmo--;
                timeSinceLastShot = 0f;
                OnGunShot();
            }
        }
        else
        {
            Debug.Log("Out of ammo");
        }
    }

    private void Update()
    {
        if (cam == null)
        {
            return;
        }

        timeSinceLastShot += Time.deltaTime;

        if (Input.GetButton("Fire1"))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
        }

        Debug.DrawRay(cam.position, cam.forward);
    }

    private void OnGunShot()
    {
        // Play the shooting sound using PlayOneShot
        if (shootSound != null && shootClip != null)
        {
            shootSound.PlayOneShot(shootClip);
        }
        else
        {
            Debug.LogError("No AudioSource or AudioClip assigned for shooting sound.");
        }
    }
}