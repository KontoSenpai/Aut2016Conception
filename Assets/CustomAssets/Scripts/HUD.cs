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

	private float canvasHight;
	private float canvasWidth;

	private GameObject timer;

	public Transform canvas;
	public GameObject heartPrefab;			//Prefab of heart showing in the UI
	public GameObject countdownPrefab;

    public void Awake()
    {
      
		heartPosY = Screen.height-(heartPrefab.GetComponent<RectTransform>().rect.height/2)-10;
		canvasWidth = Screen.width/2;//canvas.GetComponent<RectTransform>().rect.width;

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
                GameObject heart = (GameObject)Instantiate(heartPrefab, new Vector3(heartPosXP1 + (i * threshold), heartPosY + canvasHight, 0.0f), Quaternion.identity, hud);
                heart.transform.name = "Heart" + i.ToString() + player.name;
                heartsListP1.Add(heart);
            }
        }
        else
        {
            for (int i = 0; i < playerCurrentLife; i++)
            {
                GameObject heart = (GameObject)Instantiate(heartPrefab, new Vector3(heartPosXP2 + (i * threshold), heartPosY + canvasHight, 0.0f), Quaternion.identity, hud);
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

						StartCoroutine (Delay (generator, round));
					}
				}
			} 
			else if (child.CompareTag ("RoundWinUI") && child.gameObject.activeInHierarchy == true)
			{
				child.gameObject.SetActive (false);
			}
		}
	}
	IEnumerator Delay(GameObject generator, int round)
	{		

		Time.timeScale = 0;

		//Wait five second before continuing
		float pauseEndTime = Time.realtimeSinceStartup + 5;

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
		}
	}
}