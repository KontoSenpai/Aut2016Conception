using UnityEngine;
using System.Collections;

public class PickupSpeed : MonoBehaviour
{
   // public AudioClip pickupSound;
   // private AudioSource source;

    public float newMaxSpeed = 10.0f;
    public float activeTime =2;
    private float maxSpeed;
    public bool isActif = true;

    void Start()
    {
        //source = gameObject.GetComponent<AudioSource>();
        GetComponentInParent<SpawnPickUp>().pickupIsActif = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag=="Player")
            changeSpeed(other);
    }

    public void changeSpeed(Collider2D player)
    {

        //Save the preview speed and set the new
        maxSpeed = player.gameObject.GetComponentInParent<PlayerController>().GetMaxSpeed();
        player.gameObject.GetComponentInParent<PlayerController>().SetMaxSpeed(newMaxSpeed);

        //wait active time and set to the preview speed and destroy the pickup
        StartCoroutine(wait(player, maxSpeed));

        // bool for the spawner of pickup
        GetComponentInParent<SpawnPickUp>().pickupIsActif = false;

        //put the pickup to invisible 
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Renderer>().enabled = false;


    }

    private IEnumerator wait(Collider2D player, float maxSpeed)
    {
        yield return new WaitForSeconds(activeTime);

        player.gameObject.GetComponentInParent<PlayerController>().SetMaxSpeed(maxSpeed);

        Destroy(gameObject);
    }


    
}
