using UnityEngine;
using System.Collections;

public class GroundCollision : MonoBehaviour {

    private float currentTime;
    private ArrayList slide = new ArrayList();
    private float jumpForce = 600f;
    private float slideForce = 300;
    private float delay = 0.15f;
    private bool canJump = true;

    // Update is called once per frame
    void Update()
    {
        if (slide.Count > 0)
            GetComponentInParent<PlayerController>().Jump(slideForce);
    }

    private IEnumerator WaitForJump( float time)
    {
        yield return new WaitForSeconds(time);
        GetComponentInParent<PlayerController>().Jump(jumpForce);
        GetComponentInParent<PlayerController>().SetAnimation("Ground", false);
        StartCoroutine(WaitForJumping(0.3f));
    }

    private IEnumerator WaitForJumping(float time)
    {
        yield return new WaitForSeconds(time);
        canJump = true;
    }

    /* COLLISIONS
    *
    */
    void OnCollisionEnter2D(Collision2D col)
	{
        GetComponentInParent<PlayerController>().SetCanMove(true);
        if (col.gameObject.tag.Equals("Ground") && gameObject.transform.position.y > col.gameObject.transform.position.y)
		{
			if (canJump)
            {
                canJump = false;
                GetComponentInParent<PlayerController> ().SetAnimation ("Ground", true);
                GetComponentInParent<PlayerController>().SetAnimation("Stomp", false);
                StartCoroutine(WaitForJump(delay));
            }
		}
		else if (col.gameObject.tag.Equals("Sliders") )
        {
            if ( col.gameObject.transform.parent.name.Contains("Half"))
            {
                if (gameObject.transform.position.y < col.gameObject.transform.position.y + 0.3f && (gameObject.GetComponentInParent<PlayerController>().GetMove() < -0.3 || gameObject.GetComponentInParent<PlayerController>().GetMove() > 0.3))
                    slide.Add(col.gameObject);
                else if (gameObject.transform.position.y >= col.gameObject.transform.position.y + 0.3f && canJump)
                {
                    canJump = false;
                    GetComponentInParent<PlayerController>().SetAnimation("Ground", true);
                    GetComponentInParent<PlayerController>().SetAnimation("Stomp", false);
                    StartCoroutine(WaitForJump(delay));
                }
            }
            else if( col.gameObject.transform.parent.name.Contains("Full") && (gameObject.GetComponentInParent<PlayerController>().GetMove() < -0.3 || gameObject.GetComponentInParent<PlayerController>().GetMove() > 0.3))
                    slide.Add(col.gameObject);
        }

	}

    void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.tag == "Sliders")
            slide.Remove(col.gameObject);
    }
}
