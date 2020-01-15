using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public int health = 3;
    public Color[] colors;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = colors[health];
    }
    public void OnHit(int damage){
        //Note how this script never calls this function. We dont use OnCollisionEnter (we could, tho, that would be fine).
        //Thats so if we wanted fancy bullets that call different functions (instant destroy, freeze, powerup, push-back, whatever), we can let the bullet do that.
        //or so different bullets could do different damage, they could tell the thing they hit how much damage they do by passing it as an argument, as demonstrated.
        
        health = health - damage;
        
        spriteRenderer.color = colors[health];

        if (health <= 0){
            Destroy(gameObject);
        }
        
    }
}
