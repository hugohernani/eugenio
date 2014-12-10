using UnityEngine;
using System.Collections;

public class UserInteraction : MonoBehaviour {

	readonly int defaultQntStage = 10; // this is good for the pattern that we are using, which is 10. But if we change, this also need to be changed!

	AudioManager audioManager;
	MessageInteraction messageInteraction;
	ScoreInteraction scoreInteraction;
	EugenioInteraction eugenioInteraction;
	User currentUser;
	int total;
	int hitQty;
	int failQty;
	int step;
	int stage;

	void Awake(){
		currentUser = User.getInstance;
		audioManager = GetComponent<AudioManager> ();
		messageInteraction = GetComponent<MessageInteraction> ();
		scoreInteraction = GetComponent<ScoreInteraction> ();
		eugenioInteraction = GetComponent<EugenioInteraction> ();
		MainCategory category = (MainCategory)currentUser.CurrentCategory;
		stage = category.Stage;
	}

	// Use this for initialization
	void Start () {
		step = 1;
		int level = currentUser.Level_pet;
		total = 10;
		hitQty = 0;
		failQty = 0;
	
	}

	void Update(){
		if(hitQty == failQty){
			eugenioInteraction.EugenioNormal();
		}else if(hitQty > failQty){
			eugenioInteraction.EugenioHappy();
		}else{
			eugenioInteraction.EugenioSad();
		}

	}

	public int hit(){
		if (increaseStep(++hitQty)){
			audioManager.playHit ();
			scoreInteraction.increaseHit();
			messageInteraction.showHitMessage();
			eugenioInteraction.EugenioYesAnimation();
		}
		currentUser.TaskPoints = hitQty;
		currentUser.SaveTaskHits();
		return hitQty;
	}

	public int fail(){
		if(increaseStep()){
			audioManager.playFail ();
			scoreInteraction.increaseFail();
			messageInteraction.showFailMessage();
			eugenioInteraction.EugenioNoAnimation();
		}
		return failQty;
	}

	bool increaseStep(int hitQ = 0){
		// TODO REDO
		if(step == 10){
			Application.LoadLevel("EndTask");
			return false;
		}else{
			scoreInteraction.increaseStep(++step);
			return true;
		}
	}

	public int getCurrentStage(){
		return stage;
	}


}
