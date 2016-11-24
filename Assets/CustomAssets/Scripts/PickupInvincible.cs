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
        if (other.transform.parent.tag == "Player")
        {
            SetInvincible(other.transform.parent.gameObject);
            StartCoroutine(wait(other.transform.parent.gameObject));
            // bool for the spawner of pickup
            GetComponentInParent<SpawnPickUp>().pickupIsActif = false;
            //put the pickup to invisible 
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Renderer>().enabled = false;
        }
    }

    public void SetInvincible(GameObject player)
    {
        player.GetComponent<PlayerStatus>().SetVulnerability(false);
        player.GetComponent<PlayerStatus>().invulnerablePickup = true;
    }

    private IEnumerator wait(GameObject player)
    {
        yield return new WaitForSeconds(activeTime);

        player.GetComponent<PlayerStatus>().SetVulnerability(true);
        player.gameObject.GetComponent<PlayerStatus>().invulnerablePickup = false;

        Destroy(gameObject);
    }



}