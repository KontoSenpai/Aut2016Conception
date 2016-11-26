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
        if (JumpOccurs())
        {
            timeCollisions.Clear();
            GetComponentInParent<PlayerController>().Jump(jumpForce);
            GetComponentInParent<PlayerController>().SetAnimation("Ground", false);
        }
        else if (slide.Count > 0)
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
        GetComponentInParent<PlayerController>().SetCanMove(true);
        if (col.gameObject.tag.Equals("Ground") && gameObject.transform.position.y > col.gameObject.transform.position.y)
		{
			if (timeCollisions.Count == 0)
            {
				GetComponentInParent<PlayerController> ().SetAnimation ("Ground", true);
                GetComponentInParent<PlayerController>().SetAnimation("Stomp", false);
            }
			timeCollisions.Add (Time.time);
		}
		if (col.gameObject.tag.Equals("Sliders") && gameObject.GetComponentInParent<PlayerController>().GetMove() != 0)
        {
            if ( col.gameObject.transform.parent.name.Contains("Half"))
            {
                if (gameObject.transform.position.y < col.gameObject.transform.position.y + 0.4f)
                    slide.Add(col.gameObject);
                else
                    timeCollisions.Add(Time.time);
            }
            else if( col.gameObject.transform.parent.name.Contains("Full"))
            {
                if (gameObject.transform.position.y < col.gameObject.transform.position.y + 0.9f)
                    slide.Add(col.gameObject);
                else
                    timeCollisions.Add(Time.time);
            }
        }

	}

    void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.tag == "Sliders")
            slide.Remove(col.gameObject);
    }
}
