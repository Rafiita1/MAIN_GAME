using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLastDoor : MonoBehaviour
{
    public GameObject llave1;
    public GameObject llave2;
    public GameObject llave3;


    private void Start()
    {
            llave1.SetActive(false);    
        llave2.SetActive(false);
          llave3.SetActive(false);
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Shooting.llave == 3 && Input.GetKey(KeyCode.E))
        {
            llave1.SetActive(true);

            Shooting.llave--;
        }

        if (other.CompareTag("Player") && Shooting.llave == 2 && Input.GetKey(KeyCode.E))
        {
            llave2.SetActive(true);
            Shooting.llave--;
        }
        if (other.CompareTag("Player") && Shooting.llave == 1 && Input.GetKey(KeyCode.E))
        {
            llave3.SetActive(true);
            Shooting.llave--;
        }
    }



}
