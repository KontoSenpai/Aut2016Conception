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
        GameObject player = Instantiate(playerObject, transform.position, transform.rotation) as GameObject;
        player.transform.parent = transform;
        player.name = "Player " + index;
		player.tag = "Player";
		player.GetComponent<PlayerStatus>().SetID(index);
    }
}
