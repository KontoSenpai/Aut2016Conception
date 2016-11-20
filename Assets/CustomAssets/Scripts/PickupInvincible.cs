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
        GetComponentInParent<SpawnPickUp>().pickupIsActif = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // source.PlayOneShot(pickupSound, volumeRange);

            SetInvincible(other);

            //wait active time and set to the preview speed and destroy the pickup
            StartCoroutine(wait(other));

            // bool for the spawner of pickup
            GetComponentInParent<SpawnPickUp>().pickupIsActif = false;
            //put the pickup to invisible 
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Renderer>().enabled = false;
        }

    }

    public void SetInvincible(Collider2D player)
    {
        player.gameObject.GetComponent<PlayerStatus>().SetVulnerability(false);
        player.gameObject.GetComponent<PlayerStatus>().invulnerablePickup = true;
    }

    private IEnumerator wait(Collider2D player)
    {
        yield return new WaitForSeconds(activeTime);

        player.GetComponent<PlayerStatus>().SetVulnerability(true);
        player.gameObject.GetComponent<PlayerStatus>().invulnerablePickup = false;

        Destroy(gameObject);
    }



}