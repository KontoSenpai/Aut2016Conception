using UnityEngine;
using System.Collections;

public class GroundCollision : MonoBehaviour {

    private float currentTime;
    private ArrayList timeCollisions = new ArrayList();
    private float jumpForce = 600f;
    private float slideForce = 300;
    float delay = 0.15f;

    Rigidbody2D rb;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(JumpOccurs())
        {
            timeCollisions.Clear();
            GetComponentInParent<PlayerController>().Jump(jumpForce);
            GetComponentInParent<PlayerController>().SetAnimation("Ground", false);
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
                GetComponentInParent<PlayerController>().SetAnimation("Ground", true);
            timeCollisions.Add(Time.time);
        }
        else if(col.gameObject.tag == "Slider")
            GetComponentInParent<PlayerController>().Jump(slideForce);
    }

    void OnCollisionExit2D(Collision2D col)
    {
    }
}
