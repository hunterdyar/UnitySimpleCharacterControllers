using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Blooper.TwinStick{
    public class TwinStickWeaponManager : MonoBehaviour
    {
        public GameObject bulletPrefab;

        //We could have multiple weapons by storing prefabs in a list, and then grabbing whatever the "active" one is.
        //List<GameObject> weapons;
        //int currentWeaponIndex;

        public float fireSpeed;
        [HideInInspector]
        float timer = 0;
        void Update(){
            timer = timer+Time.deltaTime;//Counting in seconds. We could also increment by one and count in frames.
        }
        public void Fire()
        {
            if(timer > fireSpeed){
                Vector3 spawnPos = transform.position;
                spawnPos = spawnPos + transform.up/3;//So the bullet was spawning inside the center of the player and not at the point of the triangle, so i'm offsetting it.
                
                GameObject bullet = GameObject.Instantiate(bulletPrefab,spawnPos,transform.rotation);//Create a bullet prefab at our location with our rotation.
                //We could use Quaternion.identity for the rotation, but if we want to shoot arrows or missiles or pointed things, we will want them rotated.
                
                //If we wanted to keep things in fewer scripts, we could just apply our forces from this script:
                //bullet.GetComponent<Rigidbody>().velocity = asdf;
                //Instead, we will let the bullet take care of itself, which we can do because we are rotating it. 
                //If we were not rotating the bullet, we may need to do something like this:
                //bullet.GetComponent<Bullet>().ShootInThisDirection(transform.up);//or whatever the direction is.

                timer = 0;
            }//end if timer
        }//end fire()
    }
}