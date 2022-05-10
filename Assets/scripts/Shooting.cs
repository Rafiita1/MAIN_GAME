using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public static int llave;
    public AudioSource reload;
    public AudioSource disparo;
    public Transform ammo;
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
    public Transform posicionArt;
    Rigidbody rb;
    private float nextTimeToFire = 0f;
    public GameObject bulletImpact;

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
        go = GameObject.Find(go.name);
    }


    private void FixedUpdate()
    {
     

    }
    void Update()
    {
        if (isReloading)
            return;
        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetBool("Reload", true);
            reload.Play();
            animArms.SetBool("ReloadArms", true);
            StartCoroutine(Reload());
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {

            disparo.Play();
            Shoot();
            nextTimeToFire = Time.time + 1f / fireRate;
            
        }
        ReloadAnimation();


        if (Input.GetKeyDown(KeyCode.Mouse1) && artefacto >= 1)
        {
            anim.SetBool("Lanzagranadas", true);
            animArms.SetBool("Lanzagranadas", true);
            
            Launch();

         
        }
        else
        {
            anim.SetBool("Lanzagranadas", false);
            animArms.SetBool("Lanzagranadas", false);
        }


      

    }

    
    private void Launch()
    {

        GameObject Artefacto1Instance = Instantiate(Artefacto1, posicionArt.position,posicionArt.rotation);
        Artefacto1Instance.GetComponent<Rigidbody>().AddForce(posicionArt.forward * grenadeRange, ForceMode.Impulse);
        artefacto--;
      
       

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
            reload.Play();
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
        ammo.GetComponent<Text>().text = "20" + ammo.name.ToString();
        currentAmmo = maxAmmo;
        isReloading = false;


    }
    void Shoot()
    {
        ammo.GetComponent<Text>().text = currentAmmo.ToString() + ammo.name.ToString(); ;
        muzzleFlash.Play();
        currentAmmo--;
        RaycastHit hit;
        

        if (currentAmmo == 0)
        {
            ammo.GetComponent<Text>().text = "0" + ammo.name.ToString(); ;

        }
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            Instantiate(bulletImpact, hit.point, transform.rotation);
            Debug.Log(hit.transform.name);
        }
        Target target = hit.transform.GetComponent<Target>();
        if (target != null)
        {

            target.TakeDamage(damage);



        }

        
    }

 

}
