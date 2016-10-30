﻿using UnityEngine;
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
		if (canvas.gameObject.activeInHierarchy == false) 
		{
			canvas.gameObject.SetActive (true);
			Time.timeScale = 0;
		} 
		else
		{
			canvas.gameObject.SetActive (false);
			Time.timeScale = 1;
		}
	}

	public void Resume() {
		Pause ();
	}

	public void Quit() {
		Application.Quit ();
	}
}