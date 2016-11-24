using UnityEngine;
using System.Collections;

public class GroundCollision : MonoBehaviour {

    private float currentTime;
    private ArrayList slide = new ArrayList();
    private ArrayList timeCollisions = new ArrayList();
    private float jumpForce = 600f;
    private float slideForce = 300;
    float delay = 0.15f;

    // Update is called once per frame
    void Update()
    {
        // TO REMOVE IF YOU WANT BOTH PLAYERS TO MOVE
        if (JumpOccurs() && gameObject.GetComponentInParent<PlayerStatus>().GetID() == 1)
        {
            timeCollisions.Clear();
            GetComponentInParent<PlayerController>().Jump(jumpForce);
            GetComponentInParent<PlayerController>().SetAnimation("Ground", false);
        }

        if (slide.Count > 0)
            GetComponentInParent<PlayerController>().Jump(slideForce);
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
		if (col.gameObject.tag == "Ground" && gameObject.transform.position.y > col.gameObject.transform.position.y)
		{
			GetComponentInParent<PlayerController> ().SetCanMove (true);
			if (timeCollisions.Count == 0)
            {
				GetComponentInParent<PlayerController> ().SetAnimation ("Ground", true);
                GetComponentInParent<PlayerController>().SetAnimation("Stomp", false);
            }

			timeCollisions.Add (Time.time);
		}
		if (col.gameObject.tag == "Sliders" && gameObject.transform.position.y <= col.gameObject.transform.position.y)
		{
			GetComponentInParent<PlayerController> ().SetCanMove (true);
			slide.Add (col.gameObject);
		}
	}

    void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.tag == "Sliders")
            slide.Remove(col.gameObject);
    }
}
