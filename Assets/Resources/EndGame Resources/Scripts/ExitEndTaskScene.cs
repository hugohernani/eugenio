using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class ExitEndTaskScene : MonoBehaviour {

	MessagePointReceiver mpReceiver;
	UnityAction clickAction;

	void Awake(){
		mpReceiver = GameObject.Find ("Manager").GetComponent<MessagePointReceiver> ();
		clickAction = () => {
						mpReceiver.GoMainScene ();};
	}

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Button> ().onClick.AddListener (clickAction);	
	}
	
}
