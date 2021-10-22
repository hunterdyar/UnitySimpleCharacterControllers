using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    namespace Blooper.TwinStick{
    public class TwinStickCharacterController : MonoBehaviour
    {

        Rigidbody2D rb;
        public float speed;    
        private Vector3 mousePos;
        private Quaternion goalRotation;
        private Vector2 goalVelocity;
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
            //dir.Normalize();//sanitize input if necessary
            
            //store it here, this is presumably applied in Update
            goalVelocity = speed*dir;
        }

        //This is probable the hardest part, coding wise, of the controller.
        //It requires knowledge of vectors and quaternions, but also knowledge of what funcitons unity has available for us to use.
        void TurnToFaceMouse(){

            //Quaternion's look goalRotation is an interesting one. We give it two vector3's, one defining a direction that the blue axis (forward) should be, and the other defining the green axis.
            //So we give it Vector3.Forward for forward, which is because this is 2d, we never really want the player's forward direction to change.
            //in a 3d game, a player standing up and looking around would never really want the green y axis to change.
            //For the upward direction for us, thats actually just hte green axis, and thats what we want to face the mouse. So we give it the direction, which we can get with subtraction.

            //We store it here (called from Update) but we will apply it in fixedUpdate
            goalRotation = Quaternion.LookRotation(Vector3.forward,mousePos-transform.position);
        }

        //We want to apply the changes that we get from update in fixedUpdate, so that the physics system is nice and happy. Bug prevention aside, it keeps things smooth and not jitter-free.
        private void FixedUpdate()
        {
            //using MovePosition and MoveRotation (instead of directly changing the position or the rotation from the transform function) ensures that collisions will be calculated correctly.

            //fixedDeltaTime is just the fixedUpdate 'version' of deltaTime.
            //fixedDeltaTime is important here, We want to move velocity units per second, not units per physics-tick. Multiplying by the amount of time that has passed since the last FixedUpdate lets this happens. 
            
            rb.MovePosition(rb.position+goalVelocity*Time.fixedDeltaTime);
            rb.MoveRotation(goalRotation);
            
            //alternatively, we could set rb.velocity = velocity, which will also calculate collisions correctly. We would still need to set MoveRotation ourselves (or calculate angular velocity and set it directly) 
        }
    }
}

