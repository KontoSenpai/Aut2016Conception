using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public float time;

    public Text timerText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        time -= Time.deltaTime;

        if (time <= 0)
        {
            //do some shit for ending/ continue game
            time = 0;
        }

        int minute = (int) time / 60;
        var seconds = time % 60;


        timerText.text = string.Format("{0:00} : {1:00}", minute, seconds);
        Debug.Log(string.Format("{0:00} : {1:00}", minute, seconds));

       
	
	}
}
