using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    
    
  
    public CharacterController controller;
    public float speed = 8f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask sueloMask;
    public Animator anim;
    public int i = 0;
    public int time = 6;
    public bool run = false;
    public Transform sueloCheck;
    public float sueloDistance = 0.4f;

    public float jumpHeight = 3f;

    Vector3 velocity;
    bool isGrounded;
    bool isSuelo;




    // Update is called once per frame
    private void Start()
    {
       anim = GetComponent<Animator>();
    }

    
    void Update()
    {
      
     

        DontDestroyOnLoad(gameObject);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isSuelo = Physics.CheckSphere(sueloCheck.position, sueloDistance, sueloMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
       

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
      
     


        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        if (run == false && Input.GetKey(KeyCode.LeftShift))
        {
            speed = 8f;


        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            speed = 2f;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            speed = 4f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {

            
            speed = 4f;



        }
        

     
    }
  

  
    
   
   
}
