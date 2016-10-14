using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
   // public AudioClip pickupSound;
   // private AudioSource source;

    public float newMaxSpeed = 30.0f;
    public float activeTime =2;
    private float maxSpeed;
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
        if(other.gameObject.tag=="Player")
        {
            // source.PlayOneShot(pickupSound, volumeRange);
            changeSpeed(other);

            //Destroy(gameObject);
        }

    }

    public void changeSpeed(Collider2D player)
    {
        //Save the preview speed and set the new
        maxSpeed = player.gameObject.GetComponentInParent<PlayerController>().maxSpeed;
        player.gameObject.GetComponentInParent<PlayerController>().maxSpeed = newMaxSpeed;

        //wait active time and set to the preview speed and destroy the pickup
        StartCoroutine(wait(player, maxSpeed));

        //put the pickup to invisible 
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        
    }

    private IEnumerator wait(Collider2D player, float maxSpeed)
    {
        
        yield return new WaitForSeconds(activeTime);

        player.gameObject.GetComponentInParent<PlayerController>().maxSpeed = maxSpeed;
        Destroy(gameObject);
    }


    
}
