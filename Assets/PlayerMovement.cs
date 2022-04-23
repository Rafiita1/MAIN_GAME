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
    public bool Punch;
    public Animator anim;
    public int i = 0;
    public float cooldownE = 1f;
    public float cooldownG = 1f;
    public Text Ecooldown;
    public int hedasheao = 0;
    public int time = 6;
   
   

    public float jumpHeight = 3f;

    Vector3 velocity;
    bool isGrounded;
   

    // Update is called once per frame
    void Update()
    {
        if (hedasheao == 1)
        {
            Ecooldown.text = time.ToString();
            
            if (time <= 0)
            {
                Ecooldown.text = "";
                hedasheao = 0;
                
            }
        }
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.Mouse0) && i == 0)
        {
            anim.SetBool("Punch", true);
            Invoke("Falso", 0.8f);
            Invoke("CooldownDisparo", 1.4f);
            i = 2;
        }


        if (Input.GetKeyDown(KeyCode.Mouse3) && cooldownE == 1f)
        {
            cooldownE = 0f;
            speed = 90f;
            hedasheao = 1;
            Invoke("CooldownActivate", 5f);
            StartCoroutine(Pasar(0));
            Invoke("Parar", 0.2f);
            
            
        }
        if (Input.GetKey(KeyCode.Mouse1) && cooldownG == 1f)
        {
            anim.SetBool("Guardia", true);
            Invoke("PararGuardia", 3f);
            cooldownG = 0f;
        }
    }
    void Falso()
    {
        anim.SetBool("Punch", false);
    }
    void Parar()
    {
        speed = 8f;
        
        
        
    }
    void CooldownActivate()
    {
        cooldownE = 1f;
    }
    void CooldownDisparo()
    {
        i = 0;
    }
    void PararGuardia()
    {
        Invoke("GuardiaCooldown", 6f);
        anim.SetBool("Guardia", false);
    }
    void GuardiaCooldown()
    {
        cooldownG = 1f;
    }
    
    IEnumerator Pasar (int number)
    {
        time = 6;
        while (time > 0)
        {
            time -= 1;
            yield return new WaitForSeconds(1.0f);
        }
        
    }
   
}
