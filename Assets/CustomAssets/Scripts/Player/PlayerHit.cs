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
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.transform.tag == "Head" && col.gameObject.transform.position.y < gameObject.transform.position.y)
        {
            if( col.gameObject.GetComponentInParent<PlayerStatus>().IsVulnerable())
                col.gameObject.GetComponentInParent<PlayerStatus>().Hurt();
            gameObject.GetComponentInParent<PlayerController>().Jump(400f);
        }
    }
}
