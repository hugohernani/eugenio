using UnityEngine;
using System.Collections;

public class ApplicationQuit : MonoBehaviour {

	void Awake(){
		DontDestroyOnLoad (gameObject);
	}

	void OnApplicationQuit(){
		DataAccess dataAccess = gameObject.AddComponent<DataAccess> ();

		DBTimeControlTask.END_TASK ();

		dataAccess.createUpdateUserDoes ();

		Destroy (this.gameObject);
	}

}
