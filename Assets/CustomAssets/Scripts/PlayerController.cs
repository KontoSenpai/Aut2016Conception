using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float maxSpeed = 10f;
	bool facingRight = true;
	Rigidbody2D rb;

	Animator anim;

	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	public float jumpForce = 700f;
	bool hasJumped = false;

	private bool hasCollided = false;

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();

	}
	/*
	void Update () {
		if (grounded && Input.GetKeyDown (KeyCode.Space)) {
			anim.SetBool ("Ground", false);
			rb.AddForce (new Vector2 (0, jumpForce));

		} 

	}
	*/

	// Update is called once per frame
	void FixedUpdate () 
	{
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool ("Ground", grounded);

		anim.SetFloat ("vSpeed", rb.velocity.y);

		float move = Input.GetAxis ("Horizontal");

		anim.SetFloat ("hSpeed", Mathf.Abs(move));
		/*
		if (grounded && !hasJumped) {
			rb.AddForce (Vector2.up * jumpForce);
			hasJumped = true;
		}
		else {
			hasJumped = false;
		}
		*/
		rb.velocity = new Vector2 (move * maxSpeed, rb.velocity.y);



		if (move > 0 && !facingRight) {
			Flip ();
		} 
		else if (move < 0 && facingRight) {
			Flip();
		}
	}

	//Flip the character
	void Flip() 
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.collider.gameObject.tag == "Ground" && !hasCollided)
		{
			hasCollided = true;
			float y = rb.velocity.y;
			Vector3 tmp = rb.velocity;
			tmp.y = 0.0f;
			rb.velocity = tmp;
			rb.AddForce(Vector2.up * jumpForce);
		}
	}
	void OnCollisionExit2D(Collision2D col)
	{
		if (col.collider.gameObject.tag == "Ground" && hasCollided)
		{
			hasCollided = false;
		}
	}

}
