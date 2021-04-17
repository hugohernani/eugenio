using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreInteraction : MonoBehaviour {
	public Text hitValue;
	public Text failValue;
	public Text step;

	UserInteraction userManagerInteraction;
	int stage;

	void Awake(){
		userManagerInteraction = GetComponent<UserInteraction> ();
	}

	void Start(){
		stage = userManagerInteraction.getCurrentStage ();
		step.text = "Fase " + stage + "\n" + 1 + "/10";
	}
	
	public void increaseHit(){
		int value = int.Parse (hitValue.text) + 1;
		hitValue.text = "" + value;
	}

	public void increaseFail(){
		int value = int.Parse (failValue.text) + 1;
		failValue.text = "" + value;
	}

	public void increaseStep(int newValue){
		step.text = "Fase " + stage + "\n" + newValue + "/10";
	}
	
}
