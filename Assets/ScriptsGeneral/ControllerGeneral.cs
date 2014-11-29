/**
* The controller is responsible for responding to user input and perform 
* interactions on the data model objects. The controller receives the input, 
* it validates the input and then performs the business operation that modifies 
* the state of the data model.
*
* @author David Cesar
*/

using UnityEngine;
using System.Collections;

public class ControllerGeneral {

	public ControllerGeneral () {
	}

	/**
	* @desc Make the button. If it is pressed, decidesCallFunction is called to
    *       decide which method has to be called.
	* @param style GUI Style are a collection of custom attributes for use with
   			 UnityGUI. For example: Button's Background image
	* @param pos Position on the screen where start the rectangle button
	* @param size 	
	* @see GUIStyle
	* @see Vector2
	* @see GUI#Button
	* @see Rect	
	*/
	public void createButton (GUIStyle style, Vector2 pos, 
								Vector2 size, string name){
		if (GUI.Button (new Rect (pos.x, pos.y, size.x, size.y), "", style)) {
			decidesCallFunction (name);
		}
	}

	private void decidesCallFunction (string nameButton) {
		switch (nameButton) {	
			case "buttonContinue":
				Model.getInstance().incLevelQuantoEh();	
				Application.LoadLevel("Estudar");	
				break;
			case "buttonExit":
				Application.LoadLevel("MainMenu");
				break;	
			default:
				break;
		}
	}

	public int getLevelQuantoEh(){
		return Model.getInstance().getLevelQuantoEh();
	}

	public int getQtyChlQuantoEh(){
		return Model.getInstance().getQtyChlQuantoEh();
	}
}
