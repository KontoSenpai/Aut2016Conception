using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

	public int maxLife;
    public float invulnerabilityTime = 1.5f;
    private float invulnerabilityStart;
    private bool vulnerable = true;
	public int currentLife;
	private int playerID;

	// Use this for initialization
	void Awake ()
    {
		currentLife = maxLife;
	}

    void Update()
    {
        if (Time.time -invulnerabilityStart >= invulnerabilityTime && !vulnerable)
            vulnerable = true;
    }
    public void Hurt()
    {
		if (gameObject != null) {
			if (currentLife > 0) {
				currentLife--;
				vulnerable = false;
				invulnerabilityStart = Time.time;
			} 
			else {
				Destroy (gameObject);
				//ROUND OVER		
			}
		}
    }

    public bool IsVulnerable(){return vulnerable;}
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
}
