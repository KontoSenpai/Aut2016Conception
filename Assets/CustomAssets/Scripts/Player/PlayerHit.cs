using UnityEngine;
using System.Collections;

public class PlayerHit : MonoBehaviour {

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    /* COLLISIONS
    *
    */
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && col.gameObject.transform.position.y < gameObject.transform.position.y)
        {
            print("OWNER" + gameObject.transform.position.y);
            print("HIT " + col.gameObject.transform.position.y);
            GetComponentInParent<PlayerController>().Jump(300f);
        }
    }
}
