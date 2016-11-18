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
        print(col.gameObject.transform.parent.tag);
        if (col.gameObject.transform.parent.tag == "Player" && col.gameObject.transform.position.y > gameObject.transform.position.y)
        {
            if( col.gameObject.GetComponentInParent<PlayerStatus>().IsVulnerable())
                col.gameObject.GetComponentInParent<PlayerStatus>().Hurt();
            col.gameObject.GetComponentInParent<PlayerController>().Jump(400f);
        }
    }
}
