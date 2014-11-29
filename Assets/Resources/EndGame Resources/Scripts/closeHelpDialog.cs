using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class closeHelpDialog : MonoBehaviour {

	UnityAction clickAction;

	void Awake(){
		clickAction = () => {closeHelpMessage();};
	}

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Button> ().onClick.AddListener (clickAction);
	}

	void closeHelpMessage(){
		gameObject.GetComponentInParent<Canvas> ().enabled = false;
	}
}
