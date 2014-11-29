using UnityEngine;
using System.Collections;

public class RestartApp : MonoBehaviour {

	private GameScore gameScore = GameScore.getInstance;

	//EXIT BUTTON
	public void RestartApplication() {
		gameScore.resetAll();
		Application.LoadLevel(Application.loadedLevel);
	}

	//RESET BUTTON
	public void PlayAgain() {
		gameScore.resetAll();
		gameScore.increaseAttempts();
		gameScore.changeStatusButton();
		Application.LoadLevel(Application.loadedLevel);
	}
}
