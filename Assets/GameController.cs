using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public Transform canvas;

	private GameObject[] players;
	private bool gameOver = false;

	void Start() {
	
	}
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Pause();
		}

		if (gameOver == true) {
			if (Input.GetKeyDown (KeyCode.R) || (Input.GetJoystickNames ().Length > 0 && Input.GetButtonDown("Submit"))) {
				Time.timeScale = 1;
				Application.LoadLevel (Application.loadedLevel);
			} 
			else if (Input.GetKeyDown (KeyCode.T)|| (Input.GetJoystickNames ().Length > 0 && Input.GetButtonDown("Cancel"))) {
				Quit ();
			}
		}
	}

	public void Pause () {

		//Get all child in the canvas
		foreach (Transform child in canvas) {

			//Check if the child is part of the pause menu and if it is displayed or not
			if (child.CompareTag ("PauseUI") && child.gameObject.activeInHierarchy == false) {
				child.gameObject.SetActive (true);
				Time.timeScale = 0;
			}
			else if (child.CompareTag ("PauseUI") && child.gameObject.activeInHierarchy == true) {
				child.gameObject.SetActive (false);
				Time.timeScale = 1;
			}


		}
	}

	public void Resume() {
		Pause ();
	}

	public void Quit() {
		Application.Quit ();
	}

	public void GameOver(GameObject player) {
		
		int idplayerDead = player.GetComponent<PlayerStatus> ().GetID();

		foreach (Transform child in canvas) {

			//Check if the child is part of the pause menu and if it is displayed or not
			if (child.CompareTag ("VictoryUI") && child.gameObject.activeInHierarchy == false) {
				child.gameObject.SetActive (true);

				foreach (Transform children in child) {
					
					//Check if the child is part of the pause menu and if it is displayed or not
					if (children.name.Contains(idplayerDead.ToString()) && child.gameObject.activeInHierarchy == true) {
						children.gameObject.SetActive (false);
						Time.timeScale = 0;
					}
				}
			}
		}
		gameOver = true;
	}
}