using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageInteraction : MonoBehaviour {

	public string[] happyMessages;
	public string[] sadMessages;
	public Image messageHolder;
	public Text messageText;

	public Animator anim;

	string message = "";

	void showMessage(){
		messageText.text = message;
		anim.SetTrigger ("Speak");
	}

	public void showMesssage(string customMessage){
		messageText.text = customMessage;
		anim.SetTrigger ("Speak");
	}

	public void showHitMessage(){
		if(happyMessages != null)
			message = happyMessages[Random.Range (0, happyMessages.Length)];
		else
			message = "Muito bem";
		showMessage ();
	}

	public void showFailMessage(){
		if(sadMessages != null)
			message = sadMessages[Random.Range (0, sadMessages.Length)];
		else
			message = "Voce errou";
		showMessage ();
	}
}
