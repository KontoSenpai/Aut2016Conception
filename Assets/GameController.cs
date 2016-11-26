using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public GameObject generator;
    public Transform canvas;

    private int round = 1;
	private List<GameObject> players;
    int roundWins = 0; // methode sale pour définir le vainqueur : si >0 joueur 2, sinon joueur 1
    
	private bool gameOver = false;
	private bool canPause = true;

    void Start()
    {
        players = new List<GameObject>();
        generator = Instantiate(generator, new Vector3(0, 0, 0), transform.rotation) as GameObject;
        generator.name = "Generator";
        generator.GetComponent<LevelGeneratorV3>().Refresh(round);
        players = generator.GetComponent<LevelGeneratorV3>().GetPlayers();
        //GetComponent<HUD>().Refresh();
    }

	void Update ()
    {
        if (Input.GetKeyDown("space"))
        {
            generator.GetComponent<LevelGeneratorV3>().Refresh(round);
            players = generator.GetComponent<LevelGeneratorV3>().GetPlayers();
            //GetComponent<HUD>().Refresh();
        }
        if (Input.GetKeyDown("g") && round < 3)
        {
            round++;
            generator.GetComponent<LevelGeneratorV3>().Refresh(round);
            players = generator.GetComponent<LevelGeneratorV3>().GetPlayers();
            //GetComponent<HUD>().Refresh();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
			Pause();

		if(gameOver)
        {
			if (Input.GetKeyDown (KeyCode.R) || (Input.GetJoystickNames ().Length > 0 && Input.GetButtonDown("Submit"))) {
				Time.timeScale = 1;
				Application.LoadLevel (Application.loadedLevel);
			} 
			else if (Input.GetKeyDown (KeyCode.T)|| (Input.GetJoystickNames ().Length > 0 && Input.GetButtonDown("Cancel"))) {
				Quit ();
			}
		}
	}
	public void Pause ()
    {
		if (canPause) {
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
	}

	public void Resume()
    {
        Pause ();
	}

	public void Restart() {
		Application.LoadLevel (Application.loadedLevel);
	}

	public void Quit()
    {
		Application.Quit ();
	}

    public void RoundEnd(GameObject deadPlayer)
    {
		foreach (GameObject player in GameObject.FindGameObjectsWithTag ("Player")) {
			if (player.GetComponent<PlayerStatus> ().GetID () != deadPlayer.GetComponent<PlayerStatus> ().GetID ()) {
				player.GetComponent<PlayerStatus> ().SetRoundWin ();
			}
		
		}
		if( round < 3)
		{
			round++;
			RoundOver ();
			generator.GetComponent<LevelGeneratorV3>().Refresh(round);
		}
		else if( !gameOver)
		{
			GameOver(deadPlayer);
		}

		/*
        if( deadPlayer.GetComponent<PlayerStatus>().GetID() == 1)
            roundWins += 1;
        else
            roundWins -= 1;
        if( round < 3)
        {
            round++;
            generator.GetComponent<LevelGeneratorV3>().Refresh(round);
        }
        else if( !gameOver)
        {
            GameOver();
        }
        */
    }

	private void RoundOver() {
		
	}

	private void GameOver(GameObject deadPlayer)
    {
		int deadPlayerID = deadPlayer.GetComponent<PlayerStatus>().GetID();
        
		/*if (roundWins < 0)
            idPlayer = 2;
        else
            idPlayer = 1;
		*/

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
						canPause = false;
						Time.timeScale = 0;
					}
				}
			}
		}
		gameOver = true;
	}
}