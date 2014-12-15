using UnityEngine;
using System.Collections;

public class ApplicationQuit : MonoBehaviour {

	DataAccess dataAccess;
	public static bool connectionAvailable;

	void Awake(){
		DontDestroyOnLoad (gameObject);
		dataAccess = gameObject.AddComponent<DataAccess> ();
		InvokeRepeating ("saveData", 300f, 300f); // save data after each 10 minutes
	}

	void FixedUpdate(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
	}

	void saveData(){
		dataAccess.trySaveUserInformationsOnDB ();
	}

	void OnApplicationQuit(){
		User.getInstance.Logged_time = Time.timeSinceLevelLoad;
		CancelInvoke ("saveData");
		if(ApplicationQuit.connectionAvailable)
			dataAccess.trySaveUserInformationsOnDB ();
	}

}
