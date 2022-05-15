using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtefactoBossFinal : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKey(KeyCode.E))
        {
            Shooting.artefactoBossCount = 1 ;
            Destroy(gameObject);

        }
    }








}
