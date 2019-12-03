using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Blooper{
    public class GridSnapCharacterInput : MonoBehaviour
    {
        GridSnapCharacterController controller;
        void Awake()
        {
            controller = GetComponent<GridSnapCharacterController>();    
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

}