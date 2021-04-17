using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class NumberClick : MonoBehaviour {

	UnityAction clickAction;
	ManagerMeasuring manager;

	void Awake(){
		manager = GameObject.Find ("ManagerGame").GetComponent<ManagerMeasuring> ();
	}

	// Use this for initialization
	void Start () {
		string buttonValue = GetComponentInChildren<Text> ().text;
		clickAction = () =>{manager.buttonSelected(buttonValue);};
		GetComponent<Button> ().onClick.AddListener (clickAction);
	}
	
}
