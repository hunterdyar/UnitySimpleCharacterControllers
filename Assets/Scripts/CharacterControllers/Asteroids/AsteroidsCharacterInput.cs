using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blooper;
public class AsteroidsCharacterInput : MonoBehaviour
{
    AsteroidsCharacterController controller;//"using blooper" up top? That lets us reference this thing from another namespace.
    //We could also do "Blooper.AsteroidsCharacterController controller;"
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

