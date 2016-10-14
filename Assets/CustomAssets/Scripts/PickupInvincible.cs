using UnityEngine;
using System.Collections;

public class PickupInvincible : MonoBehaviour {

    // public AudioClip pickupSound;
    // private AudioSource source;

    public float activeTime = 2;
    //public float volumeRange = 1f;


    // Use this for initialization
    void Start()
    {
        //source = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // source.PlayOneShot(pickupSound, volumeRange);

        }

    }

    private IEnumerator wait(Collider2D player, float maxSpeed)
    {

        yield return new WaitForSeconds(activeTime);
    
        Destroy(gameObject);
    }



}