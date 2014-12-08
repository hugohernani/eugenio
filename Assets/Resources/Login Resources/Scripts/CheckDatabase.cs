using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CheckDatabase : MonoBehaviour {

	GameObject CanvasBlock;
	Text textLoading;

	InputField username;
	InputField password;

	public static string UriReach = "";

	private string message = "";
	private float timeMessage = 4;

	void Awake(){
		CanvasBlock = GameObject.Find ("CanvasBlock");
		textLoading = GameObject.Find ("LoadingMessage").GetComponent<Text> ();
		CanvasBlock.SetActive (false);
	}

	void Update(){
		if ((timeMessage -= Time.deltaTime) <= 0.0) {
			message = "";
			timeMessage = 4;
		}
	}

	void OnGUI(){
		if (message.Length != 0) {
			GUILayout.BeginArea(new Rect(Screen.width/2-50, Screen.height/2-25, 200f, 200f));
			GUILayout.BeginVertical();
			GUILayout.Box(message);
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}

	public void retrieveFromDatabase(){
		CanvasBlock.SetActive (true);

		username = GameObject.Find ("inputName").GetComponent<InputField> ();
		password = GameObject.Find ("inputSenha").GetComponent<InputField> ();

		DataAccess dataAccess = this.gameObject.AddComponent<DataAccess> ();

		//StartCoroutine (dataAccess.LoginUser (username.value, password.value, (info) =>{
		StartCoroutine (dataAccess.LoginUser ("hugohernani0", "pass1234", (info) =>{
				if(info != "finished"){
				textLoading.text = info;
			}else{
				Application.LoadLevel("MainMenu");
				Destroy(GameObject.FindGameObjectWithTag("MAIN_SCENE_OBJECT"));

			}
		}
		));

//		dataAccess.LoginUser (username.value, password.value);

	}

//	
//	void Start(){
//		bool isConnected = false;
//		StartCoroutine(checkInternetConnection((isConnected)=>{
//			Debug.Log(isConnected);
//		}));
//	}
//	checkInternetConnection(Action<bool> action){
//		WWW www = new WWW("http://google.com");
//		yield return www;
//		if (www.error != null) {
//			action (false);
//		} else {
//			action (true);
//		}
//	} 

//	void FixedUpdate(){
//		if(DataAccess.FINISH){
//			Application.LoadLevel("MainMenu");
//			DataAccess.FINISH = false;
//			Destroy(GameObject.FindGameObjectWithTag("MAIN_SCENE_OBJECT"));
//		}else{
//			textLoading.text = "Carregando... \n" + UriReach;
//		}
//	}

}
