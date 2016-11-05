using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float maxSpeed = 10f;

	private bool facingRight = true;
	private Rigidbody2D rb;
    private int playerID;
    
	private Animator anim;

	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
    float delay = 0.25f;
    float currentTime = 0.0f;
	public LayerMask whatIsGround;

	public float jumpForce = 700f;

	private bool hasCollided = false;

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();

		playerID = gameObject.GetComponent<PlayerStatus> ().GetID ();
	}
	// Update is called once per frame
	void FixedUpdate () 
	{
		//grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		//anim.SetBool ("Ground", grounded);
        /*
        if( Time.time - currentTime >= 0.2f && hasCollided == true)
        {
            Vector3 tmp = rb.velocity;
            tmp.y = 0.0f;
            rb.velocity = tmp;
            rb.AddForce(Vector2.up * jumpForce);
            hasCollided = false;
            anim.SetBool("Ground", false);
        }
        */
        float move = 0.0f;

        try
        {
            move = Input.GetAxis("Horizontal_P" + playerID);
            move = Input.GetAxis("Horizontal");
        }
        catch
        {
            print("Keepo");
        }

        rb.velocity = new Vector2 (move * maxSpeed, rb.velocity.y);

		if (move > 0 && !facingRight)
			Flip ();
		else if (move < 0 && facingRight)
			Flip();
	}

	//Flip the character
	void Flip() 
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

	}
    /*
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.collider.gameObject.tag == "Ground" && !hasCollided)
		{
            anim.SetBool("Ground", true);
            currentTime = Time.time;
			hasCollided = true;
        }
	}
    */
	void OnCollisionExit2D(Collision2D col)
	{/*
		if (col.collider.gameObject.tag == "Ground" && hasCollided)
        {
            hasCollided = false;
            anim.SetBool("Ground", false);
        }
        */
	}


}
