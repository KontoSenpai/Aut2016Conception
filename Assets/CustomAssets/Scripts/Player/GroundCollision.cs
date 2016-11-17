using UnityEngine;
using System.Collections;

public class GroundCollision : MonoBehaviour {

    private float currentTime;
    private ArrayList timeCollisions = new ArrayList();
    private float jumpForce = 600f;
    private float slideForce = 300;
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
        {
            if (timeCollisions.Count == 0)
                anim.SetBool("Ground", true);
            timeCollisions.Add(Time.time);
        }
        else if(col.gameObject.tag == "Slider")
        {
            rb.AddForce(Vector2.up * slideForce);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
    }
}
