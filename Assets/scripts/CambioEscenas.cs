using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscenas : MonoBehaviour
{
    public GameObject canvas;
    public string SceneName = "Aldea";
    private void OnTriggerEnter(Collider other)
    {
  
        if (Shooting.artefactoBossCount == 1 && other.CompareTag("Player"))
        {
         


            bl_SceneLoaderUtils.GetLoader.LoadLevel(SceneName);
            

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

