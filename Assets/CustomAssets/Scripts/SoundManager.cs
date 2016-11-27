using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    //variable sound
    public AudioClip hurtSound;
    public AudioClip pickupSound;
    public AudioClip slamSound;

    public AudioClip startSound;
    public AudioClip movingMenusSound;
    public AudioClip selectMenusSound;

    public AudioClip readySound;
    public AudioClip fightSound;
    public AudioClip quitGameSound;

    public AudioClip winRoundSound;
    public AudioClip winGameSound;

    public AudioClip backgroundSound;

    public float volumeRange;
    public float volumeRangeMusic = 1f;

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
	
	}

    //GAMEPLAY SOUND
    public void PlayPickupSound(Vector3 position)
    {
        //AudioSource.PlayClipAtPoint(pickupSound, position, volumeRange);
		sourceSoundEffect.clip = pickupSound;
		sourceSoundEffect.Play ();
    }

    public void PlayHurtSound(Vector3 position)
    {
        //AudioSource.PlayClipAtPoint(hurtSound, position, volumeRange);
		sourceSoundEffect.clip = hurtSound;
		sourceSoundEffect.Play ();
    }

    public void PlaySlamSound(Vector3 position)
    {
        //AudioSource.PlayClipAtPoint(slamSound, position, volumeRange);
		sourceSoundEffect.clip = slamSound;
		sourceSoundEffect.Play ();
    }

    // MENUS SOUND
    public void PlayStartSound(Vector3 position)
    {
        //AudioSource.PlayClipAtPoint(startSound, position, volumeRange);
		sourceSoundEffect.clip = startSound;
		sourceSoundEffect.Play ();
    }
    public void PlaymMovingMenusSound(Vector3 position)
    {
        //AudioSource.PlayClipAtPoint(movingMenusSound, position, volumeRange);
		sourceSoundEffect.clip = movingMenusSound;
		sourceSoundEffect.Play ();
    }
    public void PlaySelectMenusSound(Vector3 position)
    {
        //AudioSource.PlayClipAtPoint(selectMenusSound, position, volumeRange);
		sourceSoundEffect.clip = selectMenusSound;
		sourceSoundEffect.Play ();
    }

    //SOUND GAME
    public void PlayReadySound(Vector3 position)
    {
        //AudioSource.PlayClipAtPoint(readySound, position, 1);
		sourceBackground.Play();
		sourceSoundEffect.clip =readySound;
		sourceSoundEffect.Play();
	
    }

    public void PlayFightSound(Vector3 position)
    {
     	//AudioSource.PlayClipAtPoint(fightSound, position, volumeRange);
		sourceSoundEffect.clip = fightSound;
		sourceSoundEffect.Play ();
    }

    public void PlayQuitGameSound(Vector3 position)
    {
        //AudioSource.PlayClipAtPoint(quitGameSound, position, volumeRange);
		sourceSoundEffect.clip = quitGameSound;
		sourceSoundEffect.Play ();
    }

    //WIN SOUND
    public void PlayWinRoundSound(Vector3 position)
    {
        //AudioSource.PlayClipAtPoint(winRoundSound, position, volumeRangeMusic);
		sourceBackground.Pause();
		sourceSoundEffect.clip = winRoundSound;
		sourceSoundEffect.Play ();
    }

    public void PlayWinGameSound(Vector3 position)
    {
        //AudioSource.PlayClipAtPoint(winGameSound, position, volumeRangeMusic);
		sourceBackground.Stop();
		sourceSoundEffect.clip = winGameSound;
		sourceSoundEffect.Play ();
    }
	/*
    public void PlayBackgroundSound(Vector3 position)
    {
        //AudioSource.PlayClipAtPoint(backgroundSound, position, volumeRange);
		sourceSoundEffect.clip = backgroundSound;
		sourceSoundEffect.Play ();
    }
*/
}