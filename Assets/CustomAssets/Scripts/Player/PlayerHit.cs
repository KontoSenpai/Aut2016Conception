using UnityEngine;
using System.Collections;

public class PlayerHit : MonoBehaviour {
    //variable sound
    public AudioClip hitSound;
    public float volumeRange = 1f;

    /* COLLISIONS
    *
    */

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.transform.tag == "Head" && col.gameObject.transform.position.y < gameObject.transform.position.y)
        {
			gameObject.GetComponentInParent<PlayerController>().SetCanMove(true);
						
            if (col.gameObject.GetComponentInParent<PlayerStatus>().IsVulnerable())
            {
                col.gameObject.GetComponentInParent<PlayerStatus>().Hurt();

                //playsound
                AudioSource.PlayClipAtPoint(hitSound, transform.position, volumeRange);
            }              
            gameObject.GetComponentInParent<PlayerController>().Jump(400f);
        }
    }
}
