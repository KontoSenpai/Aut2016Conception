using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class HUD : MonoBehaviour {
	
	private GameObject[] players; 			//Array of players
	private int[] playerCurrentLife; 		//Array of players's current life
	private int[] playerPreviousLife; 		//Array of players's current life
	private int[] playerMaxLife; 			//Array of players's current life

	private int threshold = 75;			//Space between hearts
	//Position X of Player 1 spawn of hearts (center screen - (distance between hearts + timer.length/2)
	private int heartPosXP1 = (Screen.width/2)-(75*4) ;			
	private int heartPosXP2 = (Screen.width/2)+(75*2);			//Position X of Player 2 spawn of hearts
	private float heartPosY;// = Screen.height/2-100;			//Position Y of Players spawn of hearts

	private List<GameObject> heartsListP1;	//List of Player 1 hearts currently shown
	private List<GameObject> heartsListP2;	//List of Player 2 hearts currently shown

	private Transform hud;
	private GameObject timer;

	public Transform canvas;
	public GameObject heartPrefab;			//Prefab of heart showing in the UI
	public GameObject countdownPrefab;

    public void Awake()
    {
      
		heartPosY = Screen.height-(heartPrefab.GetComponent<RectTransform>().rect.height/2)-10;

        heartsListP1 = new List<GameObject>();
        heartsListP2 = new List<GameObject>();
       
        //Find the gameobject HUD in the canvas
        foreach (Transform child in canvas)
        {
            if (child.CompareTag("HUD") == true)
                hud = child;
        }
    }

	public void ResetTimer() 
	{
		if (timer != null) {
			DestroyTimer ();
			CreateTimer ();
		} 
		else 
		{
			CreateTimer();
		}

	}

	public void CreateTimer() 
	{
		timer = (GameObject)Instantiate(countdownPrefab, new Vector3(Screen.width/2, Screen.height, 0.0f), Quaternion.identity, hud);
		timer.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0,-20);
	}

	public void DestroyTimer ()
	{
		Destroy (timer);
	}

	public void UpdateHearts(GameObject player)
    {
        //If the player exists
        if (player != null)
        {
                DestroyHeartUI(player);
                CreateHeartUI(player);
        }
        else
        {
            DestroyHeartUI(player);
        }
    }

	public void CreateHeartUI(GameObject player)
    {
        int playerCurrentLife = player.GetComponent<PlayerStatus>().GetCurrentLife();

        if (player.GetComponent<PlayerStatus>().GetID() == 1)
        {
            for (int i = 0; i < playerCurrentLife; i++)
            {
                GameObject heart = (GameObject)Instantiate(heartPrefab, new Vector3(heartPosXP1 + (i * threshold), heartPosY, 0.0f), Quaternion.identity, hud);
                heart.transform.name = "Heart" + i.ToString() + player.name;
                heartsListP1.Add(heart);
            }
        }
        else
        {
            for (int i = 0; i < playerCurrentLife; i++)
            {
                GameObject heart = (GameObject)Instantiate(heartPrefab, new Vector3(heartPosXP2 + (i * threshold), heartPosY, 0.0f), Quaternion.identity, hud);
                heart.transform.name = "Heart" + i.ToString() + player.name;
                heartsListP2.Add(heart);
            }
        }     
    }

    //Destroy all heart
	private void DestroyHeartUI(GameObject player)
    {
        if (player.GetComponent<PlayerStatus>().GetID() == 1)
        {
			if (heartsListP1.Count () != 0) {
				foreach (GameObject heart in heartsListP1) {				
					Destroy (heart);
				}
			}
        }
        else
        {
			if (heartsListP2.Count () != 0) {
				foreach (GameObject heart in heartsListP2) {
					Destroy (heart);
				}
			}
        }
    }

	public void DisplayPauseUI ()
	{

			//Get all child in the canvas
			foreach (Transform child in canvas) {

				//Check if the child is part of the pause menu and if it is displayed or not
				if (child.CompareTag ("PauseUI") && child.gameObject.activeInHierarchy == false) {
					child.gameObject.SetActive (true);
					Time.timeScale = 0;
				} else if (child.CompareTag ("PauseUI") && child.gameObject.activeInHierarchy == true) {
					child.gameObject.SetActive (false);
					Time.timeScale = 1;
				}
			}
	
	}
	public void DisplayRoundWinner(GameObject deadPlayer, GameObject generator, int round) {


		int deadPlayerID = deadPlayer.GetComponent<PlayerStatus>().GetID();

		Destroy (deadPlayer);

		foreach (Transform child in canvas)
		{
			//Check if the child is part of the pause menu and if it is displayed or not
			if (child.CompareTag ("RoundWinUI") && child.gameObject.activeInHierarchy == false) {
				child.gameObject.SetActive (true);
				foreach (Transform children in child) {
					//Check if the child is part of the pause menu and if it is displayed or not
					if (children.name.Contains (deadPlayerID.ToString ()) && child.gameObject.activeInHierarchy == true) {
						children.gameObject.SetActive (false);
						GetComponent<GameController> ().SetCanPause (false);

						StartCoroutine (DelayRoundWinner (generator, round));
					}
				}
			}
		}
	}
	IEnumerator DelayRoundWinner (GameObject generator, int round)
	{		

		Time.timeScale = 0;

		//Wait five second before continuing
		float pauseEndTime = Time.realtimeSinceStartup + GetComponent<SoundManager> ().winRoundSound.length;

		while (Time.realtimeSinceStartup < pauseEndTime)
		{
			yield return 0;
		}

		Time.timeScale = 1;
		generator.GetComponent<LevelGeneratorV3>().Refresh(round);
	}

	public void HidingRoundWinner() 
	{
		foreach (Transform child in canvas) 
		{
			//Check if the child is part of the pause menu and if it is displayed or not
			if (child.CompareTag ("RoundWinUI") && child.gameObject.activeInHierarchy == true) 
			{
				
				child.gameObject.SetActive (false);
			}
			foreach (Transform children in child) {
				//Check if the child is part of the pause menu and if it is displayed or not
				if (children.gameObject.activeInHierarchy == false) { //&& child.gameObject.activeInHierarchy == true) {
					children.gameObject.SetActive (true);

				}
			}
		}
	}

	public void DisplayGameOver(GameObject deadPlayer)
	{
		int deadPlayerID = deadPlayer.GetComponent<PlayerStatus>().GetID();

		foreach (Transform child in canvas)
		{
			//Check if the child is part of the pause menu and if it is displayed or not
			if (child.CompareTag ("VictoryUI") && child.gameObject.activeInHierarchy == false)
			{
				child.gameObject.SetActive (true);
				foreach (Transform children in child)
				{
					//Check if the child is part of the pause menu and if it is displayed or not
					if (children.name.Contains(deadPlayerID.ToString()) && child.gameObject.activeInHierarchy == true)
					{
						children.gameObject.SetActive (false);
						GetComponent<GameController> ().SetCanPause (false);
						Time.timeScale = 0;

						GetComponent<GameController> ().SetGameOver (true);
					}
				}
			}
		}
	}

	public void DisplayRoundBeginUI() {
		DisplayReadyImage ();

	}

	private void DisplayReadyImage() {
		foreach (Transform child in canvas)
		{
			//Check if the child is part of the pause menu and if it is displayed or not
			if (child.CompareTag ("RoundBeginUI") && child.gameObject.activeInHierarchy == false) {
				child.gameObject.SetActive (true);
				foreach (Transform children in child) {
					//Check if the child is part of the pause menu and if it is displayed or not
					if (children.name.Contains ("Fight") && child.gameObject.activeInHierarchy == true) {
						children.gameObject.SetActive (false);
						GetComponent<GameController> ().SetCanPause (false);
                        GetComponent<GameController>().PlaySound("Ready");

                        StartCoroutine (DelayReadyImage (children));
					}
				}
			}
		}
	}

	IEnumerator DelayReadyImage(Transform children)
	{		

		Time.timeScale = 0;
		//Wait five second before continuing
		float pauseEndTime = Time.realtimeSinceStartup + GetComponent<SoundManager> ().readySound.length + 1; // +1 to give a pause before saying GO

		while (Time.realtimeSinceStartup < pauseEndTime)
		{
			yield return 0;
		}

		DisplayFightImage (children);
		//Time.timeScale = 1;
	}

	private void DisplayFightImage(Transform fightImage) {
		

		foreach (Transform child in canvas)
		{
			//Check if the child is part of the pause menu and if it is displayed or not
			if (child.CompareTag ("RoundBeginUI") && child.gameObject.activeInHierarchy == true) {
				child.gameObject.SetActive (true);

				foreach (Transform children in child) {
					//Check if the child is part of the pause menu and if it is displayed or not
					if (children.name.Contains ("Ready") && child.gameObject.activeInHierarchy == true) {
						
						children.gameObject.SetActive (false);
						fightImage.gameObject.SetActive (true);
						GetComponent<GameController> ().SetCanPause (false);
                        GetComponent<GameController>().PlaySound("Fight");

                        StartCoroutine (DelayFightImage (child, children));
					}
				}
			}
		}
	}
	IEnumerator DelayFightImage(Transform roundBeginUI, Transform readyImage)
	{	
		//Wait five second before continuing
		float pauseEndTime = Time.realtimeSinceStartup + GetComponent<SoundManager> ().fightSound.length-0f;

		while (Time.realtimeSinceStartup < pauseEndTime)
		{
			yield return 0;
		}
		readyImage.gameObject.SetActive (true);
		roundBeginUI.gameObject.SetActive (false);
		GetComponent<GameController> ().SetCanPause (true);
		Time.timeScale = 1;
	}


}