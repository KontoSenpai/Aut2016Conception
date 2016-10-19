using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour
{

    public GameObject playerObject;

    private int playerIndex;
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void Spawn(int index)
    {
        transform.name = "SpawnPlayer " + index;
        GameObject boule = Instantiate(playerObject, transform.position, transform.rotation) as GameObject;
        boule.transform.parent = transform;
    }
}
