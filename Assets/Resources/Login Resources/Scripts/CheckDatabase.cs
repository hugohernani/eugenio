using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CheckDatabase : MonoBehaviour {

	public GameObject CanvasBlock;
	public Text textLoading;
	public Button cancelButton;
	public InputField username;
	public InputField password;
	public DataAccess dataAccess;

	void Awake(){
		username.Select ();
		messageDialog (false);
	}

	public void retrieveFromDatabase(){
		messageDialog (true);

		if(username.text != "" && password.text != ""){
			StartCoroutine (dataAccess.LoginUser (username.text, password.text, (info) =>{
			//StartCoroutine (dataAccess.LoginUser ("hugohernani0", "pass1234", (info) =>{
					if(info != "finished"){
					textLoading.text = info;
				}else{
					ApplicationQuit.connectionAvailable = true;
					Application.LoadLevel("MainMenu");
					Destroy(GameObject.FindGameObjectWithTag("MAIN_SCENE_OBJECT"));

				}
			}
			));
		}
	}

	public void messageDialog(bool canvasActive){
		CanvasBlock.SetActive (canvasActive);
		if(canvasActive){
			CanvasBlock.AddComponent<EugenioLoginPresentation> ();
			cancelButton.Select ();
		}else{
			Destroy (CanvasBlock.GetComponent<EugenioLoginPresentation> ());
			username.Select ();
		}

	}

}
