using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class totemPuzzle : MonoBehaviour
{
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerStay(Collider other)
    {
        if (Totem.totemCount == 5 && other.CompareTag("Player") && Input.GetKey(KeyCode.E))
        {
            gameObject.SetActive(false);
          
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
