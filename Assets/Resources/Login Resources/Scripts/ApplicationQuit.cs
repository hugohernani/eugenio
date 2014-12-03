﻿using UnityEngine;
using System.Collections;

public class ApplicationQuit : MonoBehaviour {

	DataAccess dataAccess;

	void Awake(){
		DontDestroyOnLoad (gameObject);
		dataAccess = gameObject.AddComponent<DataAccess> ();
		InvokeRepeating ("saveData", 100f, 100f); // save data after each 10 minutes
	}

	void saveData(){
		Debug.Log ("Called saveData");
		dataAccess.trySaveUserInformationsOnDB ();
	}

	void OnApplicationQuit(){
		CancelInvoke ("saveData");
		dataAccess.trySaveUserInformationsOnDB ();
	}

}
