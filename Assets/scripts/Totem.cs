using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    public float health;
    public static int  totemCount;
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            totemCount++;
            Die();
            
        }



        void Die()
        {


            Destroy(gameObject);

        }

    }
}
