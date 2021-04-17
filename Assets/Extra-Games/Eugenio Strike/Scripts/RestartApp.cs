using UnityEngine;
using System.Collections;

public class RestartApp : MonoBehaviour {

	private GameScore gameScore = GameScore.getInstance;

	//EXIT BUTTON
	public void Close() {
		Application.LoadLevel("MainMenu");
		Destroy(GameObject.FindGameObjectWithTag("MainObject"));
	}

	//RESET BUTTON
	public void PlayAgain() {
		gameScore.resetAll();
		gameScore.increaseAttempts();
		gameScore.changeStatusButton();
		Destroy(GameObject.FindGameObjectWithTag("MainObject"));
		Application.LoadLevel(Application.loadedLevel);
	}
}
