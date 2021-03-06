﻿using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {


    public int maxLife;
    public float invulnerabilityTime = 1.5f;

    public bool invulnerablePickup = false;
    public int currentLife;
	private int playerID;
	private int roundWin = 0;
    //Variables used to handle player damage behavior
    private bool vulnerable = true;
    private bool blink = false;
    //Variables linked to sprite rendering
    private SpriteRenderer rd;
    private float lastDisplay;
    private float displayDelay = 0.1f;

    // Use this for initialization
    void Awake ()
    {
		currentLife = maxLife;
        rd = GetComponentInChildren<SpriteRenderer>();

        //set particle system out
        gameObject.GetComponent<ParticleSystem>().enableEmission = false;
	}

    void Update()
    {
        if( blink)
        {
            if (rd.enabled && Time.time - lastDisplay >= displayDelay)
            {
                rd.enabled = false;
                lastDisplay = Time.time;
            }
            else if (!rd.enabled && Time.time - lastDisplay >= displayDelay)
            {
                rd.enabled = true;
                lastDisplay = Time.time;
            }
        }
        if( currentLife == 0)
        {
			GameObject gameController = GameObject.FindGameObjectWithTag ("GameController");
			if (gameController != null)
                gameController.GetComponent<GameController>().RoundEnd(gameObject);
        }
    }

    public void Hurt()
    {
		if (gameObject != null)
        {
			if (currentLife > 0)
            {
				currentLife--;
				vulnerable = false;
                blink = true;
                StartCoroutine(WaitForBlink(invulnerabilityTime));

                //Update player HUD
                GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
				gameController.GetComponent<HUD>().UpdateHearts(gameObject);

                //PlaySound 
                gameController.GetComponent<GameController>().PlaySound("Hurt");
            }
		}
    }

    private IEnumerator WaitForBlink(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        blink = false;
        vulnerable = true;
        rd.enabled = true;
    }


    public bool IsVulnerable(){return vulnerable;}
    public void SetVulnerability(bool isVulnerable) {vulnerable = isVulnerable; }
	public int GetCurrentLife(){return currentLife;}
	public int GetMaxLife(){return maxLife;}

	public void SetCurrentLife(int newLife)
    {
		if (currentLife + newLife > maxLife)
            currentLife = maxLife; 
		else
			currentLife += newLife;
	}

	public int GetID(){return playerID;}
	public void SetID(int id){playerID = id;}

	public int GetRoundWin(){return roundWin;}
	public void AddRoundWin(){roundWin++;}
}
