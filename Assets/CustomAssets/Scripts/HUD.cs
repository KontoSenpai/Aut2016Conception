using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class HUD : MonoBehaviour {
	
	private GameObject[] players;

	private int playerCurrentLife;
	private Transform hud;
	private int heartPosX = 50;
	private int heartPosY = -100;
	private int threshold = 100;
	private List<GameObject> heartsList;
	private int nbrHeartsUI;
	private float canvasHight;
	private float canvasWidth;
	private int playerMaxLife;

	public Transform canvas;
	public GameObject heartPrefab;


	void Start() {

		players = GameObject.FindGameObjectsWithTag("Player").OrderBy(go =>go.name).ToArray();

		canvasHight = canvas.GetComponent<RectTransform> ().rect.height;
		canvasWidth = canvas.GetComponent<RectTransform> ().rect.width;

		heartsList = new List<GameObject>();

		playerCurrentLife = players [0].GetComponent<PlayerStatus>().GetCurrentLife();

		playerMaxLife = players [0].GetComponent<PlayerStatus>().GetMaxLife();


		foreach (Transform child in canvas) {
			if (child.CompareTag ("HUD") == true) {
				hud = child;
			}
		}

		CreateHeartUI (); 
	}

	void Update() {

		playerCurrentLife = players [0].GetComponent<PlayerStatus>().GetCurrentLife();

		if (playerCurrentLife <= playerMaxLife) {
			DestroyHeartUI ();
			CreateHeartUI ();
		}
	}

	private void CreateHeartUI() {
		
		for (int i = 0; i < playerCurrentLife; i++) {

			GameObject heart = (GameObject)Instantiate (heartPrefab,
				new Vector3(heartPosX + (i * threshold), 
					heartPosY + canvasHight, 0.0f),Quaternion.identity, hud);

			heart.transform.name = "Heart" + i.ToString() + players[0].name;
			heartsList.Add(heart);
		}
	}

	private void DestroyHeartUI() {

		foreach (GameObject heart in heartsList) {
			Destroy(heart);
		}
	}



}
