using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform cam;

    private float timeSinceLastShot;

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
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, gunData.maxDistance))
                {
                    IDamagable damagable = hit.transform.GetComponent<IDamagable>();
                    damagable?.Damage(gunData.damage);
                }

                gunData.currentAmmo--;
                timeSinceLastShot = 0f;
                OnGunShot();
            }
        }
    }

    private void Update()
    {
        if (cam == null)
        {
            return;
        }

        timeSinceLastShot += Time.deltaTime;

        if (Input.GetButton("Fire1")) // Assuming "Fire1" is the input for shooting
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R)) // Assuming "R" is the key for reloading
        {
            StartReload();
        }

        Debug.DrawRay(cam.position, cam.forward);
    }

    private void OnGunShot()
    {
        // Implement any additional logic for when the gun is shot
    }
}