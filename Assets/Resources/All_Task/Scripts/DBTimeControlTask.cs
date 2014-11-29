using UnityEngine;
using System;
using System.Collections;

public class DBTimeControlTask : MonoBehaviour {

	static float TIMESCENE;

	void Awake(){
		DontDestroyOnLoad (gameObject);
		TIMESCENE = 0.0f;
	}

	// Update is called once per frame
	void FixedUpdate () {
		TIMESCENE = Time.timeSinceLevelLoad;
	}

	public static void END_TASK(){

		DateTime dateUserEndTask = DateTime.Now;
		
		Debug.Log (dateUserEndTask.ToString());

		User user = User.getInstance;

//		user.SaveTaskHits ();

		user.SaveTaskDateAdnDuration(TIMESCENE, dateUserEndTask);

		Destroy(GameObject.FindGameObjectWithTag("TIMELOADING"));

	}

	public static void restartTime(){
		TIMESCENE = 0.0f;
	}

}
