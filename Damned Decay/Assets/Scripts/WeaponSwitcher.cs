using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] int currentWeapon = 0;

    void Start() // on start find the active weapon to get the starting weapon.
    {
        SetWeaponActive();
    }

    // Update is called once per frame
    void Update()
    {
        int previousWeapon = currentWeapon;

        ProcessKeyInput();
        ProcessScrollWheel();

        if (previousWeapon != currentWeapon)
        {
            SetWeaponActive();
        }
    }

    private void ProcessScrollWheel()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (currentWeapon >= transform.childCount - 1) // If player tries to scroll past the 3 weapons available
                                                           // the current weapon will be reset back to 0.
            {
                currentWeapon = 0; // this resets weapon selected back to 0 (pistol) if scrolling past the rifle index (2)
            }

            else
            {
                currentWeapon++; // otherwise increase weapon index by 1.
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentWeapon <= 0)                        // same code as above but this is for scrolling down. i.e going
            {                                              // from rifle to shotgun to pistol and then repeating back to
                currentWeapon = transform.childCount - 1;  // rifle if scrolling past the pistol.
            }

            else
            {
                currentWeapon--; // the logic is just reversed from above.
            }
        }
    }

        private void ProcessKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeapon = 2;
        }
    }


    private void SetWeaponActive() // find which weapon is active.
    {
        int weaponIndex = 0; // the first gun (the pistol) under the weapons object will be given an index
                             // of 0, 1 for the second gun (the shotgun) and 3 for the last gun (the rifle).

        foreach (Transform weapon in transform)
        {
            if (weaponIndex == currentWeapon) // if the weapon in hand has an index of 0
            {
                weapon.gameObject.SetActive(true); // make that gun set as active
            }
            else // otherwise any other indexed weapons
            {
                weapon.gameObject.SetActive(false); // will be set to false and not be seen.
            }
            weaponIndex++; // add 1 to the index and go through the SetWeaponActive
                           // function again until active weapon is found
        }

    }
}
