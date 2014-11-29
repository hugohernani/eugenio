using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LimitController : MonoBehaviour {

	public GameObject finalScoreText, restartButton, exitButton;
	ControlClass controlClass = ControlClass.getInstance;
	SpawnController spawnController;	//Just to have time's control

	void Update() {
		if(controlClass.GetActiveGame()) {
			if(Input.GetKeyDown(KeyCode.R)) {
				Application.LoadLevel(Application.loadedLevel);
				controlClass.ResetAll();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Eugenio") {
			finalScoreText.SetActive(true);
			UpdateScreenScore();

			controlClass.ChangeActiveGame(); //Not activated
			restartButton.SetActive(true);
			exitButton.SetActive(true);
		}
		else Destroy(other.gameObject);
	}

	void UpdateScreenScore() {
		finalScoreText.GetComponent<Text>().text = controlClass.GetScore().ToString() + " pontos";
	}
}
