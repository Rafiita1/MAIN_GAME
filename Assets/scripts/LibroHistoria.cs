using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibroHistoria : MonoBehaviour
{
   public bool libroAbierto = false;
  
    Animator anim;
    public GameObject canvasMenu;
    public AudioSource mapa;

    void Start()
    {
        anim = GetComponent<Animator>();

       AudioSource audio = GetComponent<AudioSource>();

    }
    void Update()
    {
        if (libroAbierto == false && Input.GetKeyUp(KeyCode.M))
        {
           
            anim.SetBool("zoommap", true);
            mapa.Play();


            Invoke("AbrirLibro", 1.0f);
            
        }
        if (libroAbierto == true && Input.GetKeyUp(KeyCode.M))
        {
            anim.SetBool("zoommap", false);
           
            libroAbierto = false ;
        
        }
    }

    void AbrirLibro()
    {
        libroAbierto = true;
    


    }
   
   

}
