using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class HUD : MonoBehaviour {
	
	private GameObject[] players; 			//Array of players
	private int[] playerCurrentLife; 		//Array of players's current life
	private int[] playerPreviousLife; 		//Array of players's current life
	private int[] playerMaxLife; 			//Array of players's current life

	private int heartPosXP1 = 50;			//Position X of Player 1 spawn of hearts
	private int heartPosXP2 = 1150;			//Position X of Player 2 spawn of hearts
	private int heartPosY = -100;			//Position Y of Players spawn of hearts
	private int threshold = 100;			//Space between hearts

	private List<GameObject> heartsListP1;	//List of Player 1 hearts currently shown
	private List<GameObject> heartsListP2;	//List of Player 2 hearts currently shown

	private Transform hud;

	private float canvasHight;
	private float canvasWidth;

	public Transform canvas;
	public GameObject heartPrefab;			//Prefab of heart showing in the UI


	void Start() {

		//Get player using the tag "Player" sort alphabeticaly and then send as an array
		players = GameObject.FindGameObjectsWithTag("Player").OrderBy(go =>go.name).ToArray();
		//Debug.Log (GameObject.FindGameObjectsWithTag("Player").ToString());
		playerCurrentLife = new int[players.Length];
		playerPreviousLife = new int[players.Length];
		playerMaxLife = new int[players.Length];

		canvasHight = canvas.GetComponent<RectTransform> ().rect.height;
		canvasWidth = canvas.GetComponent<RectTransform> ().rect.width;

		heartsListP1 = new List<GameObject>();
		heartsListP2 = new List<GameObject>();

		for (int i = 0; i<players.Length; i++) {
			playerCurrentLife[i] = players[i].GetComponent<PlayerStatus>().GetCurrentLife();
		}

		for (int i = 0; i<players.Length; i++) {
			playerMaxLife[i] = players[i].GetComponent<PlayerStatus>().GetMaxLife();
		}

		//Find the gameobject HUD in the canvas
		foreach (Transform child in canvas) {
			if (child.CompareTag ("HUD") == true) {
				hud = child;
			}
		}

		//Initialize Player 1 UI
		CreateHeartUI(0);

		//Initialize Player 2 UI
		CreateHeartUI(1);
	}

	void Update() {

		//Update the players UI
		for (int i = 0; i < players.Length; i++) {

			//If the player exists
			if (players [i] != null) {
				playerCurrentLife [i] = players [i].GetComponent<PlayerStatus> ().GetCurrentLife ();

				if ((playerCurrentLife [i] <= playerMaxLife [i]) && (playerCurrentLife [i] != playerPreviousLife [i])) {
					DestroyHeartUI (i);
					CreateHeartUI (i);
					playerPreviousLife [i] = playerCurrentLife [i];
				}
			}
			else {
				DestroyHeartUI (i);
			}
		}
	}

	private void CreateHeartUI(int player) {

		//Player 1
		if (player == 0) {
			for (int i = 0; i < playerCurrentLife[0]; i++) {

				GameObject heart = (GameObject)Instantiate (heartPrefab,
					new Vector3(heartPosXP1 + (i * threshold), 
						heartPosY + canvasHight, 0.0f),Quaternion.identity, hud);

				heart.transform.name = "Heart" + i.ToString() + players[0].name;
				heartsListP1.Add(heart);
			}
		} 
		//Player 2
		else {
			for (int i = 0; i < playerCurrentLife [1]; i++) {

				GameObject heart = (GameObject)Instantiate (heartPrefab,
					new Vector3 (heartPosXP2 + (i * threshold), 
						heartPosY + canvasHight, 0.0f), Quaternion.identity, hud);

				heart.transform.name = "Heart" + i.ToString () + players [1].name;
				heartsListP2.Add (heart);
			}
		}
	}

	private void DestroyHeartUI(int player) {

		//Player 1
		if (player == 0) {
			foreach (GameObject heart in heartsListP1) {
				Destroy(heart);
			}
		}
		//Player 2
		else {
			foreach (GameObject heart in heartsListP2) {
				Destroy(heart);
			}
		}

	}
}
