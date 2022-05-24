using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtefactoBossFinal : MonoBehaviour
{
    public GameObject barrera;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKey(KeyCode.E) && barrera.activeSelf== false)
        {
            Shooting.artefactoBossCount = 1 ;
            Destroy(gameObject);

        }
    }








}
