using UnityEngine;
using System.Collections;

public class PickupInvincible : MonoBehaviour {

    //variable sound

    public float activeTime = 2;
    

    //use for floating the pickup
    private float y0;
    public float amplitudeAnimation = 0.1f;
    public float timeAnimation = 2f;
    void Start()
    {
        GetComponentInParent<SpawnPickUp>().pickupIsActif = true;
        y0 = this.transform.position.y;
    }

    void Update()
    {
        // make float the pickup
        transform.position = new Vector3(transform.position.x, y0 + (amplitudeAnimation * Mathf.Sin(timeAnimation * Time.time)), transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.tag == "Player")
        {
            //PlaySound 
            GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
            gameController.GetComponent<GameController>().PlaySound("Pickup");

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