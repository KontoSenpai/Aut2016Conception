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
        if(other.transform.parent.tag=="Player")
            changeSpeed(other.transform.parent.gameObject);
    }

    public void changeSpeed(GameObject player)
    {

        //Save the preview speed and set the new
        maxSpeed = player.GetComponent<PlayerController>().GetMaxSpeed();
        player.GetComponent<PlayerController>().SetMaxSpeed(newMaxSpeed);

        //wait active time and set to the preview speed and destroy the pickup
        StartCoroutine(wait(player, maxSpeed));

        // bool for the spawner of pickup
        GetComponentInParent<SpawnPickUp>().pickupIsActif = false;

        //put the pickup to invisible 
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Renderer>().enabled = false;
    }

    private IEnumerator wait(GameObject player, float maxSpeed)
    {
        yield return new WaitForSeconds(activeTime);
        player.GetComponent<PlayerController>().SetMaxSpeed(maxSpeed);
        Destroy(gameObject);
    }
}
