using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;


public class AudioIconButton : MonoBehaviour {

	AudioManager audioManager;
	UnityAction clickAction;

	void Awake(){
		audioManager = GameObject.Find ("Manager").GetComponent<AudioManager> ();
	}

	// Use this for initialization
	void Start () {
		clickAction = () => {audioManager.MuteControl();};
		gameObject.GetComponent<Button> ().onClick.AddListener (clickAction);

	}

}
