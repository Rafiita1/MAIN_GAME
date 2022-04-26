using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibroHistoria : MonoBehaviour
{
   public bool libroAbierto = false;
  
    Animator anim;
    public GameObject canvasMenu;

    void Start()
    {
        anim = GetComponent<Animator>();
        


    }
    void Update()
    {
        if (libroAbierto == false && Input.GetKeyUp(KeyCode.M))
        {
           
            anim.SetBool("abrir", true);
          
            
            Invoke("AbrirLibro", 1.0f);
            
        }
        if (libroAbierto == true && Input.GetKeyUp(KeyCode.M))
        {
            anim.SetBool("abrir", false);

            libroAbierto = false ;
            CerrarLibro();
        }
    }

    void AbrirLibro()
    {
        libroAbierto = true;
        Time.timeScale = 0;


    }
   
    void CerrarLibro()
    {
        
        Time.timeScale = 1;
     

    }

}
