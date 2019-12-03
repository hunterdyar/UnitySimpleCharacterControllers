using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridSnapCharacterInput : MonoBehaviour
{
    Blooper.GridSnapCharacterController controller;
    //We could also put "using BLooper;" up at the top, and then we wouldn't have to type "Blooper. before every one of the references"
    //Theres only 2 in this script its fine.
    void Awake()
    {
        controller = GetComponent<Blooper.GridSnapCharacterController>();    
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            controller.Move(new Vector2Int(0,1));
        }else if(Input.GetKeyDown(KeyCode.DownArrow)){
            controller.Move(new Vector2Int(0,-1));
        }else if(Input.GetKeyDown(KeyCode.RightArrow)){
            controller.Move(new Vector2Int(1,0));
        }else if(Input.GetKeyDown(KeyCode.LeftArrow)){
            controller.Move(new Vector2Int(-1,0));
        }
    }
}

