using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMinimap : MonoBehaviour
{
    bool gotMinimap = false;
    public GameObject Minimap;
    public GameObject DecorativeMap;

    private void Start()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKey(KeyCode.E) && gotMinimap == false)
        {

            Minimap.SetActive(true);
           
          
            gotMinimap = true;
            Destroy(DecorativeMap);

        }
    }
}
