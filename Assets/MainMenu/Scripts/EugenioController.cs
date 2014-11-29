using UnityEngine;
using System.Collections;

public class EugenioController : MonoBehaviour {
	private Animator animator;
	private int level = 1;

	// To change	
	private bool soundIsOn = true;

	/* AudioClip properties */
	public AudioClip eatingSound;
	public AudioClip bathingSound;
	public AudioClip takingMedicineSound;
	public AudioClip evolutionSound;

	void Awake () {
		animator = GetComponent <Animator>();
	}
	
	public void playBathing (float time) {
		callSound(bathingSound);
		StartCoroutine(playAnimation("bBathing", time));
	}

	public void playEathing (float time) {
		callSound(eatingSound);
		StartCoroutine(playAnimation("bEating", time));
	}
	
	public void playTakingMedicine (float time) {
		callSound(takingMedicineSound);
		StartCoroutine(playAnimation("bTakingMedicine", time));
	}
	
	public void playEvolution (float time) {
		callSound(evolutionSound);
		StartCoroutine(playAnimation("bEvolution", time));
	}

	public void callScene (string scene) {
		Application.LoadLevel(scene);
	}
	
	private void callSound (AudioClip sound) {
		if(soundIsOn)
			audio.PlayOneShot(sound);
	}

	IEnumerator playAnimation (string var, float time) {
		animator.SetBool (var, true);
		yield return new WaitForSeconds(time);
		animator.SetBool (var, false);
	}
}
