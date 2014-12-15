using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour {

	UnityAction clickAction;
	AudioManager audioManager;

	void Awake(){
		clickAction = () => {
				restartScene ();audioManager.playRestart();};
		audioManager = GameObject.Find ("Manager").GetComponent<AudioManager> ();
	}

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Button> ().onClick.AddListener (clickAction);
	}

	void restartScene(){
		DBTimeControlTask.allowedToSave = false;
		Destroy (GameObject.FindGameObjectWithTag ("TIMELOADING"));
		Destroy (GameObject.FindGameObjectWithTag("TaskMainObject"));

		User user = User.getInstance;
		user.StartSavingTask ();
		Application.LoadLevel (Application.loadedLevel);
	}
}
