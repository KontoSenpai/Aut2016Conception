using UnityEngine;
using System.Collections;

public class SpawnPickUp : MonoBehaviour {


    public GameObject[] pickUp;
    public float SpawnTimer;
    public bool pickupIsActif = true;
    public float timebeforespawn;
	// Use this for initialization
	void Start ()
    {
        SpawnPickup();
        timebeforespawn = Time.time + SpawnTimer;
	}	
	// Update is called once per frame
	void Update ()
    {

        if (Time.time >= timebeforespawn && !pickupIsActif)
        {
            SpawnPickup();
        }
        else if(pickupIsActif)
        { 
            timebeforespawn = Time.time + SpawnTimer;
        }
    }

    public void SpawnPickup()
    {
        GameObject pickup = Instantiate(pickUp[Random.Range(0, pickUp.Length)], transform.position, transform.rotation) as GameObject;
        pickup.transform.parent = transform;
        pickup.name = "Pickup ";
    }
}
