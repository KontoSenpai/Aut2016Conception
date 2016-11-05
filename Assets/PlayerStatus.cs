using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

	public int maxLife;

	public int currentLife;
	private int playerID;

	// Use this for initialization
	void Awake () {
		currentLife = maxLife;
	}

	public int GetCurrentLife() {
		return currentLife;
	}

	public int GetMaxLife() {
		return maxLife;
	}

	public void SetCurrentLife(int newLife) {
		
		if (currentLife + newLife > maxLife) {
			currentLife = maxLife;
		} 
		else {
			currentLife += newLife;
		}

	}

	public int GetID() {
		return playerID;
	}

	public void SetID(int id)
	{
		playerID = id;
	}
}
