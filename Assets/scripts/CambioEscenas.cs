using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscenas : MonoBehaviour
{
    
    public string SceneName = "ZonaJefe";
    private void OnTriggerEnter(Collider other)
    {
  
        if (other.CompareTag("Player") )
        {
         


            bl_SceneLoaderUtils.GetLoader.LoadLevel(SceneName);
            

        }
    }

 
}

