using UnityEngine;
using System.Collections;

public class EugenioController : MonoBehaviour {
	private Animator animator;
	private int level = 0;

	// To change
	private bool soundIsOn = true;

	/* AudioClip properties */
	public AudioClip eatingSound;
	public AudioClip bathingSound;
	public AudioClip takingMedicineSound;
	public AudioClip evolutionSound;
	public AudioClip negationSound;

	AudioSource audio;
	User user = User.getInstance;

	void Awake () {
		level = user.Level_pet + 1;
		GameObject mainCameraGO = GameObject.Find ("Main Camera");
		this.audio = mainCameraGO.GetComponent<AudioSource> ();
		animator = GetComponent <Animator>();
		animator.SetInteger ("evolution", level);

	}

	public bool playBathing (float time) {
		callSound(bathingSound);
		StartCoroutine(playAnimation("bBathing", time));
		return true;
	}

	public bool playEathing (float time) {
		callSound(eatingSound);
		StartCoroutine(playAnimation("bEating", time));
		return true;
	}
	
	public bool playTakingMedicine (float time) {
		callSound(takingMedicineSound);
		StartCoroutine(playAnimation("bTakingMedicine", time));
		return true;
	}
	
	public void playEvolution (float time) {
		callSound(evolutionSound);
		StartCoroutine(playAnimation("bEvolution", time));
	}

	public void playNegation ()
	{
		callSound (negationSound);
		animator.SetTrigger ("not");
	}

	public void callScene (string scene) {
		Application.LoadLevel(scene);
	}
	
	private void callSound (AudioClip sound) {
		if(soundIsOn)
			this.audio.PlayOneShot(sound);
	}


	IEnumerator playAnimation (string var, float time) {
		animator.SetBool (var, true);
		yield return new WaitForSeconds(time);
		animator.SetBool (var, false);
	}

	public void upgradeUser(){
		level = user.Level_pet;
		StartCoroutine("evolution");
	}

	void evolution(){
		callSound (evolutionSound);
		animator.SetInteger ("evolution", user.Level_pet+1);
	}

	public int Level {
		get {
			return this.level;
		}
	}

}
