using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibroHistoria : MonoBehaviour
{
    public bool libroAbierto = false;
    public GameObject libro;
    public GameObject camaraLibro;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        


    }
    void Update()
    {
        if (libroAbierto == false && Input.GetKey(KeyCode.M))
        {
            
            anim.SetBool("Open", true);
            camaraLibro.SetActive(true);


            ReseumeGame();
            
        }
        else if (libroAbierto == true && Input.GetKey(KeyCode.M))
        {
            Time.timeScale = 1;
            libro.SetActive(false);
            camaraLibro.SetActive(false);

            libroAbierto = false;


        }
    }

  
    public void ReseumeGame()
    {
        StartCoroutine(Resuming());

    }

    IEnumerator Resuming()
    {
        yield return new WaitForSeconds(1);
        libroAbierto = true;
        anim.SetBool("Open", false);
        Time.timeScale = 0;
        StopCoroutine(Resuming());
    }


 
}
