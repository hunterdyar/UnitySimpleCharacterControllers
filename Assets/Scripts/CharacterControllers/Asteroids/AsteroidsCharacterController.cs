﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blooper{

    //Simple asteroids character controller using unity Rigidbody for collisions and movement.
    //Allows the player to throttle and turn, but both have friction.
    //i.e. we are adjusting the characters ~acceleration~, not their velocity or position.


    //For fine-tuning the character, the properties you want to mess with are the rigidbody's angular/linear drag and this controllers rotationForce.
    
    //First, lock the mass in. Keeping the player with a mass of 1 and changing the mass of everything else that you may bounce off of (or knock around) is a good enough starting point.

    //High drag and high rotation forces will feel "snappier", but may be hard to be accurate.
    //Low drag and high rotation will probably spin out of control.
    //low drag and low rotation will be slow to rotate, like you're getting a powerful thing up and running.
    //high drag and low rotation will feel sluggish.
    //ANyway just fiddle a bunch. The example scene isn't really a starting point, i didn't spend much time with that.
    public class AsteroidsCharacterController : MonoBehaviour
    {
        [Header("Character Settings")]
        [Tooltip("Controls how quickly the character can rotate")]
        public float rotationForce;
        [Tooltip("Controls movement acceleration")]
        public float movementForce;
        [Tooltip("When false player will not be able to throttle in opposite direction they are facing.")]
        public bool allowBackwardsMovement = true;

        [HideInInspector]//Even though the variable is public, it will not appear as editable in the inspector.
        public float damageTaken = 0;//We aren't actually using this code, but perhaps I want to make it public so other scripts can how much damage this one has taken?
        //okay, fine, it would probably be private but here is an example of "HideInInspector". It helps keep things tidy.
        Rigidbody2D rb;
        
        void Awake(){
            rb = GetComponent<Rigidbody2D>();//This should be familiar by now.
        }
        void Start()
        {
            damageTaken = 0;

            //The following are examples of some tests of likely mistakes when we are setting up the controller.

            //If you are worried about the performance impact of tests like this, there are ways to make sections of your code not even compile in finished builds! Neat!
            //We also don't need to do it on Start, we could use one of the event functions for the editor, and these could get called without us even entering play mode.
            if(rotationForce == 0){
                Debug.LogWarning(gameObject.name+" rotation force is 0. They can't rotate!",this);
            }
            if(movementForce == 0){
                Debug.LogWarning(gameObject.name+" movementForce is 0! They can't move.",this);
            }
            if(rb.gravityScale != 0){
                Debug.LogWarning(gameObject.name + " has gravity. You should probably go into the rigibody and make appropriate adjustments.");
            }
        }

        public void Rotate(float rotationInput)//Lets receive a value between -1 and 1, where positive is clockwise and negative is counterClockwise.
        {
            //"Sanitize" our input, since its... input.
            rotationInput = Mathf.Clamp(rotationInput,-1f,1f);//if the value is greater than or less than 1, we will make it -1 or 1 respectively. Can't be outside of our range.
            rotationInput = -rotationInput*rotationForce;//The negative is because it was rotating the wrong way. I don't know math I just tested it and went "hey thats backwards".
            rb.AddTorque(rotationInput,ForceMode2D.Force);
            //its that easy! Thanks addTorque for being literally exactly what we need.
        }
        public void Throttle(float throttleInput){
            if(allowBackwardsMovement){//if this bool is true...
                throttleInput = Mathf.Clamp(throttleInput,-1f,1f);//same as above
            }else{
                throttleInput = Mathf.Clamp(throttleInput,0,1);//if we don't allow backwards movmeent, we will clamp it to a minimum value of 0.
                //Negative forces are just positive forces in the opposite direction. 
            }
            throttleInput = throttleInput*movementForce;//give us some POWER
            Vector2 forceVector = transform.up*throttleInput;//Our forward direction is where we want to go.
            rb.AddForce(forceVector,ForceMode2D.Force);
        }
        public void OnCollisionEnter2D(Collision2D collision){//dont forget we are usng Physics2D, not Physics!
            
            damageTaken = damageTaken + collision.relativeVelocity.magnitude;
            
            //Very simple knock-back. Wall pushes player away from themselves.
            Vector2 knockback = collision.GetContact(0).normal * collision.GetContact(0).normalImpulse*2;
            rb.AddForce(knockback,ForceMode2D.Impulse);//Add a "knock back" force.
        }
    }

}
