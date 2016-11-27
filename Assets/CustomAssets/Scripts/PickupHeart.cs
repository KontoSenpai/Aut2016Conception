using UnityEngine;
using System.Collections;

public class PickupHeart : MonoBehaviour
{
    //use for floating the pickup
    private float y0;
    public float amplitudeAnimation = 0.1f;
    public float timeAnimation = 2f;

    // Use this for initialization
    void Start()
    {
        GetComponentInParent<SpawnPickUp>().pickupIsActif = true;
        y0 = this.transform.position.y;
    }

    // Update is called once per frame
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

            AddHeart(other.transform.parent.gameObject);

            // bool for the spawner of pickup
            GetComponentInParent<SpawnPickUp>().pickupIsActif = false;

            Destroy(gameObject);
        }
    }

    public void AddHeart(GameObject player)
    {
        int currentlife = player.GetComponent<PlayerStatus>().currentLife;

        if (currentlife < 3)
        {
            player.GetComponent<PlayerStatus>().SetCurrentLife(1);
            
            //Update player HUD
            GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
            gameController.GetComponent<HUD>().UpdateHearts(player);

        }

    }
}
