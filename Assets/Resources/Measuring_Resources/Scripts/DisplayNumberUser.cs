using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayNumberUser : MonoBehaviour {

	Animator anim;
	Text text;

	void Awake(){
		anim = GetComponent<Animator> ();
		text = GetComponent<Text> ();
	}

	// Use this for initialization
	void Start () {	
	}
	

	public void display(string numberString){
		anim.SetBool ("DisplayNumber", false);
		anim.SetBool ("DisplayNumber", true);
		text.text = numberString;
	}

	public void stopDisplay(){
		anim.SetBool ("DisplayNumber", false);
		text.text = "";
	}
}
