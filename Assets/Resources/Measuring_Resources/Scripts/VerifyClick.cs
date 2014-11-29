using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class VerifyClick : MonoBehaviour {

	UnityAction unityClick;
	ManagerMeasuring manager;

	void Awake(){
		manager = GameObject.Find ("ManagerGame").GetComponent<ManagerMeasuring> ();
		unityClick = () => {
			manager.verifyAnswer ();};
	}

	// Use this for initialization
	void Start () {
		GetComponent<Button> ().onClick.AddListener (unityClick);
	}
	
}
