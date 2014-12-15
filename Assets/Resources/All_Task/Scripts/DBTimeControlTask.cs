using UnityEngine;
using System;
using System.Collections;

public class DBTimeControlTask : MonoBehaviour {

	float TIMESCENE;

	public static bool allowedToSave;

	void Awake () {
		DontDestroyOnLoad(gameObject);
		TIMESCENE = 0.0f;
	}

	// Update is called once per frame
	void FixedUpdate () {
		TIMESCENE = Time.timeSinceLevelLoad;
	}

	void OnDestroy(){
		if(DBTimeControlTask.allowedToSave){
			Debug.Log ("Destroying TASK_RUNNING.");
			
			User user = User.getInstance;
			
			user.SaveTaskDateAndDuration(TIMESCENE);
		}
		allowedToSave = !allowedToSave;
	}
}
