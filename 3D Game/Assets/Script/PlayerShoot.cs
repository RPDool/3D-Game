using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;

    [SerializeField] private KeyCode shootKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode reloadKey = KeyCode.R;

    private void Update()
    {
        if (Input.GetKeyDown(shootKey))
        {
            Debug.Log("Shoot key pressed.");
            shootInput?.Invoke();
        }
        if (Input.GetKeyDown(reloadKey))
        {
            Debug.Log("Reload key pressed.");
            reloadInput?.Invoke();
        }
    }
}