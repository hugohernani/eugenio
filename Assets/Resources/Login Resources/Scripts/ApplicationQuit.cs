using UnityEngine;
using System.Collections;

public class ApplicationQuit : MonoBehaviour {

	void Awake(){
		DontDestroyOnLoad (gameObject);
	}

	void OnApplicationQuit(){
		DataAccess dataAccess = gameObject.AddComponent<DataAccess> ();

		dataAccess.createUpdateUserDoes ();

		Destroy (this.gameObject);
	}

}
