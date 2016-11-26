using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

	public int maxLife;
    public float invulnerabilityTime = 1.5f;
    private float invulnerabilityStart;
    private bool vulnerable = true;
    public bool invulnerablePickup = false;
    public int currentLife;
	private int playerID;
	private int roundWin = 0;

	// Use this for initialization
	void Awake ()
    {
		currentLife = maxLife;
	}

    void Update()
    {
        if (Time.time - invulnerabilityStart >= invulnerabilityTime && !vulnerable && !invulnerablePickup)
            vulnerable = true;
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
				invulnerabilityStart = Time.time;
                //Update player HUD
                GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
				gameController.GetComponent<HUD>().UpdateHearts(gameObject);
            }
		}
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
	public void SetRoundWin(){roundWin++;}
}
