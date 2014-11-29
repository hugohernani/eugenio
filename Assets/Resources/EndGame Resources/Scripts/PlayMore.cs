using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayMore : MonoBehaviour {

	UnityAction clickAction;
	MessagePointReceiver mpReceiver;

	void Awake(){
		mpReceiver = GameObject.Find ("Manager").GetComponent<MessagePointReceiver> ();
		clickAction = () => {mpReceiver.GoBackScene();};
	}

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Button> ().onClick.AddListener (clickAction);
	}
	
}
