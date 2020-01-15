using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinStickCharacterController : MonoBehaviour
{

    Rigidbody2D rb;
    public float speed;    
    private Vector3 mousePos;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x,mousePos.y,transform.position.z);//Set the z position of this lookat position to the same as the player, because 2d.
        TurnToFaceMouse();
    }
    public void Move(Vector2 dir){
        rb.velocity = speed*dir;
    }

    //This is probable the hardest part, coding wise, of the controller.
    //It requires knowledge of vectors and quaternions, but also knowledge of what funcitons unity has available for us to use.
    void TurnToFaceMouse(){

        //Quaternion's look rotation is an interesting one. We give it two vector3's, one defining a direction that the blue axis (forward) should be, and the other defining the green axis.
        //So we give it Vector3.Forward for forward, which is because this is 2d, we never really want the player's forward direction to change.
        //in a 3d game, a player standing up and looking around would never really want the green y axis to change.
        //For the upward direction for us, thats actually just hte green axis, and thats what we want to face the mouse. So we give it the direction, which we can get with subtraction.
        //unity.hdyar.com has a link to a bit about subtracting vectors to get directions.
        transform.rotation = Quaternion.LookRotation(Vector3.forward,mousePos-transform.position);
    }
}
