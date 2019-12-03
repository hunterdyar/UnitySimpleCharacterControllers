using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blooper{
    //GridSnap Character Controller by Hunter Dyar
    //Extremely simple character controller.
    public class GridSnapCharacterController : MonoBehaviour
    {
        public bool canMove = true;//
        public float gridScale = 1f;
        //public Vector3 offset;//I am snapping to the default Unity Grid.
        //We are "storing" the grid information as collision shapes in the scene, and will assume everything is "snapped" to the grid
        //We will also assume the character is moves this scale every frame. What if it was 2 squares wide? DOes it move one or two squares?
        //This controller will handle none of these sorts of edge cases. 

        public LayerMask collisionLayers;//Stores which layers we will collide with for movement.

        public ContactFilter2D interactablesContactFilter;
        //Move takes a vector2Int, which is the same as vector2 but using int's instead of float's, we have no need for float precision) with a grid system.
        //That will save us some potential bugs, and improve the performance by like 0.001%.

        private Collider2D col;
        void Awake(){//Lets get all our component calls in awake.
            col = GetComponent<Collider2D>();
        }
        void Start(){//I did all my component calls in awake, and I could do this too. I'm doing it in Start because of a pattern (first get references, next access them) that isn't necessary here.
        //But if we start using it, we will just HIT LESS BUGS and life will be easier.
            SnapToGrid();
        }
        void FixedUpdate(){
            //Fixed update is for physics things, but NOT input. it will miss input. Dont sweat the details of the two simultaneously running update ticks at different speeds. just remember fixed update for physics engine stuff.

            //We only need to do this after we move
            //or after the world moves. Without getting into event systems where the world tells the player when it's moving, we can just check every frame.
            //Not the most performative, since we are creating a list, but. it's fine. for now.
            //if(!dead){ CheckCurrentLocation(); }//We dont want the death or item pickup or whatever to happen constantly, so we have to remember to destroy the item and ... be dead.
            CheckCurrentLocation();
            
        }
        //Snap the character into the grid.
        void SnapToGrid(){
            //How can we figure out what our grid is? We need to know the scale of our grid, and at least one reference point between "our" grid and unity's grid thats the same value.
            //But that's easy! it's probably origin! 0. 

            //Thinking on one dimension, an axis, our grid is basically an arbitrary scale. a point every x distance. So we have to just find what the nearest x is. Thats.. division, basically!
            //We will divide by gridscale, so the numbers are in our scale. after dividing by gridScale, if its 3, we are at 3 units gridscale. 

            //But what about if we are at 3.3 units gridScale? Lets round! Then multiply out and we will be exactly on our scale.
            //The only thing missing would be some offset we might need. That could be the case depending on where we parent our squares. Is the literal value the bottom left? the center?
            //For this, I like to keep my gameObjects value exactly at whatever grid scale value, so the transform.position is nice and neat, and then if i need an offset, I put the spriteRenderer in a child GameObject that has the offset.
            
            //In a real-world situation I would not create 3 vector3's that only get used once, but this is more readable.
            Vector3 pos = transform.position;
            Vector3 atNormalScale = pos/gridScale;//Now each "unit" is it's location in terms of our scale.
            Vector3 atNormalScaleRounded = new Vector3(Mathf.Round(atNormalScale.x),Mathf.Round(atNormalScale.y),Mathf.Round(atNormalScale.z));
            //Ah, "Mathf". One of the most useful classes. Like getting your hands on a scientific calculator.
            transform.position = atNormalScaleRounded * gridScale;//
        }
        //Takes a Vector2Int for our direction. a vector2int is just a vector2 but with ints instead of floats. No fractions! 
        public void Move(Vector2Int direction){
            //Since we are getting this data from input, ie: the user, let's "sanitize" it and make sure its fine.
            if(direction.magnitude != 1){
                Debug.LogError("direction magnitude not 1. Our we trying multiple moves at once, on a diagonal, or a length more than one square?");
            }
            if(direction.magnitude == 0){
                Debug.LogWarning("Attempted to move 0 units? That's probably not right.");
            }
            if(!canMove){
                return;//This will end the function right now, and none of the following code will end up executing.
            }
            //First move.
            if(CheckDirection(direction)){
                transform.position = transform.position+new Vector3(direction.x,direction.y,0)*gridScale;//Vector2Int can't be implicitly converted to a vector3f, so we will turn the data into a vector3 ourselves.
                //We multiply it by gridScale, because we assume direction is just a magnitude of 1 in whatever direction we are going. 
            }
            
        }
        public bool CheckDirection(Vector2Int direction){
            //Physcs2D.Raycast is great. It, instantly, marches a line out from a point (argument 1), in a direction (argument 2), for so-many units (argument 3), until it hits something (valid things to hit: argument 4). 
            //It then tells us what happened in this neat data-holder called a RaycastHit. The RaycastHit2D just stores info about the collision, kind of like the Collision object that the OnCOllisionEnter event function gives us. 
            RaycastHit2D hit = Physics2D.Raycast(transform.position,(Vector2)direction, gridScale,collisionLayers);
            //Raycast2D and Physics (3D) raycasts have a slightly different syntax, FYI. Can't copy/paste and remove the 2d part, sorry. 
            if(hit.collider != null){
            //One problem is that we hit OURSELF!? We can collide with THIS collider. Physics2D.Raycast doesn't know who is calling the raycast! It has no idea what collider is attached here, or if any.
            //Madness. Solution: Collision layers! We can put the player on a different layer then the background world! Then just set the collisionLayers accordingly: Raycast against background and not with player. 

            //Either set the player to like IgnoreRaycast, or make a layer for Level. 
            //The other option is to go into Physics2D Settings and uncheck "Queries Start In Colliders", which is what I would do if this example project didn't have to support other systems of movement too.
                return false;
            }else{
                return true;
            }
        }

        public void CheckCurrentLocation(){
            List<Collider2D> overlaps = new List<Collider2D>(5);//Making a list isn't performative, we should move this to the top to keep it cached cleanly.
            //Looks at the current position and 
            if(col.OverlapCollider(interactablesContactFilter, overlaps) > 0){
                //OverlapCollider returns an int with the number of objects we are overlapping.

                //Lets loop through them, even through we only suspect there will be one. Probably just one? That's a fast loop!
                foreach(Collider2D interactCol in overlaps){
                    //I would consider putting this in another function that we pass the tag into and it says "got item" or "dead now" but ....
                    if(interactCol.CompareTag("Death")){
                        CharacterDied();
                        break;//ends the loop. If we have death and maybe a pickup item on the same tile, we would order these if statements so it hits the death one first.
                    }else if(interactCol.CompareTag("Item")){
                        //Check if the item has a component like "key" and if it does, maybe run a public function that key component has. Maybe some function that like, destroys a door?
                        Destroy(interactCol.gameObject);//"pick up" the item
                        //some integer score = score+1; or whatever we're doing here.
                    }//etc etc.
                }
            }
        }
        void CharacterDied(){
            
            canMove = false;
            Debug.Log("we ded");
            //This is going to get called like a hundred times. The solution to that is to make a "isDead" public boolean and check if it's true. and if it is, don't keep killing the player.
            //Thats just rude.
        }
    }

}
