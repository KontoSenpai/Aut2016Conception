using UnityEngine;
using System.Collections;

public class SoundMenuManager : MonoBehaviour {



	public AudioClip startSound;
	public AudioClip menuNavigationSound;
	public AudioClip selectMenuOptionSound;

	public AudioClip backgroundSound;

	private AudioSource sourceBackground;
	private AudioSource sourceSoundEffect;

	// Use this for initialization
	void Awake () {
		AudioSource[] sources = GetComponents<AudioSource> ();

		sourceBackground = sources [0];
		sourceBackground.clip = backgroundSound;
		sourceBackground.playOnAwake = true;
		sourceBackground.loop = true;
		if (!sourceBackground.isPlaying) {
			sourceBackground.Play ();
		}

		sourceSoundEffect = sources [1];
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Vertical_P1")!=0) {
			PlayMenuNavigationSound ();
		}
		else if (Input.GetButtonDown("Action_P1")){
			PlayStartSound ();
		}
	}

	// MENUS SOUND
	public void PlayStartSound( )
	{
		//AudioSource.PlayClipAtPoint(startSound, position, volumeRange);
		sourceSoundEffect.clip = startSound;
		sourceSoundEffect.Play ();
	}
	public void PlayMenuNavigationSound()
	{
		//AudioSource.PlayClipAtPoint(movingMenusSound, position, volumeRange);
		sourceSoundEffect.clip = menuNavigationSound;
		sourceSoundEffect.Play ();
	}
	public void PlaySelectMenuSound( )
	{
		//AudioSource.PlayClipAtPoint(selectMenusSound, position, volumeRange);
		sourceSoundEffect.clip = selectMenuOptionSound;
		sourceSoundEffect.Play ();
	}
}
