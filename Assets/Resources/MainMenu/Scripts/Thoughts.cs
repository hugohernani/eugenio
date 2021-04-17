using UnityEngine;
using System.Collections;

public class Thoughts : MonoBehaviour {

	Animator animator;

	void Awake(){
		animator = GetComponent<Animator> ();
	}

	public void playCustomAnimationTrigger(string name){
		this.animator.SetTrigger (name);
	}

	public void playCustomAnimationTrigger(Animator animator, string name){
		animator.SetTrigger (name);
	}

}
