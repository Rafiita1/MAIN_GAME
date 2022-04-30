using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artefacto : MonoBehaviour
{

  
    public Transform thisObject;
    public GameObject effectoExplosion;
    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        Instantiate(effectoExplosion, thisObject.position, thisObject.rotation);
        
    }

}
