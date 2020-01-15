using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 5;
    public int damage = 1;
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up*bulletSpeed;
        Destroy(gameObject,10);//Destroy myself after 10 seconds. Prevents game from eventually crashing with thousands of bullets floating around.
    }

    void OnCollisionEnter2D(Collision2D collision){//EASY MISTAKE ALERT: note the "2d" in this function. It's different then OnCollisionEnter.
        if(!collision.gameObject.CompareTag("Player")){//We would want to use layers to handle this collision what-with-what stuff in a larger project.
            if(collision.gameObject.GetComponent<Enemy>() != null){//Does the thing we hit have an "enemy" component?
                collision.gameObject.GetComponent<Enemy>().OnHit(damage);//Tell that thing that we hit it. Let it sort itself out.
            }
            //
            Destroy(gameObject);//Destroy myself.
        }//end if not player
    }//end onCollisionEnter
}
