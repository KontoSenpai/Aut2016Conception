using UnityEngine;
using System.Collections;

public class PickupSpeed : MonoBehaviour
{

    public float newMaxSpeed = 13.0f;
    public float activeTime = 4;
    private float maxSpeed;
    public bool isActif = true;
    public ParticleSystem Particle;

    //use for floating the pickup
    private float y0;
    public float amplitudeAnimation = 0.1f;
    public float timeAnimation = 1.2f;

    void Start()
    {
        GetComponentInParent<SpawnPickUp>().pickupIsActif = true;
        y0 = this.transform.position.y;
    }

    void Update()
    {
        // make float the pickup
        transform.position = new Vector3 (transform.position.x, y0 + (amplitudeAnimation * Mathf.Sin(timeAnimation * Time.time)), transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //PlaySound 
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        gameController.GetComponent<GameController>().PlaySound("Pickup");

        if (other.transform.parent.tag=="Player")
            changeSpeed(other.transform.parent.gameObject);
    }

    public void changeSpeed(GameObject player)
    {
        //active particle systeme
        player.GetComponent<ParticleSystem>().enableEmission = true;

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

        player.GetComponent<ParticleSystem>().enableEmission = false;
        Destroy(gameObject);
    }
}
