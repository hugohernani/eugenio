using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour {

	UnityAction clickAction;
	AudioManager audioManager;

	void Awake(){
		audioManager = GameObject.Find ("Manager").GetComponent<AudioManager> ();
	}
	// Use this for initialization
	void Start () {
		clickAction = () => {
			closeApplication ();
			audioManager.playCloseApplication();
		};
		gameObject.GetComponent<Button> ().onClick.AddListener (clickAction);
	}

	void closeApplication(){
		Application.LoadLevel ("MainMenu");
	}
	
}
