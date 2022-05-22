using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaPotolosd : MonoBehaviour
{
    public GameObject canvas;
    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else
        {
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canvas.SetActive(false);
    }
}
