using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject generator;
    public Transform canvas;

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

		if (Input.GetButtonDown("Pause_P1") || Input.GetButtonDown("Pause_P2"))
        {
			if (canPause) {
				GetComponent<HUD> ().DisplayPauseUI ();
			}
		}
		if(gameOver)
        {
			if (Input.GetKeyDown (KeyCode.R) || (Input.GetJoystickNames ().Length > 0 && ( Input.GetButtonDown("Action_P1")) || (Input.GetButtonDown("Action_P2"))))
            {
				Time.timeScale = 1;
				Application.LoadLevel (Application.loadedLevel);
			} 
			else if (Input.GetKeyDown (KeyCode.T)|| (Input.GetJoystickNames ().Length > 0 && Input.GetButtonDown("Cancel")))
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
		if( round < 3)
		{
			round++;
			GetComponent<HUD>().DisplayRoundWinner(deadPlayer, generator, round);
            PlaySound("WinRound", new Vector3 (0.0f,0.0f,0.0f) );
        }
		else if( !gameOver)
		{
			GetComponent<HUD>().DisplayGameOver(deadPlayer);
            PlaySound("WinGame", new Vector3(0.0f, 0.0f, 0.0f));
        }
    }

	private void RoundOver(GameObject deadPlayer)
    {
		int deadPlayerID = deadPlayer.GetComponent<PlayerStatus>().GetID();

		Destroy (deadPlayer);

		foreach (Transform child in canvas)
		{
			//Check if the child is part of the pause menu and if it is displayed or not
			if (child.CompareTag ("RoundWinUI") && child.gameObject.activeInHierarchy == false)
			{
				print ("Round Over");
				child.gameObject.SetActive (true);
				foreach (Transform children in child)
				{
					//Check if the child is part of the pause menu and if it is displayed or not
					if (children.name.Contains(deadPlayerID.ToString()) && child.gameObject.activeInHierarchy == true)
					{
						
						children.gameObject.SetActive (false);
						canPause = false;
						Time.timeScale = 0;

						generator.GetComponent<LevelGeneratorV3>().Refresh(round);
						//StartCoroutine (Delay ());
					}
				}
			}
		}
	}

    public void PlaySound(string Nameobject, Vector3 position)
    {
        switch (Nameobject)
        {
            case "Pickup":
                gameObject.GetComponent<SoundManager>().PlayPickupSound(position);
                break;
            case "Hurt":
                gameObject.GetComponent<SoundManager>().PlayHurtSound(position);
                break;
            case "Slam":
                gameObject.GetComponent<SoundManager>().PlaySlamSound(position);
                break;
            case "Start":
                gameObject.GetComponent<SoundManager>().PlayStartSound(position);
                break;
            case "MoveMenus":
                gameObject.GetComponent<SoundManager>().PlaymMovingMenusSound(position);
                break;
            case "SelectMenus":
                gameObject.GetComponent<SoundManager>().PlaySelectMenusSound(position);
                break;
            case "Ready":
                gameObject.GetComponent<SoundManager>().PlayReadySound(position);
                break;
            case "Fight":
                gameObject.GetComponent<SoundManager>().PlayFightSound(position);
                break;
            case "Quit":
                gameObject.GetComponent<SoundManager>().PlayQuitGameSound(position);
                break;
            case "WinRound":
                gameObject.GetComponent<SoundManager>().PlayWinRoundSound(position);
                break;
            case "WinGame":
                gameObject.GetComponent<SoundManager>().PlayWinGameSound(position);
                break;

        }
    }

    IEnumerator Delay()
	{		
		yield return new WaitForSeconds(5);

		generator.GetComponent<LevelGeneratorV3>().Refresh(round);
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