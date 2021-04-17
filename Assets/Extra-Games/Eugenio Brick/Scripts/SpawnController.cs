using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpawnController : MonoBehaviour {

	ControlClass controlClass;

	public GameObject startButton;
	public GameObject brick;
	public Text timer;
	
	//Timer
	float timeLeft;

	void Start () {
		controlClass = ControlClass.getInstance;

		startButton.SetActive(!controlClass.GetActiveGame());

		timeLeft = 60.0f;
		UpdateTimer();

//		StartCoroutine(spawn());
	}

	void FixedUpdate() {
		if(controlClass.GetActiveGame()) {
			timeLeft -= Time.deltaTime;
			UpdateTimer();
			if(timeLeft < 0) {
				timeLeft = 0;
			}
		}
	}

	//COUROTINE
//	IEnumerator spawn() {
//
//		int time = 10;
//
//		while(time > 1) {
//			Vector3 spawnPosition = new Vector3(transform.position.x,
//			                                    transform.position.y,
//			                                    0.0f);
//			Quaternion spawnRotation = Quaternion.identity;
//			Instantiate(brick, spawnPosition, spawnRotation);
//
//			yield return new WaitForSeconds(1.0f);
//			time--;
//		}
//
//	}

	public void StartGame() {
		startButton.SetActive(false);
		controlClass.ChangeActiveGame();	//Playing
	}

	public void ResetGame() {
		controlClass.ResetAll();
		controlClass.ChangeActiveGame();

		Application.LoadLevel(Application.loadedLevel);
	}

	public void ExitGame() {
		Application.LoadLevel(Application.loadedLevel);

		controlClass.ResetAll();
	}

	void UpdateTimer() {
		timer.text = "Tempo:\n" +  Mathf.RoundToInt(timeLeft);
	}
}
