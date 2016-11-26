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

    public float volumeRange = 1f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //GAMEPLAY SOUND
    public void PlayPickupSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(pickupSound, position, volumeRange);
    }

    public void PlayHurtSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(hurtSound, position, volumeRange);
    }

    public void PlaySlamSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(slamSound, position, volumeRange);
    }

    // MENUS SOUND
    public void PlayStartSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(startSound, position, volumeRange);
    }
    public void PlaymMovingMenusSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(movingMenusSound, position, volumeRange);
    }
    public void PlaySelectMenusSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(selectMenusSound, position, volumeRange);
    }

    //SOUND GAME
    public void PlayReadySound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(readySound, position, volumeRange);
    }

    public void PlayFightSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(fightSound, position, volumeRange);
    }

    public void PlayQuitGameSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(quitGameSound, position, volumeRange);
    }

    //WIN SOUND
    public void PlayWinRoundSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(winRoundSound, position, volumeRange);
    }

    public void PlayWinGameSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(winGameSound, position, volumeRange);
    }

}