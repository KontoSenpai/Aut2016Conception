using UnityEngine;
using System.Collections;

public class DynamicTrapSpawner : MonoBehaviour {

    public GameObject[] dynamicTraps;
    public float gravityScale = 1;

    private GameObject dynamicTrap = null;
    private GameObject instance = null;

    private float deathTime = 0;

    /** Determine the type of the trap depending on the round
    *
    */
    public void InitializeCave()
    {
            dynamicTrap = dynamicTraps[0];
    }

    public void InitializeColiseum(bool left)
    {
        if (left)
            dynamicTrap = dynamicTraps[1];
        else
            dynamicTrap = dynamicTraps[2];
    }

    /** Game loop that gonna handle the spawn of the trap
    *
    */
	void Update ()
    {
        if( instance == null && Time.time - deathTime >= 1.5)
        {
            instance = Instantiate(dynamicTrap, transform.position, transform.rotation) as GameObject;
            instance.transform.parent = transform;
        }
	}

    public void SetDeathTimer(float time)
    {
        deathTime = time;
    }
}
