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
	private int heartPosY = Screen.height/2-100;			//Position Y of Players spawn of hearts


	private List<GameObject> heartsListP1;	//List of Player 1 hearts currently shown
	private List<GameObject> heartsListP2;	//List of Player 2 hearts currently shown

	private Transform hud;

	private float canvasHight;
	private float canvasWidth;

	public Transform canvas;
	public GameObject heartPrefab;			//Prefab of heart showing in the UI

    public void Awake()
    {
      
		canvasHight = canvas.GetComponent<RectTransform>().rect.height;
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

	public void UpdateHUD(GameObject player)
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
            foreach (GameObject heart in heartsListP1)
            {
                Destroy(heart);
            }
        }
        else
        {
            foreach (GameObject heart in heartsListP2)
            {
                Destroy(heart);
            }
        }
    }
}
