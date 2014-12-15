using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

	AudioSource audioSource;

	public AudioClip background;
	public AudioClip interaction;
	public AudioClip openHelp;
	public AudioClip restart;
	public AudioClip hit;
	public AudioClip fail;
	public AudioClip exit;

	public Sprite notMuteImage;
	public Sprite muteImage;

	void Awake(){
		audioSource = GameObject.Find("Main Camera").AddComponent<AudioSource> ();
		audioSource.clip = background;
		audioSource.volume = 0.6f;
		audioSource.loop = true;
		playBackground ();
	}


	public void stopBackground(){
		audioSource.Stop ();
	}

	public void playBackground(){
		audioSource.Play ();
	}

	public void playInteraction(){
		if(interaction != null)
			audioSource.PlayOneShot(interaction);
	}

	public void playOpenHelp(){
		if(openHelp != null)
			audioSource.PlayOneShot(openHelp);
	}


	public void playRestart(){
		if(restart != null)
			audioSource.PlayOneShot(restart);
	}


	public void playHit(){
		if(hit != null)
			audioSource.PlayOneShot(hit);
	}


	public void playFail(){
		if(fail != null)
		audioSource.PlayOneShot(fail);
	}

	public void playCloseApplication(){
		if(exit != null){
			audioSource.PlayOneShot(exit);
		}
	}

	public void playOneClip (AudioClip sound)
	{
		audioSource.PlayOneShot (sound);
	}

	public void MuteControl(){
		bool state = audioSource.mute;
		audioSource.mute = !state;
		Image imageSound = GameObject.Find ("SoundIcon").GetComponent<Image> ();
		if(state)
			imageSound.sprite = notMuteImage;
		else
			imageSound.sprite = muteImage;

	}

}
