using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    //variable sound
    public AudioClip hurtSound;
    public AudioClip pickupSound;
    public AudioClip slamSound;
    public float volumeRange = 1f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


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
}