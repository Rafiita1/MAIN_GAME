using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private GameObject go;
    public static int artefacto;
    public float grenadeRange;
    public GameObject Artefacto1;
    public float damage = 10f;
    public float range = 50f;
    public ParticleSystem muzzleFlash;
    public float fireRate = 15f;
    public int maxAmmo = 20;
    private int currentAmmo;
    public float reloadTime = 3f;
    private bool isReloading = false;
    Animator anim;
    public GameObject brazos;
    Animator animArms;
    public Transform cam1;
    Rigidbody rb;
    private float nextTimeToFire = 0f;
    

    public Camera cam;
    private void Awake()
    {
        artefacto = 1;
    }
    private void Start()
    {
       animArms = brazos.GetComponent<Animator>();
       anim =  GetComponent<Animator>();
        if (currentAmmo == -1)
            currentAmmo = maxAmmo;
        rb = Artefacto1.GetComponent<Rigidbody>();
        go = GameObject.Find("artefacto1(Clone)");
    }


    private void FixedUpdate()
    {
     

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
        ReloadAnimation();


        if (Input.GetKeyDown(KeyCode.G) && artefacto == 1)
        {

            Launch();
        }

    }

    private void Launch()
    {

        GameObject Artefacto1Instance = Instantiate(Artefacto1, cam1.position,cam1.rotation);
        Artefacto1Instance.GetComponent<Rigidbody>().AddForce(cam1.forward * grenadeRange, ForceMode.Impulse);
        artefacto--;
        Destroy(go, 3f);

        
        if (artefacto < 0)
        {
            artefacto = 0;
        }

    }
    void ReloadAnimation()
    {

        if (currentAmmo == 0)
        {
            anim.SetBool("Reload", true);
            animArms.SetBool("ReloadArms", true);
        }
        if (currentAmmo > 0)
        {
            animArms.SetBool("ReloadArms", false);
            anim.SetBool("Reload", false);
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
