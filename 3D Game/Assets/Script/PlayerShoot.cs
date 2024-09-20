using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;

    [SerializeField] private KeyCode shootKey = KeyCode.Mouse0; // Default to left mouse button
    [SerializeField] private KeyCode reloadKey = KeyCode.R; // Default to 'R' key

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