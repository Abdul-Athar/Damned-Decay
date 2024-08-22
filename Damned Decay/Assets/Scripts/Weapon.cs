using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Camera FPCamera;           // I am using SerializeField for all variable attributes
    [SerializeField] float range = 100f;        // this allows me to access them through the unity editor
    [SerializeField] float damage = 30f;        // without making them public.
    [SerializeField] ParticleSystem muzzleFlash;// all of these variables are still private and cannot be
    [SerializeField] GameObject hitEffect;      // directly accessed from other scripts or called upon by
    [SerializeField] Ammo ammoSlot;             // accident
    [SerializeField] AmmoType ammoType;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] TextMeshProUGUI ammoText;

    bool canShoot = true;

    private void OnEnable()
    {
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayAmmo();

        if (Input.GetMouseButtonDown(0) && canShoot == true) // the co routine will stop the function half way thorugh, wait, and continue later.
        {
            StartCoroutine(Shoot()); // a Co-routine is a method that pauses excecution
        }                            // and takes away user control but then continues
    }                                // where it left off in the following frame.

    private void DisplayAmmo()
    {
        int currentAmmo = ammoSlot.GetCurrentAmmo(ammoType);
        ammoText.text = currentAmmo.ToString();
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        if (ammoSlot.GetCurrentAmmo(ammoType) > 0)
        {
            PlayMuzzleFlash();
            ProcessRaycast();
            ammoSlot.ReduceCurrentAmmo(ammoType);
        }
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    private void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    private void ProcessRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, range))
        {
            CreateHitImpact(hit);

            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if(target == null)
            {
                return;
            }
            target.TakeDamage(damage);
        }
        else
        {
            return;
        }
    }

    private void CreateHitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 1);
    }
}
