using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] weapons;

    [Header("Keys")]
    [SerializeField] private KeyCode[] keys;

    [Header("Settings")]
    [SerializeField] private float switchTime;

    private int selectedWeapon;
    private float timeSincelastSwitch;

    private void Start()
    {
        SetWeapon();
        Select(selectedWeapon);

        timeSincelastSwitch = 0f;
    }

    private void SetWeapon()
    {
        weapons = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            weapons[i] = transform.GetChild(i);
        }

        if (keys == null || keys.Length == 0)
        {
            keys = new KeyCode[weapons.Length];
            for (int i = 0; i < weapons.Length; i++)
            {
                keys[i] = KeyCode.Alpha1 + i; // Default keys to Alpha1, Alpha2, etc.
            }
        }
    }

    private void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]) && timeSincelastSwitch > switchTime)
            {
                selectedWeapon = i;
                Debug.Log("Key pressed: " + keys[i] + ", switching to weapon index: " + selectedWeapon);
            }
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            Select(selectedWeapon);
        }

        timeSincelastSwitch += Time.deltaTime;
    }

    private void Select(int weaponIndex)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == weaponIndex);
        }

        timeSincelastSwitch = 0f;

        OnWeaponSelected();
    }

    private void OnWeaponSelected()
    {
        Debug.Log("Weapon Selected: " + weapons[selectedWeapon].name);
    }
}