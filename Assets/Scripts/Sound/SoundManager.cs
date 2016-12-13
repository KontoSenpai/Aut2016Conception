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
	public AudioClip openClosePauseMenu;

    public AudioClip readySound;
    public AudioClip fightSound;
    public AudioClip quitGameSound;

    public AudioClip winRoundSound;
    public AudioClip winGameSound;

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
	
	}

    //GAMEPLAY SOUND
    public void PlayPickupSound( )
    {
        //AudioSource.PlayClipAtPoint(pickupSound, position, volumeRange);
		sourceSoundEffect.clip = pickupSound;
		sourceSoundEffect.Play ();
    }

    public void PlayHurtSound( )
    {
        //AudioSource.PlayClipAtPoint(hurtSound, position, volumeRange);
		sourceSoundEffect.clip = hurtSound;
		sourceSoundEffect.Play ();
    }

    public void PlaySlamSound( )
    {
        //AudioSource.PlayClipAtPoint(slamSound, position, volumeRange);
		sourceSoundEffect.clip = slamSound;
		sourceSoundEffect.Play ();
    }

    // MENUS SOUND
    public void PlayStartSound( )
    {
        //AudioSource.PlayClipAtPoint(startSound, position, volumeRange);
		sourceSoundEffect.clip = startSound;
		sourceSoundEffect.Play ();
    }
	public void PlayMenuNavigationSound( )
    {
        //AudioSource.PlayClipAtPoint(movingMenusSound, position, volumeRange);
		sourceSoundEffect.clip = movingMenusSound;
		sourceSoundEffect.Play ();
    }
    public void PlaySelectMenuSound( )
    {
        //AudioSource.PlayClipAtPoint(selectMenusSound, position, volumeRange);
		sourceSoundEffect.clip = selectMenusSound;
		sourceSoundEffect.Play ();
    }

    //SOUND GAME
    public void PlayReadySound( )
    {
        //AudioSource.PlayClipAtPoint(readySound, position, 1);
		sourceBackground.Play();
		sourceSoundEffect.clip =readySound;
		sourceSoundEffect.Play();
	
    }

    public void PlayFightSound( )
    {
     	//AudioSource.PlayClipAtPoint(fightSound, position, volumeRange);
		sourceSoundEffect.clip = fightSound;
		sourceSoundEffect.Play ();
    }

    public void PlayQuitGameSound( )
    {
        //AudioSource.PlayClipAtPoint(quitGameSound, position, volumeRange);
		sourceSoundEffect.clip = quitGameSound;
		sourceSoundEffect.Play ();
    }

    //WIN SOUND
    public void PlayWinRoundSound()
    {
        //AudioSource.PlayClipAtPoint(winRoundSound, position, volumeRangeMusic);
		sourceSoundEffect.clip = winRoundSound;
		sourceSoundEffect.Play ();
    }

    public void PlayWinGameSound( )
    {
        //AudioSource.PlayClipAtPoint(winGameSound, position, volumeRangeMusic);
		sourceSoundEffect.clip = winGameSound;
		sourceSoundEffect.Play ();
    }

	public void PlayOpenClosePauseMenu() {
		sourceSoundEffect.clip = openClosePauseMenu;
		sourceSoundEffect.Play ();
	}

    public void PlayPauseBackgroundSound()
	{
		if (sourceBackground.isPlaying) {
			sourceBackground.Pause ();
		} else {
			sourceBackground.Play ();
		}
	}
}