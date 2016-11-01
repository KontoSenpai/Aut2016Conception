using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public Transform canvas;
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Pause();
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
}