using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blooper{

    
    //Okay so this is pretty bad, but I don't have time to polish up.
    //TODO snap player to ground position (the hit.point from the raycast.)
public class PlatformerCharacterController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private Vector2 gravity;
    
    [SerializeField]
    private LayerMask solidLayers;
    [SerializeField] private float characterWidth;
    [SerializeField] private float characterHeight;
    [Tooltip("Smaller the skin width, less buggy when clipping through corners, but greater means greater max speed before things break")]
    [SerializeField] private float skinWidth;
    [SerializeField] private float jumpSpeed;
    //Properties
    [Header("Status")]
    public bool grounded;
    private Vector2 velocity;
    private Vector2 externalForces;
    [Header("Debugging")]
    public bool showWarnings = true;
    public bool showcastLines = true;
    void Start()
    {
        if(skinWidth == 0 && showWarnings){
            Debug.LogWarning("SkinWidth is 0! Weird. Character may have collision errors.",this);
        }
        if(jumpSpeed == 0 && showWarnings){
            Debug.LogWarning("JumpSpeed is 0! Weird. Character can't jump. Is this intended?",this);
        }
        if(Physics2D.queriesStartInColliders && showWarnings){
            //We could manually set this to true here but thats super rude. Adding a component, wrong one, meant another one, and now project settings are different? 
            Debug.LogWarning("Physics2D queriesStartInColliders needs to be false! Change this in the Physics2D settings.",this);
        }
        //Initiate
        velocity = Vector2.zero;
        externalForces = Vector2.zero;   
    }

    
    void Update()
    {

        //Apply gravity.
        velocity = velocity+gravity*Time.deltaTime;
        //Apply Forces
        velocity = velocity+externalForces*Time.deltaTime;

        //Check for collisions
        bool vimpact = false;
        bool himpact = false;
        float groundYPos = 0;
        float wallXPos = 0;
        //Check for collision down
        if(velocity.y != 0){//if we are moving vertically.
            int sign = 0;
            if(velocity.y>0){
                sign = 1;
            }else{
                sign = -1;
            }
            float horizOffset = characterWidth/2 -0.02f;
            float vertPos = transform.position.y +sign*characterHeight/2 - sign*skinWidth;
            Vector2[] downCasts = new Vector2[]{new Vector2(transform.position.x-horizOffset,vertPos),
                new Vector2(transform.position.x+horizOffset,vertPos),
                new Vector2(transform.position.x,vertPos)};
            //3 Vectors here are origin, bottom left, and bottom right; of the character.
            float verticalDistanceTraveling = velocity.y*Time.deltaTime+skinWidth+0.001f;
            foreach(Vector2 origin in downCasts){
                RaycastHit2D hit = Physics2D.Raycast(origin,new Vector2(0,sign),verticalDistanceTraveling,solidLayers);
                
                //Debugging
                if(showcastLines){
                    Ray2D ray = new Ray2D(origin,new Vector2(0,sign));
                    Debug.DrawLine(origin,ray.GetPoint(verticalDistanceTraveling),Color.red);
                }
                //Check for collisions.
                if(hit.collider != null){
                    groundYPos = hit.point.y+characterHeight/2;
                    vimpact = true;
                    break;
                }
            }
        }


            ///Check For Horizontal Collisions
        if(velocity.x != 0){//if we are moving downwards.
            int sign = 0;
            if(velocity.x>0){
                sign = 1;
            }else{
                sign = -1;
            }

            float horizPos = transform.position.x + sign*characterWidth/2-sign*skinWidth;
            float vertOffset = characterWidth/2 - 0.04f;//0.01 so we don't start raycasting from the ground, which will be "in" the ground collide with it? bad.
            Vector2[] sideCasts = new Vector2[]{new Vector2(horizPos,transform.position.y),
                new Vector2(horizPos,transform.position.y+vertOffset),
                new Vector2(horizPos,transform.position.y-vertOffset)};
            //3 Vectors here are side middle,side top, side bottom
            float horizontalDistanceTraveling = velocity.y*Time.deltaTime+skinWidth+0.001f;
            himpact = false;
            foreach(Vector2 origin in sideCasts){
                RaycastHit2D hit = Physics2D.Raycast(origin,new Vector2(sign,0),horizontalDistanceTraveling,solidLayers);
                //Debugging
                if(showcastLines){
                    Ray2D ray = new Ray2D(origin,new Vector2(sign,0));
                    Debug.DrawLine(origin,ray.GetPoint(horizontalDistanceTraveling),Color.blue);
                }
                //Check for collisions.
                if(hit.collider != null){
                    wallXPos = hit.point.x-sign*characterWidth/2;
                    himpact = true;
                    break;
                }
            }
        }
            ///Update Velocity Accordingly
        if(vimpact){
            //Stop moving vertically.
            velocity.y = 0;
            //Snap to ground.
            //The +characterHeight/2 here assumes that the origin of the player is the center of the object.
            transform.position = new Vector3(transform.position.x,groundYPos,transform.position.y);
            //Update property.
            grounded = true;
        }else{
            grounded = false;
        }

        if(himpact){
            //Snap to "ground"... snap to wall.
            transform.position = new Vector3(wallXPos,transform.position.y,transform.position.y);
            velocity.x = 0;//Remember to only stop the vertical momentum when we hit something.
        }
        //finally we can move the player
        Move(velocity);
    }

    public void OverrideVelocity(Vector2 newVel){
        velocity = newVel;
    }
    public void PushHorizontal(float force){
        externalForces.x = force;
    }
    public void Jump(){
        velocity.y = jumpSpeed;
    }
    void Move(Vector2 vel){
        //Velocity is distance over time.
        transform.position = transform.position+(Vector3)velocity*Time.deltaTime;
    }
    
}//end class
}//end namespace