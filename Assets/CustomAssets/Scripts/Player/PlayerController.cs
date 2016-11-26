using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    //variable sound
    public AudioClip slamSound;
    public AudioClip jumpSound;
    public float volumeRange = 1f;

    private float maxSpeed = 5f;
    private float currentSpeed = 0f;
	private float slamForce = 800f;
	private float slamPrepForce = 100f;

    private bool facingRight = true;
	private bool canMove = true;

	private Rigidbody2D rb;
    
	private int playerID;
	private float slamCD = 1.0f;
	private float nextUsage;

    private float move;

    Animator animator;

    void Start () 
	{
		rb = GetComponent<Rigidbody2D> ();
        animator = GetComponentInChildren<Animator>();
		playerID = gameObject.GetComponent<PlayerStatus> ().GetID ();
	}

    void Update()
    {
		if (canMove)
        {
            if( playerID == 1)
            {
                print(move);
            }

			if (Input.GetJoystickNames ().Length > 0)
				move = Input.GetAxis ("Horizontal_P" + playerID);
			else
				move = Input.GetAxis ("Horizontal_C" + playerID);
		}
		if ((Input.GetButtonUp("Allourdissement_P"+ playerID) || (Input.GetKeyDown(KeyCode.H) && playerID == 1)) && Time.time > nextUsage)
            Slam();        
        if( rb.velocity.y > 10)
        {
            Vector2 v = rb.velocity;
            v.y = 10;
            rb.velocity = v;
        }
        if (currentSpeed <= maxSpeed || currentSpeed > maxSpeed)
            AdjustSpeed(move);

        rb.velocity = new Vector2 (move * currentSpeed, rb.velocity.y);

		if ((move > 0 && !facingRight) || (move < 0 && facingRight))
			Flip ();
	}

    private void AdjustSpeed( float move)
    {
        if (move == 0)
            currentSpeed = 0;

        else if ((move > 0 && facingRight) || (move < 0 && !facingRight))
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, 0.3f);
        else if ((move > 0 && !facingRight) || (move < 0 && facingRight))
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, 0.3f);
    }

	/** Function that flip the character sprite if the direction is changed
    *
    */
	private void Flip() 
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    /** Function that is called in the ground collision
    * @Param param : name of the animation
    * @Param value : value of the animation
    */
    public void SetAnimation(string param, bool value)
    {
        animator.SetBool(param, value);
    }

    /** Function that add the force required to jump
    * @Param force : Value of the Y force to be used
    */
    public void Jump( float force)
    {
        Vector3 tmp = rb.velocity;
        tmp.y = 0.0f;
        rb.velocity = tmp;
        rb.AddForce(Vector2.up * force);
        AudioSource.PlayClipAtPoint(jumpSound, transform.position, volumeRange);
    }

	public void Slam()
	{
		SetCanMove (false);	
		nextUsage = Time.time + slamCD;
        SetAnimation("Stomp",true);
		//
		rb.gravityScale = 0;
		Vector3 tmp = rb.velocity;
		tmp.x = 0.0f;
		tmp.y = 0.0f;
		tmp.z = 0.0f;
		rb.velocity = tmp;
		rb.AddForce(Vector2.up * slamPrepForce);

        //playaudio
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        gameController.GetComponent<GameController>().PlaySound("Slam", transform.position);

        StartCoroutine (Delay ());
	}

	IEnumerator Delay()
    {		
		yield return new WaitForSeconds(0.25f);
		rb.gravityScale = 3;
		rb.AddForce(Vector2.up * -slamForce);
	}

    public float GetMaxSpeed() { return maxSpeed;}
    public void SetMaxSpeed(float max) { maxSpeed = max; }

	public void SetCanMove(bool value) { canMove = value; }
	public bool GetCanMove() { return canMove; }
    public float GetMove() { return move; }
}
