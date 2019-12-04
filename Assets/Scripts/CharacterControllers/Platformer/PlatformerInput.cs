using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blooper;

[RequireComponent(typeof(PlatformerCharacterController))]
public class PlatformerInput : MonoBehaviour
{
    PlatformerCharacterController controller;
    bool extraJump;
    int jumps = 0;
    public float movementForce;
    void Awake(){
        controller = GetComponent<PlatformerCharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        if(controller.grounded){
            jumps = 0;
        }

        if(jumps<2 && Input.GetKeyDown(KeyCode.UpArrow)){
            controller.Jump();
            jumps++;
        }
        controller.PushHorizontal(Input.GetAxis("Horizontal")*movementForce);
    }
}
