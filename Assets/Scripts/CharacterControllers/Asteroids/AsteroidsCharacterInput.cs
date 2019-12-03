using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Blooper{
    public class AsteroidsCharacterInput : MonoBehaviour
    {
        AsteroidsCharacterController controller;
        void Awake()
        {
            controller = GetComponent<AsteroidsCharacterController>();    
        }

        // Update is called once per frame
        void Update()
        {
            controller.Rotate(Input.GetAxis("Horizontal"));
            controller.Throttle(Input.GetAxis("Vertical"));
        }
    }

}