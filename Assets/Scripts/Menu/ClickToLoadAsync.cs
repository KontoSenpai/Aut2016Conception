using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClickToLoadAsync : MonoBehaviour {

	public Slider loadingBar;
	public GameObject loadingImage;

	private AsyncOperation async;

	public void ClickAsync(int level) {
		GameObject gameController = GameObject.FindGameObjectWithTag ("GameController");
		gameController.GetComponent<SoundMenuManager> ().PlayStartSound ();
		StartCoroutine (Delay (level));
	}

	IEnumerator Delay(int level)
	{	
		GameObject gameController = GameObject.FindGameObjectWithTag ("GameController");

		//Wait five second before continuing
		float pauseEndTime = Time.realtimeSinceStartup + gameController.GetComponent<SoundMenuManager> ().startSound.length-2.0f;

		while (Time.realtimeSinceStartup < pauseEndTime)
		{
			yield return 0;
		}

		loadingImage.SetActive (true);
		StartCoroutine (LoadLevelWithBar (level));
	}

	IEnumerator LoadLevelWithBar (int level)
	{
		async = Application.LoadLevelAsync (level);

		while (!async.isDone) {
			loadingBar.value = async.progress;
			yield return null;
		}
	}
}
