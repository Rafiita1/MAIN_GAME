using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float damage = 10f;
    public float range = 50f;
    public ParticleSystem muzzleFlash;
    public float fireRate = 15f;
    public int maxAmmo = 20;
    private int currentAmmo;
    public float reloadTime = 3f;
    private bool isReloading = false;
    

    private float nextTimeToFire = 0f;
    

    public Camera cam;

    private void Start()
    {
        if (currentAmmo == -1)
            currentAmmo = maxAmmo;

        
    }

    void Update()
    {
        if (isReloading)
            return;

        
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            Shoot();
            nextTimeToFire = Time.time + 1f / fireRate;
            
        }



    }

    IEnumerator Reload()
    {
        isReloading = true; 
        Debug.Log("Reloading...");

        yield return new  WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;


    }
    void Shoot()
    {
        muzzleFlash.Play();
        currentAmmo--;
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
        }
        Target target = hit.transform.GetComponent<Target>();
        if (target != null)
        {

            target.TakeDamage(damage);



        }

        
    }

 

}
