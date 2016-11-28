using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject generator;
    public Transform canvas;
	public GameObject pauseUI;

    private int round = 1;
    
	private bool gameOver = false;
	private bool canPause = true;
    private bool timerOut = false;


    void Start()
    {
        generator = Instantiate(generator, new Vector3(0, 0, 0), transform.rotation) as GameObject;
        generator.name = "Generator";
        generator.GetComponent<LevelGeneratorV3>().Refresh(round);


    }

	void Update ()
    {
        if (Input.GetKeyDown("space"))
            generator.GetComponent<LevelGeneratorV3>().Refresh(round);
        if (Input.GetKeyDown("g") && round < 3)
        {
            round++;
            generator.GetComponent<LevelGeneratorV3>().Refresh(round);
        }


		if (Input.GetButtonDown("Pause_P1"))
        {
			if (canPause) {
				GetComponent<HUD> ().DisplayPauseUI ();
				PlaySound ("PlayPauseBackground");
				PlaySound ("OpenClosePauseMenu");
			}
		}

		if ((Input.GetAxis("Vertical_P1") != 0) && pauseUI.activeInHierarchy == true)
		{

			PlaySound ("MoveMenus");
		}
		if (Input.GetButtonDown("Action_P1") && pauseUI.activeInHierarchy == true)
		{

			PlaySound ("SelectMenus");
		}

		if(gameOver)
        {

			if (Input.GetKeyDown (KeyCode.R) || (Input.GetJoystickNames ().Length > 0 && ( Input.GetButtonDown("Action_P1")) || (Input.GetButtonDown("Action_P2"))))
            {
				Time.timeScale = 1;
				Application.LoadLevel (Application.loadedLevel);
			} 
			else if (Input.GetKeyDown (KeyCode.T)|| (Input.GetJoystickNames ().Length > 0 && Input.GetButtonDown("Cancel_P1")))
				Quit ();
		}

        if(timerOut)
        {
            GameObject[] playersArray = GameObject.FindGameObjectsWithTag("Player");
            if (playersArray[0].GetComponent<PlayerStatus>().currentLife > playersArray[1].GetComponent<PlayerStatus>().currentLife)
            {
                RoundEnd(playersArray[1]);
            }
            else if (playersArray[0].GetComponent<PlayerStatus>().currentLife < playersArray[1].GetComponent<PlayerStatus>().currentLife)
            {
                RoundEnd(playersArray[0]);
            }
            else
                // RAJOUTER EGALITÉ
            timerOut = false;
        }
	}
	public void Pause ()
    {
		if (canPause) {
			//Get all child in the canvas
			foreach (Transform child in canvas)
            {
				//Check if the child is part of the pause menu and if it is displayed or not
				if (child.CompareTag ("PauseUI") && child.gameObject.activeInHierarchy == false)
                {
					child.gameObject.SetActive (true);
					Time.timeScale = 0;
				}
                else if (child.CompareTag ("PauseUI") && child.gameObject.activeInHierarchy == true)
                {

					PlaySound ("PlayPauseBackground");
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

	public void Restart()
    {
		Application.LoadLevel(Application.loadedLevel);
	}

	public void Quit()
    {
		Application.Quit ();
	}

	public void RoundStart() {

		GetComponent<HUD> ().HidingRoundWinner ();
		GetComponent<HUD> ().DisplayRoundBeginUI ();
	}

   
    public void RoundEnd(GameObject deadPlayer)
	{
		foreach (GameObject player in GameObject.FindGameObjectsWithTag ("Player")) {
			if (player.GetComponent<PlayerStatus> ().GetID () != deadPlayer.GetComponent<PlayerStatus> ().GetID ()) {
				player.GetComponent<PlayerStatus> ().AddRoundWin ();
			}
		
		}
		if (round < 3) {
			round++;

			GetComponent<HUD> ().DisplayRoundWinner (deadPlayer, generator, round);
			PlaySound ("PlayPauseBackground");
			PlaySound ("WinRound");
		} else if (!gameOver) {
			GetComponent<HUD> ().DisplayGameOver (deadPlayer);
			PlaySound ("PlayPauseBackground");
			PlaySound ("WinGame");
		}
	}
		
    public void PlaySound(string Nameobject)
    {
        switch (Nameobject)
        {
            case "Pickup":
                gameObject.GetComponent<SoundManager>().PlayPickupSound();
                break;
            case "Hurt":
                gameObject.GetComponent<SoundManager>().PlayHurtSound();
                break;
            case "Slam":
                gameObject.GetComponent<SoundManager>().PlaySlamSound();
                break;
            case "Start":
                gameObject.GetComponent<SoundManager>().PlayStartSound();
                break;
            case "MoveMenus":
                gameObject.GetComponent<SoundManager>().PlayMenuNavigationSound();
                break;
            case "SelectMenus":
                gameObject.GetComponent<SoundManager>().PlaySelectMenuSound();
                break;
            case "Ready":
                gameObject.GetComponent<SoundManager>().PlayReadySound();
                break;
            case "Fight":
                gameObject.GetComponent<SoundManager>().PlayFightSound();
                break;
            case "Quit":
                gameObject.GetComponent<SoundManager>().PlayQuitGameSound();
                break;
            case "WinRound":
                gameObject.GetComponent<SoundManager>().PlayWinRoundSound();
                break;
            case "WinGame":
                gameObject.GetComponent<SoundManager>().PlayWinGameSound();
                break;
			case "PlayPauseBackground":
			gameObject.GetComponent<SoundManager>().PlayPauseBackgroundSound();
			break;
			case "OpenClosePauseMenu":
			gameObject.GetComponent<SoundManager>().PlayOpenClosePauseMenu();
			break;

        }
    }


	private void GameOver(GameObject deadPlayer)
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
						canPause = false;
						Time.timeScale = 0;
					}
				}
			}
		}
		gameOver = true;
	}
	public void SetCanPause(bool value) { canPause = value; }

    // GETTER
    public bool GetTimerOut()
    {
        return timerOut;
    }

    //SETTER
    public void SetTimerOut(bool timer)
    {
        timerOut = timer;
    }

	public void SetGameOver (bool value) {
		gameOver = value;
	}

   
}