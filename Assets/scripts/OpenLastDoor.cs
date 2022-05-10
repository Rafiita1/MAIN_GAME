using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLastDoor : MonoBehaviour
{
    bool dooropened = false;
    public AudioClip wooden;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKey(KeyCode.E) && Shooting.llave == 3 && dooropened == false)
        {


            anim.SetTrigger("opendoor");
            AudioSource.PlayClipAtPoint(wooden, transform.position);
            dooropened = true;


        }
    }
}
