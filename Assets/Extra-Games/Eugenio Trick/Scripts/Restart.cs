using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {

	// Exit
	public void Close(){
		Application.LoadLevel ("MainMenu");
		Destroy (GameObject.FindGameObjectWithTag ("MainObject"));
	}

	public void RestartGame() {
		Application.LoadLevel(Application.loadedLevel);
		Destroy (GameObject.FindGameObjectWithTag ("MainObject"));
	}
}
