using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public float timer;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		timer -= Time.deltaTime;

		if (timer <= 0)
        {
            //do some shit for ending/ continue game
			timer = 0;
        }

		int minute = (int) timer / 60;
		var seconds = timer % 60;

		gameObject.GetComponent<Text>().text = string.Format("{0:00} : {1:00}", minute, seconds);      
	}
}
