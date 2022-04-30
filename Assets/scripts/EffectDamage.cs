using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDamage : MonoBehaviour
{
    public float damage = 3f;
    private void Start()
    {
      
    }


    private void Update()
    {
         
    }


    private void OnTriggerStay(Collider other)
    {
        other.gameObject.GetComponent<HealthSystem>().ApplyDamage(damage * Time.deltaTime*1);
    }

}
