using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blooper.GGJ
{
    public class PlatformerController : MonoBehaviour
    {
        int jumps;
        Vector2 vel;
        float resetAngle = 45;
        float timeSinceGrounded;
        float timeSinceJumpPressed;
        bool grounded;
        Vector3 newLevelPos;
        [Header("Movement Settings")]
        [Space]
        public LayerMask solidThingsLayerMask;
        public float gravity = 30;
        [Min(0)]
        public float movementSpeed = 10;
        [Min(0)]
        public float jumpForce = 10;
        [Range(0,1)]
        public float friction = 0.166f;
        [Header("Movement Details")]
        [Space]
        [Tooltip("1 is single jumps, 2 is double jumps, etc.")]
        public int numberJumpsAllowed =2;
        [Tooltip("Time in seconds to allow the player to press jump before landing, within this timing window they will jump instantly when they land.")]
        public float preCoyoteTime = 0.1f;
        [Tooltip("Timing Window that allows the player to jump after leaving a platform without punishing them. Named for Wile e Coyote running off a cliff and staying afloat for a bit")]
        public float postCoyoteTime = 0.1f;
        [Tooltip("Will the players jumps reset if they hit a wall?")]
        public bool wallsResetJumps = false;
        [Header("Collision Shape")]
        [Space]
        [Tooltip("Use the Context Menu (three dots) and Run 'Define With Sprite Renderer' or 'Define with BoxCollider2D' to more easily edit this Value")]
        public Bounds collisionShape;
        public bool alwaysDefineShapeWithBoxCollider;
        float furthestCameraLeftBound;
        bool ignoreFriction = false;
        Animator animator;
        SpriteRenderer spriteRenderer;
        void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        void Start()
        {
            grounded = false;
            timeSinceGrounded = 0;
            timeSinceJumpPressed = 0;
            jumps = 0;
            if(wallsResetJumps)
            {
                resetAngle = 130;
            }else{
                resetAngle = 45;
            }
            if(alwaysDefineShapeWithBoxCollider)
            {
                DefineBoundsWithBoxCol();
            }
        }

        [ContextMenu("Define Bounds with Sprite Renderer")]
        public void DefineBoundsWithSR()
        {
            if(GetComponent<SpriteRenderer>() == null)
            {
                Debug.LogWarning("Cant define bounds with sprite renderer! no SR found.");
            }
            else
            {   
            collisionShape = GetComponent<SpriteRenderer>().bounds;
            }
        }
        [ContextMenu("Define Bounds with Box Collider 2D Component")]
        public void DefineBoundsWithBoxCol()
        {
                if(GetComponent<BoxCollider2D>() == null)
                {
                    Debug.LogWarning("Cant define bounds with box Collider 2D! no collider found.");
                }
                else
                {
                    collisionShape = GetComponent<BoxCollider2D>().bounds;
                }
        }

        public void Move(Vector2 dir)
        {
            dir.Normalize();
            //Set velocity x axis to movement speed????
            vel = new Vector2(dir.x*movementSpeed,vel.y);
            ignoreFriction = true;//we moved this frame.

            if(dir.x < 0)
            {
                spriteRenderer.flipX = true;
            }else if(dir.x > 0)
            {   spriteRenderer.flipX = false;

            }
        }
        public void Jump()
        {
            timeSinceJumpPressed = 0;
            if(grounded || timeSinceGrounded<postCoyoteTime)
            {
                vel = new Vector2(vel.x,jumpForce);
            }else{
                if(jumps<numberJumpsAllowed-1)//jumps = air-jumps
                {
                    vel = new Vector2(vel.x,jumpForce);
                    jumps++;
                }
            }
        }
        void Update()
        {
            if(grounded)
            {
                jumps = 0;
            }

            timeSinceGrounded = timeSinceGrounded + Time.deltaTime;
            timeSinceJumpPressed = timeSinceJumpPressed + Time.deltaTime;
            //gravity Force
            vel = vel + new Vector2(0,-gravity)*Time.deltaTime;

            //We have to do two orthogonal axis to prevent player from clipping into corners.
            //It was a weird bug.
            CheckCollisionOnAxis(Vector3.right);//also friction
            CheckCollisionOnAxis(Vector3.up);

            if(animator != null)
            {
                animator.SetFloat("speed",vel.magnitude);
                //animator.SetFloat("Xspeed",Mathf.Abs(vel.x));//absolute value of horiz speed
                //animator.SetFloat("Yspeed",Mathf.Abs(vel.y));//absolute value of vert speed

            }
            
            //Now we actually move the player
            transform.position = transform.position+(Vector3)vel*Time.deltaTime;
            
        
        }
        void CheckCollisionOnAxis(Vector3 dir)
        {
            Vector3 nextFrame = Vector3.Project((Vector3)vel,dir)*Time.deltaTime;
            RaycastHit2D hitInfo;
            hitInfo = Physics2D.BoxCast((Vector2)(transform.position+collisionShape.center),collisionShape.size,0,nextFrame.normalized,nextFrame.magnitude,solidThingsLayerMask);
            if(hitInfo.collider != null)
            {
                if(Vector2.Angle(Vector2.up,hitInfo.normal) <= resetAngle)//
                {
                    grounded = true;
                    timeSinceGrounded = 0;
                    if(timeSinceJumpPressed < preCoyoteTime)
                    {
                        //we meant to press the jump button JUST a moment ago, right before landing.
                        StartCoroutine(JumpAtEndOfFrame());
                    }
                }else{
                    grounded = false;
                }
                Vector3 antiForce = Vector3.Project((Vector3)vel,(Vector3)hitInfo.normal);
                CheckFriction(hitInfo.normal);
                vel = vel - (Vector2)antiForce;
            }else
            {
                grounded = false;
            }
        }

        void CheckFriction(Vector3 frictionNormal)
        {
            if(frictionNormal != Vector3.zero && !ignoreFriction)
            {
                if(Vector3.Dot(vel,frictionNormal) != 0)
                {
                    //Generalized Friction
                    Vector3 fricV = (Vector2)Vector3.ProjectOnPlane(vel,frictionNormal);
                    if(fricV.normalized == Vector3.up || fricV.normalized == Vector3.down)
                    {
                        //None of that vertical friction
                        fricV = Vector3.zero;
                    }
                    vel = vel-(Vector2)fricV*friction;
                }
            }
        }
        void LateUpdate()
        {
            ignoreFriction = false;
            // newLevelPos = levelParent.position + Vector3.left*vel.x*Time.deltaTime;
            //levelParent.position = newLevelPos;
        }
        IEnumerator JumpAtEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            Jump();
        }
    }

}