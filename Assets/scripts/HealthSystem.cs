using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private float maxHealth = 100f;
    public float currentHealth;
    public static float OnTakeDamage;
    public static float OnDamage;
    


    private void Awake()
    {

        currentHealth = maxHealth;
    }
    private void Update()
    {
       

        if (currentHealth <= 0)
            KillPlayer();
    }

   
    public void ApplyDamage(float dmg)
    {
        currentHealth -= dmg;
        
         

        

    }
    public void Damaged()
    {
        currentHealth -= 30;
        
        

    }
    private void KillPlayer()
    {
        currentHealth = 0;

        print("Dead");

    }



}
