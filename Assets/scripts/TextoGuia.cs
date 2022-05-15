using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextoGuia : MonoBehaviour
{
    private bool showtext = false;


    private void OnTriggerEnter(Collider other)
    {
        showtext = true;
    }


    private void OnTriggerExit(Collider other)
    {
        showtext=false;
    }

    private void OnGUI()
    {
        if (showtext)
        {

            GUI.Label(new Rect(10, 10, 100, 100), transform.name);


        }
    }


}
