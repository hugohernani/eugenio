using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {
	
	public GUIStyle buttonContinue;
	public GUIStyle buttonExit;

	private ControllerGeneral controller;
	private Vector2 posButtonContinue;
	private Vector2 posButtonExit;

	private Vector2 sizeButtonContinue;
	private Vector2 sizeButtonExit;

	// Use this for initialization
	void Start () {
		controller = new ControllerGeneral();	

		posButtonContinue = new Vector2();
		posButtonExit = new Vector2();

		sizeButtonContinue = new Vector2();
		sizeButtonExit = new Vector2();

		posButtonContinue.x = 10;
		posButtonContinue.y = 10;
		
		posButtonExit.x = 10;
		posButtonExit.y = 80;

		sizeButtonContinue.x = 50;
		sizeButtonContinue.y = 50;

		sizeButtonExit.x = 50;
		sizeButtonExit.y = 50;
	}
	
	// Update is called once per frame
	void OnGUI () {
		controller.createButton(buttonContinue, posButtonContinue, sizeButtonContinue, "buttonContinue");
		controller.createButton(buttonExit, posButtonExit, sizeButtonExit, "buttonExit");
	}	
}
