﻿using UnityEngine;
using System.Collections;

public class GroundCollision : MonoBehaviour {

    private float currentTime;
    private ArrayList timeCollisions = new ArrayList();
    private float jumpForce = 600f;
    float delay = 0.15f;

    Rigidbody2D rb;
    Animator anim;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponentInParent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(JumpOccurs())
        {
            timeCollisions.Clear();
            Vector3 tmp = rb.velocity;
            tmp.y = 0.0f;
            rb.velocity = tmp;
            rb.AddForce(Vector2.up * jumpForce);
            anim.SetBool("Ground", false);
        }
        else
        {
            anim.SetBool("Ground", true);
        }
    }

    private bool JumpOccurs()
    {
        for( int i = 0; i < timeCollisions.Count; i++)
        {
            if( Time.time - (float)timeCollisions[i] >=delay)
                return true;
        }
        return false;
    }

    /* COLLISIONS
    *
    */
    void OnCollisionEnter2D(Collision2D col)
    {
		if (col.gameObject.tag == "Ground")
            timeCollisions.Add(Time.time);
        else if(col.gameObject.tag == "Slider")
        {
            print("kappa");
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
    }
}
