using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject playerObject;
    private int playerIndex;


    public GameObject Spawn(int index)
    {
        transform.name = "SpawnPlayer " + index;
        GameObject player = Instantiate(playerObject, transform.position, transform.rotation) as GameObject;
        player.transform.parent = transform;
        player.name = "Player " + index;
		player.tag = "Player";
		player.GetComponent<PlayerStatus> ().SetID (index);

		//Initialize player HUD
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		gameController.GetComponent<HUD>().UpdateHearts(player);
        return player;
    }
}
