using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour {

	UnityAction clickAction;
	GameObject helpDialogCanvas;
	AudioManager audioManager;

	void Awake(){
		helpDialogCanvas = GameObject.Find ("HelpDialogCanvas");
		audioManager = GameObject.Find ("Manager").GetComponent<AudioManager> ();
	}

	// Use this for initialization
	void Start () {
		clickAction = () => {showHelpMessage(); audioManager.playOpenHelp();};
		gameObject.GetComponent<Button> ().onClick.AddListener (clickAction);
	}

	void showHelpMessage(){

		Canvas canvas = helpDialogCanvas.GetComponentInParent<Canvas> ();
		canvas.enabled = true;

	}
}