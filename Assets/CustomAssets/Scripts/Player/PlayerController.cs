using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private float maxSpeed = 5f;
    private float currentSpeed = 0f;

    private bool facingRight = true;
	private Rigidbody2D rb;
    private int playerID;
    Animator animator;

    void Start () 
	{
		rb = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator>();
		playerID = gameObject.GetComponent<PlayerStatus> ().GetID ();
	}

    void Update()
    {
        float move = 0.0f;
        if (Input.GetJoystickNames().Length > 0)
        {
            move = Input.GetAxis("Horizontal_P" + playerID);
        }
        else
        { 
            move = Input.GetAxis("Horizontal_C" + playerID);
        }


        // HERE MAKE CODE FOR ALLOURDISSEMENT
        if (Input.GetButtonUp("Allourdissement_P"+ playerID))
        { print("Bite" +playerID); }

        if (currentSpeed <= maxSpeed)
            AdjustSpeed(move);

        rb.velocity = new Vector2 (move * currentSpeed, rb.velocity.y);

		if (move > 0 && !facingRight)
			Flip ();
		else if (move < 0 && facingRight)
			Flip();
	}

    private void AdjustSpeed( float move)
    {
        if (move == 0)
            currentSpeed = 0;
        else if( (move > 0 && facingRight) || (move < 0 && !facingRight))
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, 0.3f);
        else if( (move > 0 && !facingRight) || (move < 0 && facingRight))
            currentSpeed = Mathf.Lerp( currentSpeed, maxSpeed, 0.3f);
    }

	//Flip the character
	private void Flip() 
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    public void SetAnimation(string param, bool value)
    {
        animator.SetBool(param, value);
    }

    public void Jump( float force)
    {
        Vector3 tmp = rb.velocity;
        tmp.y = 0.0f;
        rb.velocity = tmp;
        rb.AddForce(Vector2.up * force);
    }

    public float GetMaxSpeed() { return maxSpeed;}
    public void SetMaxSpeed(float max) { maxSpeed = max; }
}
