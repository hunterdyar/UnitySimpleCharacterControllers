using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinStickInput : MonoBehaviour
{
    TwinStickCharacterController twinStickCharacter;
    TwinStickWeaponManager twinStickWeapons;
    void Start()
    {
        twinStickCharacter = GetComponent<TwinStickCharacterController>();
        twinStickWeapons = GetComponent<TwinStickWeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //For a proper twin stick game I wouldn't use axis, because they fade back to center instead of snapping instantly.
        twinStickCharacter.Move(new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")));

        if(Input.GetMouseButtonDown(0)){
            twinStickWeapons.Fire();
        }
    }
}
