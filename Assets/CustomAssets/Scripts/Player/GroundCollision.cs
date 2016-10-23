using UnityEngine;
using System.Collections;

public class GroundCollision : MonoBehaviour {

    private bool hasCollided;
    private float currentTime;
    private float jumpForce = 600f;
    float delay = 0.25f;

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
        if (Time.time - currentTime >= delay && hasCollided == true)
        {
            Vector3 tmp = rb.velocity;
            tmp.y = 0.0f;
            rb.velocity = tmp;
            rb.AddForce(Vector2.up * jumpForce);
            hasCollided = false;
            anim.SetBool("Ground", false);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.gameObject.tag == "Ground" && !hasCollided && rb.velocity.y == 0)
        {
            currentTime = Time.time;
            hasCollided = true;
            anim.SetBool("Ground", true);
        }
    }
}
