using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artefacto : MonoBehaviour
{

  
    public GameObject thisObject;
    public GameObject effectoExplosion;
    private void Start()
    {
        thisObject = GameObject.Find("artefacto");
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        Instantiate(effectoExplosion, thisObject.transform.position, thisObject.transform.rotation);
        
    }

}
