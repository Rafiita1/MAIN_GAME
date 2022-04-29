using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Shooting.artefacto = 1;
        Destroy(gameObject);
    }
}
