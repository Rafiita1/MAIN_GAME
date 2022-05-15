using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLastDoor : MonoBehaviour
{
    bool dooropened = false;
    public AudioClip wooden;
    Animator anim;
    public GameObject llave1;
    public GameObject llave2;
    public GameObject llave3;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && llave1.activeInHierarchy == true && llave2.activeInHierarchy== true && llave3.activeInHierarchy == true && Input.GetKey(KeyCode.E)  && dooropened == false )
        {


            anim.SetTrigger("opendoor");
            AudioSource.PlayClipAtPoint(wooden, transform.position);
            dooropened = true;


        }
    }
}
