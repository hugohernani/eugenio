using UnityEngine;
using System;
using System.Collections;

public class DBTimeControlTask : MonoBehaviour {

	float TIMESCENE;

	void Awake(){
		DontDestroyOnLoad (gameObject);
		TIMESCENE = 0.0f;
	}

	// Update is called once per frame
	void FixedUpdate () {
		TIMESCENE = Time.timeSinceLevelLoad;
	}

	void OnDestroy(){
		User user = User.getInstance;
		
		user.SaveTaskHits ();
		
		user.SaveTaskDateAdnDuration(TIMESCENE);
	}
}
