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

    public void Spawn(int index)
    {
        transform.name = "SpawnPlayer " + index;
        GameObject boule = Instantiate(playerObject, transform.position, transform.rotation) as GameObject;
        boule.transform.parent = transform;
        boule.name = "Player " + index;
        boule.GetComponent<PlayerController>().SetID(index);
    }
}
