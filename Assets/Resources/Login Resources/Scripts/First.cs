using UnityEngine;
using System.Collections;

public class First : MonoBehaviour {

	public Texture2D logoImage;

	void Start(){
		logoImage = (Texture2D)Resources.Load ("Textures/UI/eugenio_logo");
	}

	void OnGUI () {
		float screenQuarterHeight = Screen.height * 1/6;
		float screenQuarterWidth = Screen.width * 1/4;
		// I have to change the size to make this automatically changeable in the devices
		GUILayout.BeginArea(new Rect(Screen.width/2-50,Screen.height/2-25, screenQuarterWidth, screenQuarterHeight));

//		GUILayout.BeginVertical ();
/*
		GUILayout.Label ("Width Screen: " + Screen.width);
		GUILayout.Label ("Height Screen: " + Screen.height);

		GUILayout.Label ("Height Quarter: " + screenQuarterHeight);
		GUILayout.Label ("Width Quarter: " + screenQuarterWidth);
		GUILayout.Label ("Half Width Screen: " + Screen.width/2);
		GUILayout.Label ("Half Height Screen: " + Screen.height/2);
*/

		GUILayout.Box (logoImage);

//		GUILayout.EndVertical ();


		GUILayout.EndArea ();

	}

}
