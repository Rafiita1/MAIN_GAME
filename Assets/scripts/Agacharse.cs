using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agacharse : MonoBehaviour
{
    CharacterController PlayercoL;
    float originalHeight;
    public float reducedHeight;
    // Start is called before the first frame update
    void Start()
    {
        PlayercoL = GetComponent<CharacterController>();
        originalHeight = PlayercoL.height;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        
            Crouch();
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            
                GoUp();
            
        
    }


    void Crouch()
    {
        PlayercoL.height = reducedHeight;


    }

    void GoUp()
    {
        PlayercoL.height = originalHeight;


    }

}
